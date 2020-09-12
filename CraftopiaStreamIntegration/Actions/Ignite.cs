using Oc;

namespace CraftopiaStreamIntegration.Actions
{
    public class Ignite: BaseAction
    {
        public override ActionResponse Handle()
        {
            OcPlMaster.Inst.Condition.startFire(OcPlMaster.Inst.transform.position);
            return ActionResponse.Done;
        }
    }
}