using System.ComponentModel;
using Newtonsoft.Json;

namespace CraftopiaActions
{
    public class InvertMouse: BaseAction<InvertMouse>
    {
        [DefaultValue(2)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "time")]
        private float _time;
    }
}