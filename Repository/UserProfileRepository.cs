using BookHive.Data;
using BookHive.Interfaces;
using BookHive.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Repositories;
public class UserProfileRepository : IUserProfileRepository
{
    private readonly DataContext _context;

    public UserProfileRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task UpdateUserAsync(ApplicationUser user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
