using Linka.WebUI.Models;
using Linka.WebUI.Services.CommentServices;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.MessageServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin
    .ViewComponents
    .AdminLayoutViewComponents
{
    public class
        _AdminLayoutHeaderComponentPartial
        : ViewComponent
    {
        private readonly IMessageService
            _messageService;

        private readonly IUserService
            _userService;

        private readonly ICommentService
            _commentService;

        public
            _AdminLayoutHeaderComponentPartial(
                IMessageService messageService,
                IUserService userService,
                ICommentService commentService)
        {
            _messageService =
                messageService;

            _userService =
                userService;

            _commentService =
                commentService;
        }

        public async Task<IViewComponentResult>
            InvokeAsync()
        {
            var user =
                await _userService
                    .GetUserInfo();

            var model =
                new AdminLayoutUserViewModel
                {
                    User =
                        user
                };

            /*
             * Bir yardımcı mikroservis geçici olarak kapalıysa
             * bütün admin paneli çökmemeli.
             */
            try
            {
                model.MessageCount =
                    await _messageService
                        .GetTotalMessageCountByReceiverId(
                            user.Id);
            }
            catch
            {
                model.MessageCount =
                    0;
            }

            try
            {
                model.CommentCount =
                    await _commentService
                        .GetTotalCommentCount();
            }
            catch
            {
                model.CommentCount =
                    0;
            }

            return View(model);
        }
    }
}