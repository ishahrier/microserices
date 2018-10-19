 using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
 using System;
 using Microsoft.AspNetCore.Mvc.Rendering;
 using WebMvc.Models;
 using WebMvc.ViewModels;

 namespace WebMvc.ViewModels {
     public class CatalogIndexViewModel {
         public IEnumerable<CatalogItem> CatalogItems { get; set; }
         public IEnumerable<SelectListItem> Brands { get; set; }
         public IEnumerable<SelectListItem> Types { get; set; }
         public int? BrandFilterApplied { get; set; }
         public int? TypesFilterApplied { get; set; }
         public PaginationInfo PaginationInfo { get; set; }
     }
 }