﻿using System.Collections;
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
            var sensitivity = SROptions.Current.Option_MouseSensitivity;
            SROptions.Current.Option_MouseSensitivity = _amount;
            player.StartCoroutine(ChangeMouseBack(_time, sensitivity));
            return ActionResponse.Done;
        }
        
        private static IEnumerator ChangeMouseBack(float time, float sensitivity)
        {
            yield return new WaitForSeconds(time);
            SROptions.Current.Option_MouseSensitivity = sensitivity;
        }
    }
}