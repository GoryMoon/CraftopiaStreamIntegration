using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace CraftopiaActions
{
    public class GiveItem: BaseAction<GiveItem>
    {
        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "level")]
        private int _level;
        
        [DefaultValue(new int[0])]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "enchantment_ids")]
        private int[] _enchantmentIds;
        
        [DefaultValue(9999999)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "durability_value")]
        private float _durabilityValue;
        
        [DefaultValue(9999999)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "durability_max")]
        private float _durabilityMax;
        
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;
        
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop")]
        private bool _drop;
        
        [DefaultValue(5)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "spread")]
        private int _spread;
        
        protected override GiveItem Process(GiveItem action, string username, string from, Dictionary<string, object> parameters)
        {
            action._amount = StringToInt(_amount, 1, parameters).ToString();
            return base.Process(action, username, from, parameters);
        }
    }
}