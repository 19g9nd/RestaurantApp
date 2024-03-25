using RestaurauntApp.DTOS;

namespace RestaurauntApp.Repositories.Base
{
    public interface IDiscountRepository
    {
        Task<bool> AddDiscountCode(DiscountCodeDTO discountCode);
    }
}