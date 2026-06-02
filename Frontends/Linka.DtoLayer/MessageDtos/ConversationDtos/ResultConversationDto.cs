using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.MessageDtos.ConversationDtos
{
    public class ResultConversationDto
    {
        public int ConversationId { get; set; }

        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }

        public string ProductId { get; set; }

        public int SourceCommentId { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
