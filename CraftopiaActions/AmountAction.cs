
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace CraftopiaActions
{
    public abstract class AmountAction<T>: BaseAction<T> where T : AmountAction<T>
    {
        [DefaultValue("100")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;
        
        protected override T Process(T action, string username, string from, Dictionary<string, object> parameters)
        {
            action._amount = StringToFloat(_amount, float.MinValue, parameters).ToString(CultureInfo.InvariantCulture);
            return base.Process(action, username, from, parameters);
        }
    }
}