using Linka.DtoLayer.DiscountDtos;
using Linka.DtoLayer.MessageDtos;
using Newtonsoft.Json;

namespace Linka.WebUI.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ResultInboxMessageDto>> GetInboxMessageAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("http://localhost:5000/services/Message/UserMessage/GetMessageInBox?id=" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<List<ResultInboxMessageDto>>();
            return values;
        }

        public async Task<List<ResultSendBoxMessageDto>> GetSendboxMessageAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("http://localhost:5000/services/Message/UserMessage/GetMessageSendBox?id=" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<List<ResultSendBoxMessageDto>>();
            return values;
        }

        public async Task<int> GetTotalMessageCountByReceiverId(string id)
        {
            var responseMessage = await _httpClient.GetAsync("UserMessage/GetTotalMessageCountByReceiverId?id=" +id);
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }

        public async Task<List<ResultConversationMessageDto>>
    GetConversationMessagesAsync(int conversationId)
        {
            var responseMessage =
                await _httpClient.GetAsync(
                    $"UserMessage/GetConversationMessages/{conversationId}");

            var content =
                await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Conversation messages could not be loaded: " +
                    $"{responseMessage.StatusCode} - {content}");
            }

            var values =
                JsonConvert.DeserializeObject<
                    List<ResultConversationMessageDto>>(content);

            return values ?? new List<ResultConversationMessageDto>();
        }

        public async Task SendConversationMessageAsync(
            SendConversationMessageDto sendConversationMessageDto)
        {
            var responseMessage =
                await _httpClient.PostAsJsonAsync(
                    "UserMessage/SendConversationMessage",
                    sendConversationMessageDto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Message could not be sent: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }

        public async Task MarkConversationMessagesAsReadAsync(
    int conversationId,
    string receiverId)
        {
            var responseMessage =
                await _httpClient.PutAsync(
                    $"UserMessage/MarkConversationMessagesAsRead/" +
                    $"{conversationId}/" +
                    $"{Uri.EscapeDataString(receiverId)}",
                    null);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Messages could not be marked as read: " +
                    $"{responseMessage.StatusCode} - {content}");
            }
        }
    }
}
