using RestaurauntApp.DTOS;

namespace RestaurauntApp.Repositories.Base
{
    public interface IAccountRepository
    {
        Task<int> CreateAccountAsync(UserDTO newUser);
        Task<UserDTO> GetAccountByIdAsync(int id);
        Task<int> DeleteAccountAsync(int id);
        Task<int> UpdateAccountAsync(int id, UserDTO UserToUpdate);
        Task<bool> CheckLogin(string name, string password);
    }
}