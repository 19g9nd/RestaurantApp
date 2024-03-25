using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Data;
using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly RestaurantAppDbContext context;

        public DiscountRepository(RestaurantAppDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> AddDiscountCode(DiscountCodeDTO discountCode)
        {
            try
            {
                // Проверяем, существует ли уже код скидки с таким же кодом
                var existingDiscountCode = await context.DiscountCodes.FirstOrDefaultAsync(d => d.Code == discountCode.Code);
                if (existingDiscountCode != null)
                {
                    return false; // Код скидки с таким кодом уже существует
                }
                var newDiscountCode = new DiscountCode
                {
                    Code = discountCode.Code,
                    Value = discountCode.Value,
                    ValidFrom = discountCode.ValidFrom,
                    ValidTo = discountCode.ValidTo
                };

                // Добавляем новый код скидки
                context.DiscountCodes.Add(newDiscountCode);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding discount code: {ex.Message}");
                return false;
            }
        }
    }
}