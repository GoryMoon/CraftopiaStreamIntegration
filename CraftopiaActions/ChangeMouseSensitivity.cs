using System.ComponentModel;
using Newtonsoft.Json;

namespace CraftopiaActions
{
    public class ChangeMouseSensitivity: BaseAction<ChangeMouseSensitivity>
    {
        [DefaultValue(2)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;
        
        [DefaultValue(2)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "time")]
        private float _time;
    }
}