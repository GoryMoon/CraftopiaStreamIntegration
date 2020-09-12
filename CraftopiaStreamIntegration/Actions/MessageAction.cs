
namespace CraftopiaStreamIntegration.Actions
{
    public class MessageAction: BaseAction
    {
        private readonly string _message;
        
        public MessageAction(string message)
        {
            _message = message;
        }

        public override ActionResponse Handle()
        {
            AccessUtils.PopMessage(8, "Integration", $"{_message}");
            return ActionResponse.Done;
        }
    }
}