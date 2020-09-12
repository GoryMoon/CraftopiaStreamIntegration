using Newtonsoft.Json;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class MovePlayer: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount_vertical")]
        private float _amountVertical;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "no_fall_damage")]
        private float _noFallDamage;
        
        public override ActionResponse Handle()
        {
            var player = Oc.OcPlMaster.Inst;
            player.Gravity.ActionStateJumpCtrlVelocity = player.LastGndGravityVelH;
            
            player.Gravity.initGravityVelocity(Vector3.up * _amountVertical + Vector3.Scale(Utils.GetBoundedRandVector(_amount, _amount), Vector3.forward + Vector3.right));
            player.Gravity.resetGravity();
            player.Gravity.ignoreGravity();
            player.jumped();
            player.Health.setNoHitTimer(_noFallDamage);
            
            return ActionResponse.Done;
        }
    }
}