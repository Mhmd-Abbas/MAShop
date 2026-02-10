using MAShop.DAL.Data;
using MAShop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Respository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> HasUserReviewedProductAsync(string userId, int productId)
        {
            return await _context.Review.AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task<Review> CreateAsyc(Review review)
        {
            await _context.Review.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}
