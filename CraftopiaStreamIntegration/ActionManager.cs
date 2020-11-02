using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BepInEx.Logging;
using CraftopiaStreamIntegration.Actions;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Random = UnityEngine.Random;

namespace CraftopiaStreamIntegration
{
    public class ActionManager
    {
        private Dictionary<string, Type> _actions = new Dictionary<string, Type>();
        private ConcurrentQueue<BaseAction> _actionQueue = new ConcurrentQueue<BaseAction>();
        private readonly ManualLogSource _logger;
        
        public ActionManager(ManualLogSource logger)
        {
            _logger = logger;
            AddAction(typeof(MovePlayer));
            AddAction(typeof(InventoryBomb));
            AddAction(typeof(GiveItem));
            AddAction(typeof(SpawnMob));
            AddAction(typeof(ChangeMouseSensitivity));
            AddAction(typeof(InvertControls));
            AddAction(typeof(InvertMouse));
            AddAction(typeof(Ignite));
            AddAction(typeof(DropBomb));
            
            AddAction(typeof(HealPlayer));
            AddAction(typeof(RefillMana));
            AddAction(typeof(RefillStamina));
            AddAction(typeof(RefillHunger));
            AddAction(typeof(RepairTool));
        }

        ~ActionManager()
        {
            _actions.Clear();
            _actions = null;
            _actionQueue = null;
        }

        private void AddAction(Type action)
        {
            if (!typeof(BaseAction).IsAssignableFrom(action)) return;

            var type = action.Name.Underscore();
            _actions.Add(type, action);
            _logger.LogDebug($"Added action: {type}");
        }

        public void HandleAction(string rawAction)
        {
            _logger.LogDebug($"ActionManager: Handling action {rawAction}");
            try
            {
                var o = JsonConvert.DeserializeObject<JObject>(rawAction);
                var type = (string) o["type"];
                
                var actionType = _actions[type ?? "invalid"];
                if (actionType == null) return;
                
                var actionObj = o.ToObject(actionType);
                if (!(actionObj is BaseAction action)) return;
                
                if (action.DelayMin > 0 && action.DelayMax >= action.DelayMin)
                {
                    action.TryAfter = DateTime.Now + TimeSpan.FromMilliseconds(Random.Range(action.DelayMin, action.DelayMax));
                }
                _actionQueue.Enqueue(action);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error parsing action: {e}");
            }
        }

        public void HandleMessage(string message)
        {
            _logger.LogDebug($"ActionManager: Handling message {message}");
            _actionQueue.Enqueue(new MessageAction(message));
        }

        public void Update()
        {
            if (!_actionQueue.TryDequeue(out var action)) return;
            
            if (action.TryAfter.HasValue)
            {
                if (action.TryAfter.Value > DateTime.Now)
                {
                    _actionQueue.Enqueue(action);
                    return;
                }

                action.TryAfter = null;
            }
                
            var response = action.Handle();
            switch (response)
            {
                case ActionResponse.Retry:
                    _actionQueue.Enqueue(action);
                    break;
                case ActionResponse.Done:
                {
                    if (action.GetType() != typeof(MessageAction))
                    {
                        AccessUtils.PopMessage(7, "Integration", $"{action.From} ran action {action.GetType().Name.Humanize()}");
                    }
                    break;
                }
            }
        }
    }

    public enum ActionResponse
    {
        Done,
        Retry
    }
}