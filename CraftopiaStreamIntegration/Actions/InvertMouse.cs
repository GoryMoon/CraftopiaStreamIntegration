using System.Collections;
using Newtonsoft.Json;
using Oc;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class InvertMouse: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "time")]
        private float _time;

        public override ActionResponse Handle()
        {
            Utils.InvertMouse = true;
            OcPlMaster.Inst.StartCoroutine(Reset(_time));
            return ActionResponse.Done;
        }
        
        private static IEnumerator Reset(float time)
        {
            yield return new WaitForSeconds(time);
            Utils.InvertMouse = false;
        }
    }
}