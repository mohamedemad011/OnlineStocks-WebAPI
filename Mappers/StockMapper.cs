using FinShark.Dtos.Stock;
using FinShark.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinShark.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto
            {
                Id = stock.Id,
                CompanyName = stock.CompanyName,
                Industry = stock.Industry,
                LastDiv = stock.LastDiv,
                Purchase = stock.Purchase,
                MarketCap = stock.MarketCap,
                Symbol = stock.Symbol,
                Comments = stock.Comments.Select(x=>x.ToCommentDto()).ToList(),
            };
        }
        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stock) {
            return new Stock()
            {
                Symbol = stock.Symbol,
                CompanyName=stock.CompanyName,
                Industry=stock.Industry,
                LastDiv = stock.LastDiv,
                MarketCap=stock.MarketCap,
                Purchase=stock.Purchase,
            };
        }
        public static void ToStockFromUpdateDto(this Stock stock,UpdateStockRequestDto update) {

            stock.Symbol = update.Symbol;
            stock.CompanyName = update.CompanyName;
            stock.Industry = update.Industry;
            stock.LastDiv = update.LastDiv;
            stock.MarketCap = update.MarketCap;
            stock.Purchase = update.Purchase;          
        }
    }
}
