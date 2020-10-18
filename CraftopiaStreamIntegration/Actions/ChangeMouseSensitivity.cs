using System.Collections;
using Newtonsoft.Json;
using Oc;
using UnityEngine;

namespace CraftopiaStreamIntegration.Actions
{
    public class ChangeMouseSensitivity: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private float _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "time")]
        private float _time;

        public override ActionResponse Handle()
        {
            var player = OcPlMaster.Inst;
            Utils.ChangeSensitivityStack.Push(SROptions.Current.Option_MouseSensitivity);
            SROptions.Current.Option_MouseSensitivity = _amount;
            player.StartCoroutine(ChangeMouseBack(_time));
            return ActionResponse.Done;
        }
        
        private static IEnumerator ChangeMouseBack(float time)
        {
            yield return new WaitForSeconds(time);
            if (Utils.ChangeSensitivityStack.Count >= 0)
                SROptions.Current.Option_MouseSensitivity = Utils.ChangeSensitivityStack.Pop();
            else
                SROptions.Current.Option_MouseSensitivity = 1;
        }
    }
}