using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Enums;
using Paluwagan.GenericRepository.Abstractions;

namespace Paluwagan.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<ApplicationUser?> FindByAccountIdAsync(string accountId);
    Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken);
    //unused method
    Task<bool> EmailExistsAsync(string email);
    //unused method
    Task<List<ApplicationUser>> GetUsersByRoleAsync(UserRole role);
    //unused method

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    Task<string> GenerateUserTokenAsync(ApplicationUser user, string tokenProvider, string purpose);
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    
    Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
    Task<bool> VerifyUserTokenAsync(ApplicationUser user, string token, string tokenProvider, string purpose);
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
}
}