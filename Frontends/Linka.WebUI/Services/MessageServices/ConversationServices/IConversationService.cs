using Linka.DtoLayer.MessageDtos.ConversationDtos;

namespace Linka.WebUI.Services.MessageServices.ConversationServices
{
    public interface IConversationService
    {
        Task<ResultConversationDto> StartConversationAsync(
            CreateConversationDto createConversationDto);

        Task<ResultConversationDto>
    GetConversationByIdAsync(int id);

        Task<List<ResultUserConversationDto>>
    GetConversationsByUserIdAsync(string userId);
    }
}
