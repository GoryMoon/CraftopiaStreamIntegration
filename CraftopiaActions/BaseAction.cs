using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using StreamIntegrationApp.API;

namespace CraftopiaActions
{
    public abstract class BaseAction<T>: IntegrationAction, ICloneable where T : BaseAction<T>
    {
        [JsonProperty(PropertyName = "from")]
        private string _from;

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "delay_min", DefaultValueHandling = DefaultValueHandling.Populate)]
        private int _delayMin;
        
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "delay_max", DefaultValueHandling = DefaultValueHandling.Populate)]
        private int _delayMax;

        public override string Execute(string username, string from, Dictionary<string, object> parameters)
        {
            return JsonConvert.SerializeObject(Process((T) Clone(), username, from, parameters));
        }

        protected virtual T Process(T action, string username, string from, Dictionary<string, object> parameters)
        {
            action._from = from;
            return action;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}