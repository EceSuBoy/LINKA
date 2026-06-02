using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.DtoLayer.MessageDtos.ConversationDtos
{
    public class CreateConversationDto
    {
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }
        public string ProductId { get; set; }
        public int SourceCommentId { get; set; }
    }
}
