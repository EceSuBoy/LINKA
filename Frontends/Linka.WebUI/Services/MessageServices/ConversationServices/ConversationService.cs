using Linka.DtoLayer.MessageDtos.ConversationDtos;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Linka.WebUI.Services.MessageServices.ConversationServices
{
    public class ConversationService : IConversationService
    {
        private readonly HttpClient _httpClient;

        public ConversationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultConversationDto> StartConversationAsync(
            CreateConversationDto createConversationDto)
        {
            var responseMessage =
                await _httpClient.PostAsJsonAsync(
                    "Conversations/StartConversation",
                    createConversationDto);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Conversation could not be started: " +
                    $"{responseMessage.StatusCode} - {content}");
            }

            var value =
                await responseMessage.Content
                    .ReadFromJsonAsync<ResultConversationDto>();

            if (value == null)
            {
                throw new Exception(
                    "Conversation API returned an empty response.");
            }

            return value;
        }

        public async Task<ResultConversationDto>
    GetConversationByIdAsync(int id)
        {
            var responseMessage =
                await _httpClient.GetAsync(
                    $"Conversations/GetConversationById/{id}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Conversation could not be loaded: " +
                    $"{responseMessage.StatusCode} - {content}");
            }

            var value =
                await responseMessage.Content
                    .ReadFromJsonAsync<ResultConversationDto>();

            if (value == null)
            {
                throw new Exception(
                    "Conversation API returned an empty response.");
            }

            return value;
        }

        public async Task<List<ResultUserConversationDto>>
    GetConversationsByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "User ID cannot be empty.");
            }

            var responseMessage =
                await _httpClient.GetAsync(
                    $"Conversations/GetConversationsByUserId/" +
                    $"{Uri.EscapeDataString(userId)}");

            var content =
                await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Conversations could not be loaded: " +
                    $"{responseMessage.StatusCode} - {content}");
            }

            var values =
                JsonConvert.DeserializeObject<
                    List<ResultUserConversationDto>>(content);

            return values ?? new List<ResultUserConversationDto>();
        }
    }
}