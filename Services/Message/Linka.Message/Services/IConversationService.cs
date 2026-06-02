using Linka.Message.Dtos;

namespace Linka.Message.Services
{
    public interface IConversationService
    {
        Task<ResultConversationDto> GetOrCreateConversationAsync(
            CreateConversationDto createConversationDto);

        Task<List<ResultUserConversationDto>>
    GetConversationsByUserIdAsync(string userId);

        Task<ResultConversationDto> GetConversationByIdAsync(int id);
    }
}
