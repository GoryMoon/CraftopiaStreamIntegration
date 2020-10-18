using System.Collections;
using Newtonsoft.Json;
using Oc;
using SR;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class InvertControls: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "time")]
        private float _time;

        public override ActionResponse Handle()
        {
            Utils.InvertControls = true;
            Utils.InvertControlsStack.Push(null);
            OcPlMaster.Inst.StartCoroutine(Reset(_time));
            return ActionResponse.Done;
        }
        
        private static IEnumerator Reset(float time)
        {
            yield return new WaitForSeconds(time);
            
            if (Utils.InvertControlsStack.Count <= 1) Utils.InvertControls = false;
            if (Utils.InvertControlsStack.Count > 0) Utils.InvertControlsStack.Pop();
        }
    }
}