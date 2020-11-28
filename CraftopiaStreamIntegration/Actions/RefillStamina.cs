using Newtonsoft.Json;
using Oc;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class RefillStamina: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;

        public override ActionResponse Handle()
        {
            var health = OcPlMaster.Inst.HealthPl;
            health.recoverStamina(Mathf.Min(health.ST + _amount, health.MaxST), true);
            return ActionResponse.Done;
        }
    }
}