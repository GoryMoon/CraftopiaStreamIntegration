using Newtonsoft.Json;
using Oc;
using Oc.Item;
using Oc.Item.UI;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class GiveItem: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "level")]
        private int _level;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "enchantment_ids")]
        private int[] _enchantmentIds;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "durability_value")]
        private float _durabilityValue;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "durability_max")]
        private float _durabilityMax;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private byte _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop")]
        private bool _drop;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "spread")]
        private int _spread;

        public override ActionResponse Handle()
        {
            var player = OcPlMaster.Inst;

            var itemDataMng = SingletonMonoBehaviour<OcItemDataMng>.Inst;
            var item = itemDataMng.CreateItem(_id, _level, _enchantmentIds, _durabilityValue, _durabilityMax);
            if (itemDataMng.EmptyData == item.data)
            {
                AccessUtils.PopMessage(6, "Integration", "<color=\"red\">Invalid item ID in give item action!");
                return ActionResponse.Done;
            }

            if (_drop)
            {
                var itemMng = SingletonMonoBehaviour<OcDropItemMng>.Inst;
                itemMng.activate(new OcDropItemActivateInfo
                {
                    poolType = OcDropItemMng.PoolType.KillDrop_Em,
                    pos = player.transform.position + Vector3.up * 0.8f,
                    item = item,
                    stockNum = _amount,
                    vel = Utils.RandomVector3(_spread)
                }, false);
            }
            else
            {
                var inventory = SingletonMonoBehaviour<OcItemUI_InventoryMng>.Inst;
                inventory.TryTakeItem(item, _amount);
            }

            return ActionResponse.Done;
        }
    }
}