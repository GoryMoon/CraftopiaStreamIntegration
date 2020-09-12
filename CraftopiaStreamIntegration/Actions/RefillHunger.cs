using Newtonsoft.Json;
using Oc;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class RefillHunger: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;

        public override ActionResponse Handle()
        {
            var health = OcPlMaster.Inst.Health;
            health.setForceSP(Mathf.Min(health.SP + _amount, health.MaxSP));
            return ActionResponse.Done;
        }
    }
}