using System.ComponentModel;
using Newtonsoft.Json;

namespace CraftopiaActions
{
    public class InventoryBomb: BaseAction<InventoryBomb>
    {
        [DefaultValue(5)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "spread")]
        private int _spread;
        
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop_equipment")]
        private bool _dropEquipment;
    }
}