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
        public async Task<int> CreateAccountAsync(UserDTO newUser)
        {
            var rowsAffected = await connection.ExecuteAsync(
                 @"INSERT INTO Users (Name, Password) 
                  VALUES (@Name, @Password)",
                 param: newUser);

            return rowsAffected;
        }

        public Task<int> DeleteAccountAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetAccountByIdAsync(int id)
        {
            var user = await connection.QueryFirstOrDefaultAsync<UserDTO>(
                @"SELECT * FROM Users WHERE Id = @Id",
                new { Id = id });

            return user;
        }
         public async Task<bool> CheckLogin(string name, string password)
        {
            var user = await connection.QueryFirstOrDefaultAsync<UserDTO>(
                @"SELECT * FROM Users WHERE Name = @Name AND Password = @Password",
                new { Name = name, Password = password });

            return user != null;
        }
    
        public Task<int> UpdateAccountAsync(int id, UserDTO UserToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}