using AutoMapper;
using Linka.Message.DAL.Context;
using Linka.Message.DAL.Entities;
using Linka.Message.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Linka.Message.Services
{
    public class ConversationService : IConversationService
    {
        private readonly MessageContext _messageContext;
        private readonly IMapper _mapper;

        public ConversationService(
            MessageContext messageContext,
            IMapper mapper)
        {
            _messageContext = messageContext;
            _mapper = mapper;
        }

        public async Task<ResultConversationDto>
            GetOrCreateConversationAsync(
                CreateConversationDto createConversationDto)
        {
            if (string.IsNullOrWhiteSpace(
                    createConversationDto.FirstUserId) ||
                string.IsNullOrWhiteSpace(
                    createConversationDto.SecondUserId) ||
                string.IsNullOrWhiteSpace(
                    createConversationDto.ProductId))
            {
                throw new ArgumentException(
                    "User IDs and Product ID are required.");
            }

            if (createConversationDto.FirstUserId ==
                createConversationDto.SecondUserId)
            {
                throw new ArgumentException(
                    "A user cannot start a conversation with themselves.");
            }

            /*
             * ID değerlerini alfabetik sıraya koyuyoruz.
             *
             * Böylece:
             * Kerem → Ayşe
             * ve
             * Ayşe → Kerem
             *
             * aynı konuşma olarak değerlendirilir.
             */
            var orderedUserIds = new[]
                {
                    createConversationDto.FirstUserId.Trim(),
                    createConversationDto.SecondUserId.Trim()
                }
                .OrderBy(x => x, StringComparer.Ordinal)
                .ToArray();

            var firstUserId = orderedUserIds[0];
            var secondUserId = orderedUserIds[1];

            var existingConversation =
                await _messageContext.Conversations
                    .FirstOrDefaultAsync(x =>
                        x.FirstUserId == firstUserId &&
                        x.SecondUserId == secondUserId &&
                        x.ProductId ==
                            createConversationDto.ProductId);

            if (existingConversation != null)
            {
                return _mapper.Map<ResultConversationDto>(
                    existingConversation);
            }

            var newConversation = new Conversation
            {
                FirstUserId = firstUserId,
                SecondUserId = secondUserId,
                ProductId =
                    createConversationDto.ProductId.Trim(),
                SourceCommentId =
                    createConversationDto.SourceCommentId,
                CreatedDate = DateTime.UtcNow
            };

            await _messageContext.Conversations
                .AddAsync(newConversation);

            await _messageContext.SaveChangesAsync();

            return _mapper.Map<ResultConversationDto>(
                newConversation);
        }

        public async Task<List<ResultUserConversationDto>>
    GetConversationsByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "User ID is required.");
            }

            userId = userId.Trim();

            var values =
                await _messageContext.Conversations

                    /*
                     * Yalnızca giriş yapan kullanıcının dahil olduğu
                     * konuşmaları getiriyoruz.
                     */
                    .Where(x =>
                        x.FirstUserId == userId ||
                        x.SecondUserId == userId)

                    /*
                     * Entity'nin tamamını çekmek yerine yalnızca
                     * Messages sayfasında gereken alanları seçiyoruz.
                     */
                    .Select(x =>
                        new ResultUserConversationDto
                        {
                            ConversationId =
                                x.ConversationId,

                            /*
                             * Giriş yapan kişi FirstUserId ise karşı taraf
                             * SecondUserId değeridir. Değilse tam tersidir.
                             */
                            OtherUserId =
                                x.FirstUserId == userId
                                    ? x.SecondUserId
                                    : x.FirstUserId,

                            ProductId =
                                x.ProductId,

                            SourceCommentId =
                                x.SourceCommentId,

                            CreatedDate =
                                x.CreatedDate,

                            LastMessage =
                                x.UserMessages
                                    .OrderByDescending(m =>
                                        m.MessageDate)
                                    .Select(m =>
                                        m.MessageDetail)
                                    .FirstOrDefault()
                                ?? string.Empty,

                            LastMessageDate =
                                x.UserMessages
                                    .OrderByDescending(m =>
                                        m.MessageDate)
                                    .Select(m =>
                                        (DateTime?)m.MessageDate)
                                    .FirstOrDefault(),

                            /*
                             * Sadece bu kullanıcıya gönderilmiş ve henüz
                             * okunmamış mesajları sayıyoruz.
                             */
                            UnreadMessageCount =
                                x.UserMessages.Count(m =>
                                    m.ReceiverId == userId &&
                                    m.IsRead == false)
                        })
                    .ToListAsync();

            /*
             * En son mesaj gönderilen konuşma üstte görünsün.
             * Henüz mesajı olmayan konuşmalarda oluşturulma tarihini kullanıyoruz.
             */
            return values
                .OrderByDescending(x =>
                    x.LastMessageDate ?? x.CreatedDate)
                .ToList();
        }

        public async Task<ResultConversationDto>
    GetConversationByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "Conversation ID must be greater than zero.");
            }

            var conversation =
                await _messageContext.Conversations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.ConversationId == id);

            if (conversation == null)
            {
                throw new KeyNotFoundException(
                    "Conversation could not be found.");
            }

            return _mapper.Map<ResultConversationDto>(
                conversation);
        }
    }
}