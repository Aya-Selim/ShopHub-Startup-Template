using System;

namespace myshop.Business.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
