using Linka.SignalRRealTimeApi.Services;
using Linka.SignalRRealTimeApi.Services.SignalRCommentServices;
using Linka.SignalRRealTimeApi.Services.SignalRMessageServices;
using Microsoft.AspNetCore.SignalR;

namespace Linka.SignalRRealTimeApi.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly ISignalRCommentService _signalRCommentService;

        public SignalRHub(ISignalRCommentService signalRCommentService)
        {
            _signalRCommentService = signalRCommentService;
        }
        public async Task SendStatisticCount()
        {
            var getTotalCommentCount = await _signalRCommentService.GetTotalCommentCount();
            await Clients.All.SendAsync("ReceiveCommentCount", getTotalCommentCount);
        }
    }
}
