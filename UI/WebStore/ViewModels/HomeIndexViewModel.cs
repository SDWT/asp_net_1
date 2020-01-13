using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.ViewModels
{
    public class HomeIndexViewModel
    {
        public CatalogViewModel Catalog { get; set; }

        //public RecommendViewModel Catalog { get; set; }

        //public CategoryTabViewModel Catalog { get; set; }
    }
}
