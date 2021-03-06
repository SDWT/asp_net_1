﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using WebStore.Controllers;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        private Mock<IConfiguration> _ConfigMock;
        private Mock<IProductData> _ProductMock;
        private Mock<ILogger<CatalogController>> _LoggerMock;

        [TestInitialize]
        public void TestSetup()
        {
            _ConfigMock = new Mock<IConfiguration>();
            _ProductMock = new Mock<IProductData>();
            _LoggerMock = new Mock<ILogger<CatalogController>>();
        }

        [TestMethod]
        public void Details_Returns_With_Correct_View()
        {
            // A-A-A = Arrange - Act - Assert
            #region Arrange - размещение данных

            const int expected_id = 1;
            const decimal expected_price = 10m;
            var expected_name = $"Item id {expected_id}";
            var expected_brand_name = $"Brand of item {expected_id}";

            _ProductMock
               .Setup(p => p.GetProductById(expected_id))
               .Returns<int>(id => new ProductDTO
               {
                   Id = id,
                   Name = $"Item id {id}",
                   ImageUrl = $"Image_id_{id}.png",
                   Order = 0,
                   Price = expected_price,
                   Brand = new BrandDTO
                   {
                       Id = id,
                       Name = $"Brand of item {id}"
                   },
                   Section = new SectionDTO
                   {
                       Id = id,
                       Name = $"Section of product {id}",
                       Order = 1
                   }
               });

            var controller = new CatalogController(_ProductMock.Object, _ConfigMock.Object);


            #endregion

            #region Act - выполнение тестируемого кода

            var result = controller.Details(expected_id, _LoggerMock.Object);

            #endregion

            #region Assert - проверка утверждений

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);

            Assert.Equal(expected_id, model.Id);
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_price, model.Price);
            Assert.Equal(expected_brand_name, model.Brand);

            #endregion
        }

        [TestMethod]
        public void Details_Returns_NotFound_if_Product_not_Exists()
        {
            #region Arrange

            const int expected_id = 1;

            _ProductMock
               .Setup(p => p.GetProductById(It.IsAny<int>()))
               .Returns(default(ProductDTO));

            var controller = new CatalogController(_ProductMock.Object, _ConfigMock.Object);

            #endregion

            #region Act

            var result = controller.Details(expected_id, _LoggerMock.Object);

            #endregion

            #region Assert

            Assert.IsType<NotFoundResult>(result);

            #endregion
        }

        [TestMethod]
        public void Shop_Returns_Correct_View()
        {
            #region Arrange - размещение данных

            var products = new[]
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "Product 1",
                    Order = 0,
                    Price = 10m,
                    ImageUrl = "product1.jpg",
                    Brand = new BrandDTO
                    {
                        Id = 1,
                        Name = "Brand of product 1"
                    },
                    Section = new SectionDTO
                    {
                        Id = 1,
                        Name = "Section of product 1",
                        Order = 1
                    }
                },
                new ProductDTO
                {
                    Id = 2,
                    Name = "Product 2",
                    Order = 1,
                    Price = 20m,
                    ImageUrl = "product2.jpg",
                    Brand = new BrandDTO
                    {
                        Id = 2,
                        Name = "Brand of product 2"
                    },
                    Section = new SectionDTO
                    {
                        Id = 2,
                        Name = "Section of product 2",
                        Order = 2
                    }
                }
            };

            _ProductMock
               .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
               .Returns<ProductFilter>(filter => new PagedProductDTO
               {
                   Products = products,
                   TotalCount = products.Length
               });

            var controller = new CatalogController(_ProductMock.Object, _ConfigMock.Object);

            const int expected_section_id = 1;
            const int expected_brand_id = 5;

            #endregion

            #region Act

            var result = controller.Shop(expected_section_id, expected_brand_id);

            #endregion

            #region Assert

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_section_id, model.SectionId);

            Assert.Equal("Brand of product 1", model.Products.First().Brand);

            #endregion
        }
    }
}
