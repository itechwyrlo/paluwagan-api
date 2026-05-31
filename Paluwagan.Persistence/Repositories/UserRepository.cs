using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Enums;
using Paluwagan.Domain.Repositories;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.GenericRepository.EntityFramework;
using Paluwagan.Persistence.Data;

namespace Paluwagan.Persistence.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(PaluwaganDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser?> FindByAccountIdAsync(string accountId)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.AccountId == new AccountId(accountId));
        }

        public async Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken)
        {
            return await _userManager.Users
                .Include(u => u.RefreshTokens)
                .Where(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken))
                .FirstOrDefaultAsync();
        }

        //unused method
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userManager.Users
                .AnyAsync(u => u.Email == email);
        }

    

        //unused method
        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(UserRole role)
        {
            return await _userManager.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

       

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
            => await _userManager.AddToRoleAsync(user, role);

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
            => await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
            => await _userManager.ConfirmEmailAsync(user, token);

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
            => await _userManager.CheckPasswordAsync(user, password);

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<string> GenerateUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose)
            => await _userManager.GenerateUserTokenAsync(user, tokenProvider, purpose);

        public async Task<bool> VerifyUserTokenAsync(ApplicationUser user, string token, string tokenProvider, string purpose)
            => await _userManager.VerifyUserTokenAsync(user, tokenProvider, purpose, token);

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
            => await _userManager.ResetPasswordAsync(user, token, newPassword);

        //unused method
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
            => await _userManager.GetRolesAsync(user);

    }
}