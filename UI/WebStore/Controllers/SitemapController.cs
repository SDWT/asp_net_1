﻿using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index([FromServices] IProductData ProductData)
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("Blog", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Shop", "Catalog")),
                new SitemapNode(Url.Action("Index", "WebAPITest"))
            };

            nodes.AddRange(ProductData.GetSections().Select(section => 
                new SitemapNode(Url.Action("Shop", "Catalog", new { SectionId = section.Id }))));

            //foreach (var brand in ProductData.GetBrands())
            //{
            //    nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new { BrandId = brand.Id })));
            //}
            nodes.AddRange(ProductData.GetBrands().Select(brand =>
                new SitemapNode(Url.Action("Shop", "Catalog", new { BrandId = brand.Id }))));

            nodes.AddRange(ProductData.GetProducts().Products.Select(product =>
                new SitemapNode(Url.Action("Details", "Catalog", new { product.Id }))));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}