using System;
using I2.Loc;
using Newtonsoft.Json;
using Oc;
using Oc.Item;
using Oc.Item.UI;

namespace CraftopiaStreamIntegration.Actions
{
    public class RepairTool: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;

        public override ActionResponse Handle()
        {
            var equipments = SingletonMonoBehaviour<OcItemUI_EquipmentMng>.Inst;
            var item = equipments.GetEquipItem(OcEquipSlot.WpMain);
            if (item is IOcBreakable breakable)
            {
                SingletonMonoBehaviour<OcItemUI_HotkeyMng>.Inst.RequireRefresh();
                breakable.ChangeDurability(_amount);
                SingletonMonoBehaviour<OcUI_SpeechBubbleHandler>.Inst.LocalPlMessage(LocalizationManager.GetTranslation("100_UI/Speech/Repair/Fix").Replace("[A]", item.DisplayName));
                return ActionResponse.Done;
            }

            Console.WriteLine("Trying later for item: " + item.DisplayName);
            TryLater(TimeSpan.FromSeconds(5));
            return ActionResponse.Retry;
        }
    }
}