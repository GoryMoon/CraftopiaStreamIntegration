using System;
using System.Collections;
using Newtonsoft.Json;
using Oc;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class SpawnMob: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private string _id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "level")]
        private byte _level;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private int _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "radius")]
        private int _radius;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "despawn_time")]
        private float _despawnTime;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "add_player_level")]
        private bool _addPlayerLevel;
        
        public override ActionResponse Handle()
        {
            var player = OcPlMaster.Inst;
            var mobManager = SingletonMonoBehaviour<OcEmMng>.Inst;

            if (!Enum.TryParse<OcEmType>(_id, true, out var id))
            {
                AccessUtils.PopMessage(6, "Integration", "<color=\"red\">Invalid mob ID in spawn mob action!");
                return ActionResponse.Done;
            }

            for (var i = 0; i < _amount; i++)
            {
                for (var index = 0; index < 10; ++index)
                {
                    var radiusVector = OcUtility.calcCircleVec() * OcUtility.GaussianNormalizeRange(0.0f, _radius);
                    var spawnVector = player.transform.position + radiusVector + Vector3.up * 15f;
                    if (!Physics.Raycast(spawnVector, Vector3.down, out _, 200f, 1024))
                    {
                        var guid = mobManager.doSpawn_FreeSlot_CheckHost_WithRayCheck(id, false, spawnVector,  (byte) (_level + Math.Max(0, _addPlayerLevel ? player.PlLevelCtrl.Level.Value: 0)));
                        var em = mobManager.getEmFromGuid(guid);
                        em.SetCustomName(From);
                        if (_despawnTime > 0)
                        {
                            em.StartCoroutine(DeSpawn(_despawnTime, guid));
                        }
                    
                        break;
                    }
                }
            }

            return ActionResponse.Done;
        }

        private static IEnumerator DeSpawn(float time, int guid)
        {
            yield return new WaitForSeconds(time);
            var em = SingletonMonoBehaviour<OcEmMng>.Inst.getEmFromGuid(guid);
            if (em != null && em.Health.HP > 0)
            {
                em.deactivate();
            }
        }
    }

}