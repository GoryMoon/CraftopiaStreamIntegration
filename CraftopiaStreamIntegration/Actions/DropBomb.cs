using System.Collections;
using Newtonsoft.Json;
using Oc;
using Oc.Em;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class DropBomb: BaseAction
    {
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private int _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "radius")]
        private int _radius;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage")]
        private float _damage;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage_structure")]
        private float _damageStructure;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "speed")]
        private float _speed;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "height")]
        private float _height;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop_delay")]
        private float _dropDelay;
        
        public override ActionResponse Handle()
        {
            var player = OcPlMaster.Inst;
            player.StartCoroutine(SpawnBarrels(player, _amount, _radius, _speed, _damage, _damageStructure, _height, _dropDelay));
            return ActionResponse.Done;
        }
        
        private static IEnumerator SpawnBarrels(OcCharacter player, float amount, float radius, float speed, float damage, float damageStructure, float height, float delay)
        {
            var transform = player.transform;
            var poolCtrl = AccessUtils.GetPoolCtrl(player);
            for (var i = 0; i < amount; i++)
            {
                var spawnVector = transform.position + OcUtility.calcCircleVec() * OcUtility.GaussianNormalizeRange(0.0f, radius) + Vector3.up * height;
                (poolCtrl.Place((int) OcEm.ShellPoolType.Barrel_Fall, spawnVector, transform.rotation) as OcShell)?.setup(new OcShell.ShootInfo
                {
                    shooterFront = -Vector3.up,
                    shooterUp = transform.up,
                    shootVecN = -Vector3.up,
                    degLimitPitch = 0.0f,
                    degLimitYaw = 0.0f,
                    speed = speed,
                    ownerHealth = null,
                    HitEfcType = OcEfcMng.GenaralEfcType.Explosion_Small,
                    damageMsg = {
                        motDamage = damage,
                        CriticalRate = 0.0f,
                        atk = damage,
                        attackAttribute = OcAttackAttribute.Explode,
                        attackType = OcAttackType.Explode,
                        InstallObjDmg = damageStructure,
                        damageReactionType = OcDamageReactionType.BlowAway
                    },
                    useAttitudeControl = false
                });
                if (delay > 0)
                {
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}