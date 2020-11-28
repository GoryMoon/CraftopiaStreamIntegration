using Newtonsoft.Json;
using Oc;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class RefillMana: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;

        public override ActionResponse Handle()
        {
            var health = OcPlMaster.Inst.HealthPl;
            health.setForceMP(Mathf.Min(health.MP + _amount, health.MaxMP));
            return ActionResponse.Done;
        }
    }
}