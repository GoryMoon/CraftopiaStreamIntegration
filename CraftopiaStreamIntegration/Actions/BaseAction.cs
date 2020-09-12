using System;
using Newtonsoft.Json;

namespace CraftopiaStreamIntegration.Actions
{
    public abstract class BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "from")]
        public string From;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "delay_min")]
        public int DelayMin;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "delay_max")]
        public int DelayMax;
        
        public DateTime? TryAfter = null;
        
        protected void TryLater(TimeSpan time)
        {
            TryAfter = DateTime.Now + time;
        }
        
        public abstract ActionResponse Handle();
    }
}