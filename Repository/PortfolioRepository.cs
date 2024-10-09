using FinShark.Data;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace FinShark.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;
        public PortfolioRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeleteAsync(AppUser appUser,string symbol)
        {
            var PortModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id & x.Stock.Symbol.ToLower() ==
            symbol.ToLower());
            if (PortModel == null) return null;
            _context.Portfolios.Remove(PortModel);
            await _context.SaveChangesAsync();
            return PortModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(s => s.AppUserId == user.Id).Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                Comments = stock.Stock.Comments,
                CompanyName = stock.Stock.CompanyName,
                Purchase =stock.Stock.Purchase,
                LastDiv=stock.Stock.LastDiv,
                Industry=stock.Stock.Industry,
                MarketCap=stock.Stock.MarketCap,
            }).ToListAsync();
        }
    }
}
