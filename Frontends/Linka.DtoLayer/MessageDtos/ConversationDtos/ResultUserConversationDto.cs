using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.MessageDtos.ConversationDtos
{
    public class ResultUserConversationDto
    {
        public int ConversationId { get; set; }

        public string OtherUserId { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public int SourceCommentId { get; set; }

        public string LastMessage { get; set; } = string.Empty;

        public DateTime? LastMessageDate { get; set; }

        public int UnreadMessageCount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
