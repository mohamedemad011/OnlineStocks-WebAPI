using FinShark.Extensions;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace FinShark.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository)
        {
            _stockRepository = stockRepository;
            _userManager = userManager;
            _portfolioRepository = portfolioRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            if (username == null) return BadRequest("Username Null");
            var appUser= await _userManager.FindByEmailAsync(username);
            var userPorto = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(userPorto);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string Symbol)
        {
            var email = User.GetUsername();
            var appUser= await _userManager.FindByEmailAsync(email);
            var stock =await _stockRepository.GetStockBySymbol(Symbol);
            if (stock == null) return BadRequest("No Stock With This Symbole");
            var UserPort = await _portfolioRepository.GetUserPortfolio(appUser);
            if (UserPort.Any(e => e.Symbol.ToLower() == Symbol.ToLower()))return BadRequest("Stock Added Before To Port");

            var PortModel = new Portfolio
            {
                StockId=stock.Id,
                AppUserId=appUser.Id,
            };
            await _portfolioRepository.CreateAsync(PortModel);
            return Created();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string Symbol)
        {
            var username= User.GetUsername();
            var appUser = await _userManager.FindByEmailAsync(username);

            var userPort = await _portfolioRepository.GetUserPortfolio(appUser);

            var filteredStock = userPort.Where(u => u.Symbol.ToLower() == Symbol.ToLower()).ToList();
            if (filteredStock.Count == 1)
            {
                await _portfolioRepository.DeleteAsync(appUser, Symbol);
            }
            else
            {
                return BadRequest("Stock Not In Your Port");
            }
            return Ok();
        }
    }
}
