using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.ViewModels.BreadCrumbs;
using WebStore.Interfaces.Services;
using System.Xml.Serialization;
using WebStore.Controllers;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private IProductData _ProductData;

        public BreadCrumbsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        private void GetParameters(out BreadCrumbType Type, out int id, out BreadCrumbType FromType)
        {
            if (Request.Query.ContainsKey("SectionId"))
                Type = BreadCrumbType.Section;
            else if (Request.Query.ContainsKey("BrandId"))
                Type = BreadCrumbType.Brand;
            else
                Type = BreadCrumbType.None;

            if ((string)ViewContext.RouteData.Values["action"] == nameof(CatalogController.Details))
                Type = BreadCrumbType.Product;

            id = 0;
            FromType = BreadCrumbType.Section;
            int parseId;

            switch (Type)
            {
                default: throw new ArgumentOutOfRangeException();

                case BreadCrumbType.None: break;

                case BreadCrumbType.Section:
                    if (int.TryParse(Request.Query["SectionId"].ToString(), out parseId))
                    {
                        id = parseId;
                    }
                    break;
                case BreadCrumbType.Brand:
                    if (int.TryParse(Request.Query["BrandId"].ToString(), out parseId))
                    {
                        id = parseId;
                    }
                    break;
                case BreadCrumbType.Product:
                    if (int.TryParse(ViewContext.RouteData.Values["id"].ToString(), out parseId))
                    {
                        id = parseId;
                    }

                    if (Request.Query.ContainsKey("FromBrand"))
                    {
                        FromType = BreadCrumbType.Brand;
                    }
                    break;
            }
        }

        public IViewComponentResult Invoke()
        {
            GetParameters(out var Type, out var id, out var FromType);

            switch (Type)
            {
                case BreadCrumbType.Section:
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Section,
                            Id = id.ToString(),
                            Name = _ProductData.GetSectionById(id).Name
                        }
                    });
                case BreadCrumbType.Brand:
                    return View(new[]
                    {
                        new BreadCrumbViewModel
                        {
                            BreadCrumbType = BreadCrumbType.Brand,
                            Id = id.ToString(),
                            Name = _ProductData.GetBrandById(id).Name
                        }
                    });
                case BreadCrumbType.Product:
                    return View(GetProductBreadCrumbs(_ProductData.GetProductById(id), FromType));
                default:
                    return View(Array.Empty<BreadCrumbViewModel>());
            }
        }

        private IEnumerable<BreadCrumbViewModel> GetProductBreadCrumbs(ProductDTO Product, BreadCrumbType FromType) => new[]
            {
                new BreadCrumbViewModel
                {
                    BreadCrumbType = FromType,
                    Id = FromType == BreadCrumbType.Section
                         ? Product.Section.Id.ToString()
                         : Product.Brand.Id.ToString(),
                    Name = FromType == BreadCrumbType.Section
                           ? Product.Section.Name
                           : Product.Brand.Name
                },
                new BreadCrumbViewModel
                {
                    BreadCrumbType = BreadCrumbType.Product,
                    Id = Product.Id.ToString(),
                    Name = Product.Name
                },
            };
    }
}
