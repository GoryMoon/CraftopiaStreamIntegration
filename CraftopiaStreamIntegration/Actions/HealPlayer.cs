using Newtonsoft.Json;
using Oc;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class HealPlayer: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;

        public override ActionResponse Handle()
        {
            var health = OcPlMaster.Inst.HealthPl;
            health.setForceHP(Mathf.Min(health.HP + _amount, health.MaxHP));
            return ActionResponse.Done;
        }
    }
}