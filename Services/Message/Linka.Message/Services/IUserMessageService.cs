using Linka.Message.Dtos;

namespace Linka.Message.Services
{
    public interface IUserMessageService
    {
        Task<List<ResultMessageDto>> GetAllMessageAsync();

        Task<List<ResultInboxMessageDto>>
            GetInboxMessageAsync(string id);

        Task<List<ResultSentBoxMessageDto>>
            GetSendboxMessageAsync(string id);

        Task CreateMessageAsync(
            CreateMessageDto createMessageDto);

        Task UpdateMessageAsync(
            UpdateMessageDto updateMessageDto);

        Task DeleteMessageAsync(int id);

        Task<GetByIdMessageDto>
            GetByIdMessageAsync(int id);

        Task<int> GetTotalMessageCount();

        Task<int>
            GetTotalMessageCountByReceiverId(string id);

        Task CreateConversationMessageAsync(
            SendConversationMessageDto sendConversationMessageDto);

        Task<List<ResultConversationMessageDto>>
    GetConversationMessagesAsync(int conversationId);

        Task MarkConversationMessagesAsReadAsync(
    int conversationId,
    string receiverId);
    }
}
