using FinShark.Dtos.Comment;
using FinShark.Models;

namespace FinShark.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                Id = comment.Id,
                StockId = comment.StockId,
                CreateBy=comment.AppUser.UserName,
                Title = comment.Title
            };
        }
        public static Comment ToCommentFromCreateDto(this CreateCommentDto comment,int Stock_id)
        {
            return new Comment
            {
                Content = comment.Content,
                Title = comment.Title,
                StockId = Stock_id,
                
            };
        }
        public static Comment ToCommentFromUpdateeDto(this UpdateCommentRequestDto comment, int Stock_id)
        {
            return new Comment
            {
                Content = comment.Content,
                Title = comment.Title,

            };
        }
    }
}
