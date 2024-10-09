using FinShark.Data;
using FinShark.Dtos.Stock;
using FinShark.Helpers;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock> DeleteAsync(int id)
        {
          var stockModel = await _context.Stocks.FirstOrDefaultAsync(u=>u.Id == id);
            if (stockModel == null) return null;
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public  async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks =  _context.Stocks.Include(u=>u.Comments).ThenInclude(u=>u.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks=stocks.Where(u => u.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks=stocks.Where(u => u.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDesc ? stocks.OrderByDescending(s => s.Symbol):stocks.OrderBy(s=>s.Symbol);
                }
            }
            return  await stocks.ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.Include(u=>u.Comments).ThenInclude(u => u.AppUser).FirstOrDefaultAsync(u=>u.Id == id);
        }

        public async Task<Stock?> GetStockBySymbol(string Symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol==Symbol);
        }

        public async Task<bool> StockExist(int id)
        {
            return await _context.Stocks.AnyAsync(u=>u.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existStock == null) return null;
            existStock.ToStockFromUpdateDto(stockDto);
            await _context.SaveChangesAsync();
            return existStock;
        }
    }
}
