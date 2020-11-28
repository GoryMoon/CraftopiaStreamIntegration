using System.Collections.Generic;
using Newtonsoft.Json;
using Oc;
using Oc.Item;
using Oc.Item.UI;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class InventoryBomb: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "spread")]
        private int _spread;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop_equipment")]
        private bool _dropEquipment;

        private static OcEquipSlot[] EquipmentSlots =
        {
            OcEquipSlot.WpMain,
            OcEquipSlot.WpSub,
            OcEquipSlot.EqHead,
            OcEquipSlot.EqBody,
            OcEquipSlot.FlightUnit,
            OcEquipSlot.Ammo,
            OcEquipSlot.Accessory,
            OcEquipSlot.Accessory_Dummy01
        };
        
        public override ActionResponse Handle()
        {
            var player = OcPlMaster.Inst;
            var inventory = SingletonMonoBehaviour<OcItemUI_InventoryMng>.Inst;
            var stacks = inventory.GetStacksNotNull(false);
            var itemMng = SingletonMonoBehaviour<OcDropItemMng>.Inst;
            var equipmentList = new List<OcItemStack>();
            if (_dropEquipment)
            {
                var equipments = SingletonMonoBehaviour<OcItemUI_EquipmentMng>.Inst;
                EquipmentSlots.ForEach((slot, i) =>
                {
                    var stack = equipments.GetEquipStack(slot);
                    if (stack != null && !stack.IsEmpty) stack.TryUnequip();
                });
            }
            foreach (var stack in stacks)
            {
                if (!_dropEquipment && stack.ItemData.ItemType == ItemType.Equipment)
                {
                    equipmentList.Add(new OcItemStack_Temporary(stack));
                    continue;
                }
                
                itemMng.activate(new OcDropItemActivateInfo
                {
                    poolType = OcDropItemMng.PoolType.KillDrop_Em,
                    pos = player.transform.position + Vector3.up * 0.8f,
                    item = stack.item,
                    stockNum = stack.StackCount,
                    vel = Utils.RandomVector3(_spread)
                }, false);
            }
            
            inventory.ClearInventoryItem();
            foreach (var stack in equipmentList)
            {
                inventory.TryTakeItem(stack);
            }
            return ActionResponse.Done;
        }
    }
}