using Linka.Basket.Dtos;
using Linka.Basket.LoginService;
using Linka.Basket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketIService _basketIService;
        private readonly ILoginService _loginService;

    
            public BasketsController(IBasketIService basketIService, ILoginService loginService)
            {
                _basketIService = basketIService;
                _loginService = loginService;
            }
    
            [HttpGet]
            public async Task<IActionResult> GetMyBasketDetail()
            {
                var user = User.Claims;
                var values= await _basketIService.GetBasket(_loginService.GetUserId);
                return Ok(values);
            }
    
            [HttpPost]
            public async Task<IActionResult> SaveMyBasket(BasketTotalDto basketTotalDto)
            {
                basketTotalDto.UserId = _loginService.GetUserId;
                await _basketIService.SaveBasket(basketTotalDto);
                return Ok("Changes in the basket is saved.");
            }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            await _basketIService.DeleteBasket(_loginService.GetUserId);
            return Ok("Basket is deleted.");
        }
    }
}
