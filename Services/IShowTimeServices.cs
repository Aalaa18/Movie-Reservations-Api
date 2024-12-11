using MovieApi.DTO;

namespace MovieApi.Services
{
    public interface IShowTimeServices
    {
        Task<List<string>> listshowtimes();
        Task<string> AddShowTime(ShowtimeDTO showtimedto);
        Task<string> RemoveShowTime(int id);
        Task<string> AddSeatsForShowtime(int id);
    }
}