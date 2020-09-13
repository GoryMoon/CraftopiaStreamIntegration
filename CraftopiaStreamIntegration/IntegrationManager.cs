using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using BepInEx.Logging;

namespace CraftopiaStreamIntegration
{
    public class IntegrationManager
    {
        private readonly CSIPlugin _plugin;
        private ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();
        private CancellationTokenSource _source;
        private Task _task;
        private readonly ManualLogSource _logger;

        public IntegrationManager(CSIPlugin plugin)
        {
            _plugin = plugin;
            _logger = plugin.Log;
        }

        public void Start()
        {
            _source = new CancellationTokenSource();
            var token = _source.Token;
            _task = Task.Factory.StartNew(() =>
            {
                _logger.LogInfo("Starting Integration connection");
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        using (var client = new NamedPipeClientStream(".", "Craftopia", PipeDirection.In))
                        {
                            using (var reader = new StreamReader(client))
                            {
                                while (!token.IsCancellationRequested && !client.IsConnected)
                                {
                                    try
                                    {
                                        client.Connect(1000);
                                    }
                                    catch (TimeoutException)
                                    {
                                        // Ignore
                                    }
                                    catch (Win32Exception e)
                                    {
                                        Thread.Sleep(500);
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError($"Error in pipe connection: {e}");
                                        Thread.Sleep(500);
                                    }
                                }
                                _logger.LogDebug("Connected to Integration");
                                while (!token.IsCancellationRequested && client.IsConnected)
                                {
                                    if (reader.Peek() > 0)
                                    {
                                        var line = reader.ReadLine();
                                        if (line != null)
                                        {
                                            if (Utils.InGame)
                                            {
                                                Handle(line);
                                            }
                                            else
                                            {
                                                _messages.Enqueue(line);
                                            }
                                        }
                                    }
                                    reader.DiscardBufferedData();
                                    Thread.Sleep(50);
                                }
                                _logger.LogDebug("Disconnected to Integration");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error in socket connection: {e}");
                    }
                }
            }, token);
        }

        public void Close()
        {
            if (_source == null) return;
            _source.Cancel(true);
            _task?.Wait(1000);
            _task = null;
            _messages = null;
        }
        
        public void Update()
        {
            while (_messages.TryDequeue(out var line))
            {
                Handle(line);
            }
        }

        private void Handle(string line)
        {
            if (line.StartsWith("Action: "))
            {
                _logger.LogDebug(line);
                var action = line.Substring(8);
                _plugin.ActionManager.HandleAction(action);
            }
            else if (line.StartsWith("Message: "))
            {
                _logger.LogDebug(line);
                var message = line.Substring(9);
                _plugin.ActionManager.HandleMessage(message);
            }
        }
    }
}