using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace CraftopiaActions
{
    public class DropBomb: BaseAction<DropBomb>
    {
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;
        
        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "radius")]
        private int _radius;

        [DefaultValue("10")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage")]
        private string _damage;
        
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage_structure")]
        private float _damageStructure;
        
        [DefaultValue(4)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "speed")]
        private float _speed;
        
        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "height")]
        private float _height;
        
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop_delay")]
        private float _dropDelay;
        
        protected override DropBomb Process(DropBomb action, string username, string from, Dictionary<string, object> parameters)
        {
            action._damage = StringToInt(_damage, 0, parameters).ToString();
            action._amount = StringToInt(_amount, 1, parameters).ToString();
            return base.Process(action, username, from, parameters);
        }
    }
}