namespace Linka.WebUI.Areas.User.Models
{
    public class MessageIndexViewModel
    {
        public List<ConversationListItemViewModel> Conversations
        {
            get;
            set;
        } = new List<ConversationListItemViewModel>();
    }
}
