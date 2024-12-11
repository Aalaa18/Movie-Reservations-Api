using MovieApi.DTO;
using MovieApi.Models;

namespace MovieApi.Services
{
    public interface IuserServices
    {
        //Task<string> addusers(string user_pass, string user_mail, string user_name, string user_type);
      //  bool checkAdmin(string name);
        Task<string> RemoveUser(string name);
        //Task<Appuser> GetUserbyId(int id);
        Task<Appuser> upadteDetails(DtoNewuser dtoNewuser);
        Task<List<DtoNewuser>> GetAllUsers();

    }
}