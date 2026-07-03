using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Business.DTOs;
using System;
using System.Collections.Generic;

namespace myshop.Entities.ViewModels
{
    public class ProductVM
    {
        public ProductDto Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
