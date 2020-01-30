using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WebStore.Domain.Models;
using Moq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using Assert = Xunit.Assert;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Product;
using System.Linq;

namespace WebStore.Services.Tests.Product
{
    [TestClass]
    public class CartServiceTests
    {
        private const int item_id_1 = 1;
        private const int item_id_2 = 2;

        private Cart _Cart;
        private Mock<IProductData> _ProductDataMock;
        private Mock<ICartStore> _CartStoreMock;

        [TestInitialize]
        public void SetupTest()
        {
            _Cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = item_id_1, Quantity = 1 },
                    new CartItem { ProductId = item_id_2, Quantity = 5 },
                }
            };

            _ProductDataMock = new Mock<IProductData>();

            _CartStoreMock = new Mock<ICartStore>();
            _CartStoreMock
               .Setup(c => c.Cart)
               .Returns(_Cart);
        }

        [TestMethod]
        public void Cart_Class_ItemsCount_Returns_Correct_Quantity()
        {
            const int expected_count = 6;

            var actual_count = _Cart.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            const int expected_count = 6;

            var cart_view_model = new CartViewModel
            {
                Items = new Dictionary<ProductViewModel, int>
                {
                    { new ProductViewModel {Id = 1, Name = "Product 1", Price = 0.5m}, 1 },
                    { new ProductViewModel {Id = 2, Name = "Product 2", Price = 1.5m}, 5 },
                }
            };

            var actual_count = cart_view_model.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }


        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>()
            };

            var product_data_mock = new Mock<IProductData>();

            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock
               .Setup(c => c.Cart)
               .Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            const int expected_id = 5;
            cart_service.AddToCart(expected_id);

            Assert.Equal(1, cart.ItemsCount);
            Assert.Single(cart.Items);
            Assert.Equal(expected_id, cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartService_RemoveFromCart_Remove_Correct_Item()
        {
            var cart_service = new CartService(_ProductDataMock.Object, _CartStoreMock.Object);

            cart_service.RemoveFromCart(item_id_1);

            Assert.Single(_Cart.Items);
            Assert.Equal(2, _Cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartService_RemoveAll_ClearCart()
        {
            var cart_service = new CartService(_ProductDataMock.Object, _CartStoreMock.Object);

            cart_service.RemoveAll();

            Assert.Empty(_Cart.Items);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            const int item_id = 2;

            var cart_service = new CartService(_ProductDataMock.Object, _CartStoreMock.Object);

            cart_service.DecrementFromCart(item_id);

            Assert.Equal(5, _Cart.ItemsCount);
            Assert.Equal(2, _Cart.Items.Count);
            Assert.Equal(item_id, _Cart.Items[1].ProductId);
            Assert.Equal(4, _Cart.Items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int item_id = 1;

            var cart_service = new CartService(_ProductDataMock.Object, _CartStoreMock.Object);

            cart_service.DecrementFromCart(item_id);

            Assert.Equal(5, _Cart.ItemsCount);

            Assert.Single(_Cart.Items);
        }

        [TestMethod]
        public void CartService_TransformFromCart_WorkCorrect()
        {
            var products = new List<ProductDTO>
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "Product 1",
                    Price = 1.1m,
                    Order = 0,
                    ImageUrl = "Product1.png",
                    Brand = new BrandDTO {Id = 1, Name = "Brand 1"}
                },
                new ProductDTO
                {
                    Id = 2,
                    Name = "Product 2",
                    Price = 2.1m,
                    Order = 0,
                    ImageUrl = "Product1.png",
                    Brand = new BrandDTO {Id = 1, Name = "Brand 1"}
                },
            };

            var product_data_mock = new Mock<IProductData>();
            product_data_mock
               .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(products);

            var cart_service = new CartService(product_data_mock.Object, _CartStoreMock.Object);

            var result = cart_service.TransformFromCart();

            Assert.Equal(6, result.ItemsCount);
            Assert.Equal(1.1m, result.Items.First().Key.Price);
        }
    }
}
