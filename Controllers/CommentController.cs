using FinShark.Dtos.Comment;
using FinShark.Extensions;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(UserManager<AppUser>userManager,IStockRepository stockRepository,ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments =await _commentRepository.GetAllAsync();
            var CommentDto = comments.Select(x => x.ToCommentDto());
            return Ok(CommentDto);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{Stock_id:int}")]
        public async Task<IActionResult> Create([FromRoute]int Stock_id, [FromBody]CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var username=User.GetUsername();
            var appUser=await _userManager.FindByEmailAsync(username);
            bool Stock_Exist = await _stockRepository.StockExist(Stock_id);
            if (!Stock_Exist) return NotFound();
            var commentModel = commentDto.ToCommentFromCreateDto(Stock_id);
            commentModel.UserId = appUser.Id;
            commentModel.AppUser= appUser;
            await _commentRepository.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById),new {id = Stock_id},commentModel.ToCommentDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id,[FromBody] UpdateCommentRequestDto updateComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepository.Update(id, updateComment.ToCommentFromUpdateeDto(id));
            if (comment == null) return NotFound();
            return Ok(comment.ToCommentDto());
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var commentModel = await _commentRepository.Delete(id);
            if (commentModel == null) return NotFound();
            return Ok(commentModel.ToCommentDto());
        }
    }
}
