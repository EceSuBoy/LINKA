using Linka.DtoLayer.MessageDtos;
using Linka.DtoLayer.MessageDtos.ConversationDtos;
using Linka.WebUI.Areas.User.Models;
using Linka.WebUI.Models;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.MessageServices;
using Linka.WebUI.Services.MessageServices.ConversationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IConversationService _conversationService;
        private readonly IProductService _productService;

        public MessageController(
            IMessageService messageService,
            IUserService userService,
            IConversationService conversationService,
            IProductService productService)
        {
            _messageService = messageService;
            _userService = userService;
            _conversationService = conversationService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser =
                await _userService.GetUserInfo();

            var conversations =
                await _conversationService
                    .GetConversationsByUserIdAsync(
                        currentUser.Id);

            var model =
                new MessageIndexViewModel();

            /*
             * Aynı kullanıcıyla birden fazla ürün hakkında konuşulabilir.
             * IdentityServer'a aynı kullanıcı için gereksiz yere tekrar tekrar
             * istek göndermemek için küçük bir cache kullanıyoruz.
             */
            var userCache =
                new Dictionary<string, UserDetailViewModel>();

            foreach (var conversation in conversations)
            {
                var product =
                    await _productService
                        .GetByIdProductAsync(
                            conversation.ProductId);

                if (!userCache.TryGetValue(
                        conversation.OtherUserId,
                        out var otherUser))
                {
                    otherUser =
                        await _userService
                            .GetUserByIdAsync(
                                conversation.OtherUserId);

                    userCache[conversation.OtherUserId] =
                        otherUser;
                }

                var displayName =
                    $"{otherUser.Name} {otherUser.Surname}"
                        .Trim();

                model.Conversations.Add(
                    new ConversationListItemViewModel
                    {
                        ConversationId =
                            conversation.ConversationId,

                        OtherUserId =
                            conversation.OtherUserId,

                        OtherUserDisplayName =
                            string.IsNullOrWhiteSpace(displayName)
                                ? otherUser.Username
                                : displayName,

                        ProductId =
                            conversation.ProductId,

                        ProductName =
                            product?.ProductName ?? "Product",

                        LastMessage =
                            string.IsNullOrWhiteSpace(
                                conversation.LastMessage)
                                    ? "No messages yet."
                                    : conversation.LastMessage,

                        LastMessageDate =
                            conversation.LastMessageDate,

                        UnreadMessageCount =
                            conversation.UnreadMessageCount
                    });
            }

            return View(model);
        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userService.GetUserInfo();

            var values =
                await _messageService
                    .GetInboxMessageAsync(user.Id);

            return View(values);
        }

        public async Task<IActionResult> Sendbox()
        {
            var user = await _userService.GetUserInfo();

            var values =
                await _messageService
                    .GetSendboxMessageAsync(user.Id);

            return View(values);
        }

        /*
         * Yorum üzerindeki Send Message butonuna basıldığında çalışır.
         * Konuşma daha önce oluşturulmuşsa mevcut ConversationId döner.
         * Yoksa yeni bir conversation oluşturulur.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartConversation(
            string receiverUserId,
            string productId,
            int commentId)
        {
            if (string.IsNullOrWhiteSpace(receiverUserId))
            {
                return BadRequest(
                    "The selected comment is not connected to a registered user.");
            }

            if (string.IsNullOrWhiteSpace(productId))
            {
                return BadRequest(
                    "Product ID cannot be empty.");
            }

            var currentUser =
                await _userService.GetUserInfo();

            if (currentUser.Id == receiverUserId)
            {
                return BadRequest(
                    "You cannot start a conversation with yourself.");
            }

            var conversation =
                await _conversationService
                    .StartConversationAsync(
                        new CreateConversationDto
                        {
                            FirstUserId =
                                currentUser.Id,

                            SecondUserId =
                                receiverUserId,

                            ProductId =
                                productId,

                            SourceCommentId =
                                commentId
                        });

            return RedirectToAction(
                nameof(Chat),
                new
                {
                    id = conversation.ConversationId
                });
        }

        [HttpGet]
        public async Task<IActionResult> Chat(int id)
        {
            var currentUser =
                await _userService.GetUserInfo();

            var conversation =
                await _conversationService
                    .GetConversationByIdAsync(id);

            var userBelongsToConversation =
                conversation.FirstUserId == currentUser.Id ||
                conversation.SecondUserId == currentUser.Id;

            if (!userBelongsToConversation)
            {
                return Forbid();
            }

            await _messageService
                .MarkConversationMessagesAsReadAsync(
                    id,
                    currentUser.Id);

            var otherUserId =
                conversation.FirstUserId == currentUser.Id
                    ? conversation.SecondUserId
                    : conversation.FirstUserId;

            var messages =
                await _messageService
                    .GetConversationMessagesAsync(id);

            var otherUser =
    await _userService
        .GetUserByIdAsync(otherUserId);

            var otherUserDisplayName =
                $"{otherUser.Name} {otherUser.Surname}"
                    .Trim();

            if (string.IsNullOrWhiteSpace(
                    otherUserDisplayName))
            {
                otherUserDisplayName =
                    otherUser.Username;
            }

            var product =
                await _productService
                    .GetByIdProductAsync(conversation.ProductId);

            var model =
                new ChatViewModel
                {
                    ConversationId =
                        conversation.ConversationId,

                    CurrentUserId =
                        currentUser.Id,

                    OtherUserId =
                        otherUserId,

                    ProductId =
                        conversation.ProductId,

                    ProductName =
                        product?.ProductName ?? "Product",

                    Messages =
                        messages
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(
            int conversationId,
            string messageDetail)
        {
            if (string.IsNullOrWhiteSpace(messageDetail))
            {
                TempData["MessageError"] =
                    "Message content cannot be empty.";

                return RedirectToAction(
                    nameof(Chat),
                    new { id = conversationId });
            }

            var currentUser =
                await _userService.GetUserInfo();

            var conversation =
                await _conversationService
                    .GetConversationByIdAsync(conversationId);

            var userBelongsToConversation =
                conversation.FirstUserId == currentUser.Id ||
                conversation.SecondUserId == currentUser.Id;

            if (!userBelongsToConversation)
            {
                return Forbid();
            }

            var receiverId =
                conversation.FirstUserId == currentUser.Id
                    ? conversation.SecondUserId
                    : conversation.FirstUserId;

            await _messageService
                .SendConversationMessageAsync(
                    new SendConversationMessageDto
                    {
                        ConversationId =
                            conversationId,

                        SenderId =
                            currentUser.Id,

                        ReceiverId =
                            receiverId,

                        MessageDetail =
                            messageDetail.Trim()
                    });

            return RedirectToAction(
                nameof(Chat),
                new { id = conversationId });
        }
    }
}