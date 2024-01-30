using System.Data.SqlClient;
using Dapper;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
          private readonly SqlConnection connection;
        public AccountRepository(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
        
        public Task<int> DeleteAccountAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetAccountByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAccountAsync(int id, UserDTO UserToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}