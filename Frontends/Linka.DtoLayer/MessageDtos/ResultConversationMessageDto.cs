using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.MessageDtos
{
    public class ResultConversationMessageDto
    {
        public int UserMessageId { get; set; }

        public int? ConversationId { get; set; }

        public string SenderId { get; set; } = string.Empty;

        public string ReceiverId { get; set; } = string.Empty;

        public string MessageDetail { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime MessageDate { get; set; }
    }
}
