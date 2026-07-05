using myshop.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myshop.Business.Services.IServices
{
    public interface IUserService
    {
        Task<(IEnumerable<UserDto> Users, int TotalCount)> GetUsersPagedAsync(string? search, int page, int pageSize);
        Task<(bool Success, string Message)> ChangeRoleAsync(string userId, string currentAdminId);
        Task<(bool Success, string Message)> ToggleLockoutAsync(string userId, string currentAdminId);
        Task<(bool Success, string Message)> DeleteUserAsync(string userId, string currentAdminId);
    }
}
