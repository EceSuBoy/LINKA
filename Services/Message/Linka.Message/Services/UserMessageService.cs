using AutoMapper;
using Linka.Message.DAL.Context;
using Linka.Message.DAL.Entities;
using Linka.Message.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Linka.Message.Services
{
    public class UserMessageService : IUserMessageService
    {
        private readonly MessageContext _messageContext;
        private readonly IMapper _mapper;

        public UserMessageService(MessageContext messageContext, IMapper mapper)
        {
            _messageContext = messageContext;
            _mapper = mapper;
        }

        public async Task CreateMessageAsync(CreateMessageDto createMessageDto)
        {
            var value = _mapper.Map<UserMessage>(createMessageDto);
            await _messageContext.UserMessages.AddAsync(value);
            await _messageContext.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int id)
        {
            var values = await _messageContext.UserMessages.FindAsync(id);
            _messageContext.UserMessages.Remove(values);
            await _messageContext.SaveChangesAsync();
        }

        public async Task<List<ResultMessageDto>> GetAllMessageAsync()
        {
            var values = await _messageContext.UserMessages.ToListAsync();
            return _mapper.Map<List<ResultMessageDto>>(values);
        }

        public async Task<GetByIdMessageDto> GetByIdMessageAsync(int id)
        {
            var values = await _messageContext.UserMessages.FindAsync(id);
            return _mapper.Map<GetByIdMessageDto>(values);
        }

        public async Task<List<ResultInboxMessageDto>> GetInboxMessageAsync(string id)
        {
            var values = await _messageContext.UserMessages.Where(x=>x.ReceiverId == id).ToListAsync();
            return _mapper.Map<List<ResultInboxMessageDto>>(values);
        }

        public async Task<List<ResultSentBoxMessageDto>> GetSendboxMessageAsync(string id)
        {
            var values = await _messageContext.UserMessages.Where(x => x.SenderId == id).ToListAsync();
            return _mapper.Map<List<ResultSentBoxMessageDto>>(values);
        }

        public async Task<int> GetTotalMessageCount()
        {
            int values = await _messageContext.UserMessages.CountAsync();
            return values;
        }

        public async Task<int> GetTotalMessageCountByReceiverId(string id)
        {
            var values = await _messageContext.UserMessages.Where(x => x.ReceiverId == id).CountAsync();
            return values;
        }

        public async Task UpdateMessageAsync(UpdateMessageDto updateMessageDto)
        {
            var values = _mapper.Map<UserMessage>(updateMessageDto);
            _messageContext.UserMessages.Update(values);
            await _messageContext.SaveChangesAsync();
        }

        public async Task CreateConversationMessageAsync(
    SendConversationMessageDto sendConversationMessageDto)
        {
            if (sendConversationMessageDto.ConversationId <= 0)
            {
                throw new ArgumentException(
                    "Conversation ID is required.");
            }

            if (string.IsNullOrWhiteSpace(
                    sendConversationMessageDto.SenderId) ||
                string.IsNullOrWhiteSpace(
                    sendConversationMessageDto.ReceiverId))
            {
                throw new ArgumentException(
                    "Sender and receiver IDs are required.");
            }

            if (string.IsNullOrWhiteSpace(
                    sendConversationMessageDto.MessageDetail))
            {
                throw new ArgumentException(
                    "Message content cannot be empty.");
            }

            if (sendConversationMessageDto.SenderId ==
                sendConversationMessageDto.ReceiverId)
            {
                throw new ArgumentException(
                    "A user cannot send a message to themselves.");
            }

            var conversation =
                await _messageContext.Conversations
                    .FirstOrDefaultAsync(x =>
                        x.ConversationId ==
                            sendConversationMessageDto.ConversationId);

            if (conversation == null)
            {
                throw new ArgumentException(
                    "Conversation could not be found.");
            }

            /*
             * Mesajı gönderen ve alan kullanıcıların gerçekten bu
             * konuşmaya ait olup olmadığını kontrol ediyoruz.
             */
            var senderBelongsToConversation =
                conversation.FirstUserId ==
                    sendConversationMessageDto.SenderId ||
                conversation.SecondUserId ==
                    sendConversationMessageDto.SenderId;

            var receiverBelongsToConversation =
                conversation.FirstUserId ==
                    sendConversationMessageDto.ReceiverId ||
                conversation.SecondUserId ==
                    sendConversationMessageDto.ReceiverId;

            if (!senderBelongsToConversation ||
                !receiverBelongsToConversation)
            {
                throw new ArgumentException(
                    "The sender or receiver does not belong to this conversation.");
            }

            var userMessage = new UserMessage
            {
                ConversationId =
                    sendConversationMessageDto.ConversationId,

                SenderId =
                    sendConversationMessageDto.SenderId.Trim(),

                ReceiverId =
                    sendConversationMessageDto.ReceiverId.Trim(),

                /*
                 * Sohbet ekranında her mesaj için ayrı subject
                 * yazdırmayacağız. Ancak eski tablon bu alanı zorunlu
                 * tuttuğu için sabit bir değer kaydediyoruz.
                 */
                Subject = "Product Discussion",

                MessageDetail =
                    sendConversationMessageDto.MessageDetail.Trim(),

                IsRead = false,

                MessageDate = DateTime.UtcNow
            };

            await _messageContext.UserMessages
                .AddAsync(userMessage);

            await _messageContext.SaveChangesAsync();
        }

        public async Task<List<ResultConversationMessageDto>>
    GetConversationMessagesAsync(int conversationId)
        {
            if (conversationId <= 0)
            {
                throw new ArgumentException(
                    "Conversation ID must be greater than zero.");
            }

            var conversationExists =
                await _messageContext.Conversations
                    .AnyAsync(x =>
                        x.ConversationId == conversationId);

            if (!conversationExists)
            {
                throw new KeyNotFoundException(
                    "Conversation could not be found.");
            }

            var values =
                await _messageContext.UserMessages
                    .Where(x =>
                        x.ConversationId == conversationId)
                    .OrderBy(x =>
                        x.MessageDate)
                    .Select(x =>
                        new ResultConversationMessageDto
                        {
                            UserMessageId = x.userMessageId,

                            ConversationId = x.ConversationId,

                            SenderId = x.SenderId,

                            ReceiverId = x.ReceiverId,

                            MessageDetail = x.MessageDetail,

                            IsRead = x.IsRead,

                            MessageDate = x.MessageDate
                        })
                    .ToListAsync();

            return values;
        }

        public async Task MarkConversationMessagesAsReadAsync(
    int conversationId,
    string receiverId)
        {
            if (conversationId <= 0)
            {
                throw new ArgumentException(
                    "Conversation ID must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(receiverId))
            {
                throw new ArgumentException(
                    "Receiver ID cannot be empty.");
            }

            var messages =
                await _messageContext.UserMessages
                    .Where(x =>
                        x.ConversationId == conversationId &&
                        x.ReceiverId == receiverId &&
                        x.IsRead == false)
                    .ToListAsync();

            if (!messages.Any())
            {
                return;
            }

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            await _messageContext.SaveChangesAsync();
        }
    }
}
