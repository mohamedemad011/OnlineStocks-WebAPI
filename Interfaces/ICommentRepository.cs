using FinShark.Models;

namespace FinShark.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment>CreateAsync(Comment comment);
        Task<Comment?>Update(int id,Comment comment);
        Task<Comment?> Delete(int id);
    }
}
