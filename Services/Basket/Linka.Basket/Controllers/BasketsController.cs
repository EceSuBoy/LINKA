using Linka.Basket.Dtos;
using Linka.Basket.LoginService;
using Linka.Basket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Basket.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketIService
            _basketService;

        private readonly ILoginService
            _loginService;

        public BasketsController(
            IBasketIService basketService,
            ILoginService loginService)
        {
            _basketService =
                basketService;

            _loginService =
                loginService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var basket =
                await _basketService
                    .GetBasket(
                        _loginService.GetUserId);

            return Ok(basket);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBasket(
            BasketTotalDto basketTotalDto)
        {
            /*
             * UserId değerini browser veya WebUI belirlemiyor.
             * Giriş yapan kullanıcıya ait token üzerinden alıyoruz.
             */
            basketTotalDto.UserId =
                _loginService.GetUserId;

            await _basketService
                .SaveBasket(
                    basketTotalDto);

            return Ok(
                "Basket saved successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            await _basketService
                .DeleteBasket(
                    _loginService.GetUserId);

            return Ok(
                "Basket deleted successfully.");
        }

        [HttpDelete("ClearBasket")]
        public async Task<IActionResult> ClearBasket()
        {
            await _basketService
                .DeleteBasket(
                    _loginService.GetUserId);

            return Ok(
                "Basket cleared successfully.");
        }
    }
}