﻿using HarmonyLib;
using Oc;
using Oc.Em;

namespace CraftopiaStreamIntegration
{
    public static class AccessUtils
    {
        private static FastInvokeHandler _sendChatMessage;
        private static FastInvokeHandler _getCustomNameField;
        private static FastInvokeHandler _setCustomNameField;
        private static AccessTools.FieldRef<OcCharacter, OcObjPoolCtrl> _poolCtrl;
        private static AccessTools.FieldRef<OcEm, OcEnemyHeader> _enemyHeader;

        public static void Init()
        {
            _sendChatMessage = MethodInvoker.GetHandler(AccessTools.Method("OcUI_ChatHandler:PopMessage", new []{ typeof(int), typeof(string), typeof(string) }));
            _getCustomNameField = MethodInvoker.GetHandler(AccessTools.Method(typeof(OcEm), "get_CustomName"));
            _setCustomNameField = MethodInvoker.GetHandler(AccessTools.Method(typeof(OcEm), "set_CustomName", new []{ typeof(string) }));

            _poolCtrl = AccessTools.FieldRefAccess<OcCharacter, OcObjPoolCtrl>("_PoolCtrl");
            _enemyHeader = AccessTools.FieldRefAccess<OcEm, OcEnemyHeader>("_EmHeader");
        }
        
        public static void PopMessage(int netId, string speakerName, string message)
        {
            _sendChatMessage.Invoke(SingletonMonoBehaviour<OcUI_ChatHandler>.Inst, netId, speakerName, message);
        }

        public static void SetCustomName(this OcEm data, string name)
        {
            _setCustomNameField.Invoke(data, name);
            _enemyHeader.Invoke(data).restart();
        }
        
        public static string GetCustomName(this OcEm data)
        {
            return (string) _getCustomNameField.Invoke(data);
        }

        public static OcObjPoolCtrl GetPoolCtrl(OcCharacter ctx)
        {
            return _poolCtrl.Invoke(ctx);
        }
    }
}