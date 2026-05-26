using Linka.SignalRRealTimeApi.Services;
using Linka.SignalRRealTimeApi.Services.SignalRCommentServices;
using Linka.SignalRRealTimeApi.Services.SignalRMessageServices;
using Microsoft.AspNetCore.SignalR;

namespace Linka.SignalRRealTimeApi.Hubs
{
    public class SignalRHub : Hub
    {
        private readonly ISignalRMessageService _signalRService;
        private readonly ISignalRCommentService _signalRCommentService;

        public SignalRHub(ISignalRMessageService signalRService, ISignalRCommentService signalRCommentService)
        {
            _signalRService = signalRService;
            _signalRCommentService = signalRCommentService;
        }
        public async Task SendStatisticCount(string id)
        {
            var getTotalCommentCount = _signalRCommentService.GetTotalCommentCount();
            await Clients.All.SendAsync("ReceiveCommentCount", getTotalCommentCount);

            var getTotalMessageCount = _signalRService.GetTotalMessageCountByReceiverId(id);
            await Clients.All.SendAsync("ReceiveMessageCount", getTotalMessageCount);
        }
    }
}
