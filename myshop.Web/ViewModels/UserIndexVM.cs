using myshop.Business.DTOs;
using System;
using System.Collections.Generic;

namespace myshop.Web.ViewModels
{
    public class UserIndexVM
    {
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public string CurrentUserId { get; set; } = string.Empty;
    }
}
