using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.Business.DTOs;
using myshop.Business.Services.IServices;
using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myshop.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(IEnumerable<UserDto> Users, int TotalCount)> GetUsersPagedAsync(string? search, int page, int pageSize)
        {
            var query = _userManager.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(u =>
                    (u.UserName != null && u.UserName.ToLower().Contains(term)) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)) ||
                    (u.FullName != null && u.FullName.ToLower().Contains(term)) ||
                    (u.Name != null && u.Name.ToLower().Contains(term)));
            }

            int totalCount = await query.CountAsync();

            var usersList = await query
                .OrderBy(u => u.Email)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in usersList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "Customer";

                bool isLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? user.Email ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName ?? user.Name,
                    Role = roleName,
                    IsLocked = isLocked,
                    LockoutEnd = user.LockoutEnd
                });
            }

            return (userDtos, totalCount);
        }

        public async Task<(bool Success, string Message)> ChangeRoleAsync(string userId, string currentAdminId)
        {
            if (string.Equals(userId, currentAdminId, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "You cannot modify your own role.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = currentRoles.Contains("Admin");

            if (isAdmin)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                if (!currentRoles.Contains("Customer"))
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                }
                return (true, $"User {user.Email} has been demoted to Customer.");
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "Customer");
                await _userManager.AddToRoleAsync(user, "Admin");
                return (true, $"User {user.Email} has been promoted to Admin.");
            }
        }

        public async Task<(bool Success, string Message)> ToggleLockoutAsync(string userId, string currentAdminId)
        {
            if (string.Equals(userId, currentAdminId, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "You cannot lock your own account.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "User not found.");
            }

            bool isCurrentlyLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;

            if (isCurrentlyLocked)
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, null);
                if (result.Succeeded)
                {
                    return (true, $"User {user.Email} account has been unlocked.");
                }
                return (false, "Failed to unlock account.");
            }
            else
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
                if (result.Succeeded)
                {
                    return (true, $"User {user.Email} account has been locked.");
                }
                return (false, "Failed to lock account.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(string userId, string currentAdminId)
        {
            if (string.Equals(userId, currentAdminId, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "You cannot delete your own account.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return (true, $"User {user.Email} was successfully deleted.");
            }

            return (false, "Failed to delete user.");
        }
    }
}
