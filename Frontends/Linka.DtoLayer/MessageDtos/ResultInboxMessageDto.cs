using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.MessageDtos
{
    public class ResultInboxMessageDto
    {
        public int userMessageId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public bool IsRead { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
