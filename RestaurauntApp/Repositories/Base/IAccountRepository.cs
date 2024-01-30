using RestaurauntApp.DTOS;

namespace RestaurauntApp.Repositories.Base
{
    public interface IAccountRepository
    {
        Task<UserDTO> GetAccountByIdAsync(int id);
        Task<int> DeleteAccountAsync(int id); 
        Task<int> UpdateAccountAsync(int id, UserDTO UserToUpdate);
    }
}