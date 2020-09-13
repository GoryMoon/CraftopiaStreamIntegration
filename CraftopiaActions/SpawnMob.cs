using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace CraftopiaActions
{
    public class SpawnMob: BaseAction<SpawnMob>
    {
        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private string _id;
        
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "level")]
        private string _level;
        
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;
        
        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "radius")]
        private int _radius;
        
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "scale_min")]
        private string _scaleMin;
        
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "scale_max")]
        private string _scaleMax;
        
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "despawn_time")]
        private float _despawnTime;
        
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "add_player_level")]
        private bool _addPlayerLevel;
        
        protected override SpawnMob Process(SpawnMob action, string username, string from, Dictionary<string, object> parameters)
        {
            action._level = StringToInt(_level, 0, parameters).ToString();
            action._amount = StringToInt(_amount, 1, parameters).ToString();
            action._scaleMin = StringToFloat(_scaleMin, 0.1f, parameters).ToString(CultureInfo.InvariantCulture);
            action._scaleMax = StringToFloat(_scaleMax, 0.1f, parameters).ToString(CultureInfo.InvariantCulture);
            return base.Process(action, username, from, parameters);
        }
    }
}