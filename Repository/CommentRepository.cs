using FinShark.Data;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(u=>u.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var commentModel=await _context.Comments.Include(u=>u.AppUser).FirstOrDefaultAsync(u=>u.Id==id);
            if(commentModel == null) return null;
            return commentModel;
        }
        public async Task<Comment>CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        public async Task<Comment?>Update(int id,Comment comment)
        {
            var curr_comment = await _context.Comments.FindAsync(id);
            if(curr_comment == null) return null;
            curr_comment.Title=comment.Title;
            curr_comment.Content=comment.Content;
            await _context.SaveChangesAsync();
            return curr_comment;
        }
        public async Task<Comment?>Delete(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(u=>u.Id == id);
            if(comment == null) return null;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

    }
}
