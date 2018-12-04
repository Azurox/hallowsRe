using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

public class SocketManager : MonoBehaviour
{

    private WebSocket _socket;
    private readonly Dictionary<string, List<Action<string>>> _registeredActions = new Dictionary<string, List<Action<string>>>();
    private readonly Dictionary<string, Dictionary<string, Action<string>>> _registeredAwaitedResponse = new Dictionary<string, Dictionary<string, Action<string>>>();
    private readonly  Queue<string> _registeredQueries = new Queue<string>();
    private readonly  ConcurrentStack<KeyValuePair<Action<string>, string>> _callBackStack = new ConcurrentStack<KeyValuePair<Action<string>, string>>();
    private const float MAX_PING_TIMEOUT = 25f;
    private float _pingTimer = 0;
    

    // Use this for initialization
    private IEnumerator Start()
    {

        using (_socket = new WebSocket("ws://localhost:54614/ws"))
        {
            _socket.OnMessage += (sender, e) => { ProcessMessage(e.Data); };
            _socket.Connect();

            while (true)
            {

                if (_pingTimer > MAX_PING_TIMEOUT)
                {
                    _pingTimer = 0;
                    _socket.Send("2");
                }

                if (_registeredQueries.Count != 0)
                {
                    foreach (var query in _registeredQueries.ToList())
                    {
                        _socket.Send(query);
                    }
                    _registeredQueries.Clear();
                }


                yield return null;

            }
        }
    }

    [UsedImplicitly]
    private void Update()
    {
        _pingTimer += Time.deltaTime;
        KeyValuePair<Action<string>, string> item;
        if (_callBackStack.TryPop(out item))
        {
            item.Key.Invoke(item.Value);
        }
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      _socket.Send("41");
    }

    private void ProcessMessage(string message)
    {
        if (message.StartsWith("42"))
        {
            var eventAndData = message.Substring(3);
            eventAndData = eventAndData.Remove(eventAndData.Length - 1);
            var eventName = eventAndData.Split(',')[0].Trim('"');
            var data = eventAndData.Remove(0, eventName.Length + 3);
            // Debug.Log(eventName);
            // Debug.Log(data);

            if (_registeredActions.ContainsKey(eventName))
            {
                foreach (var action in _registeredActions[eventName])
                {
                    _callBackStack.Push(new KeyValuePair<Action<string>, string>(action, data));
                }
            } else
            {
                string actionFound = null;
                foreach (KeyValuePair<string, Dictionary<string, Action<string>>> response in _registeredAwaitedResponse)
                {
                    foreach (KeyValuePair<string, Action<string>> action in response.Value)
                    {
                        Debug.Log(action.Key);
                        if (action.Key == eventName)
                        {
                            Debug.Log($"found {eventName}  == {action.Key}");
                            actionFound = response.Key;
                            _callBackStack.Push(new KeyValuePair<Action<string>, string>(action.Value, data));
                            goto leaveMultiLoop; // cleanest way to exist nested loop :(
                        }
                    }
                }
                leaveMultiLoop:


                if(actionFound != null)
                {
                    _registeredAwaitedResponse.Remove(actionFound);
                } else
                {
                    Debug.Log("Event named " + eventName + " isn't registered !");
                }
            }
        }
        else if(message != "3" && message != "40")
        {
            Debug.Log("Unknown message received : " + message);
        }
    }


    public void On(string eventName, Action<string> callback)
    {
        // if (_registeredActions.ContainsKey(eventName)) throw new Exception("Cannot register event with the same name");
        if (!_registeredActions.ContainsKey(eventName))
        {
            _registeredActions[eventName] = new List<Action<string>>();
        }
        _registeredActions[eventName].Add(callback);
    }

    public void Emit(string eventName, string data = "")
    {
        var query = data.IsNullOrEmpty() ? string.Format("42[\"{0}\"]", eventName) : string.Format("42[\"{0}\", {1}]", eventName, data);
        //Debug.Log(query);
        _registeredQueries.Enqueue(query);
    }

    public string Emit(string eventName, IRequest request)
    {
        var data = JsonConvert.SerializeObject(request);
        var query = data.IsNullOrEmpty() ? string.Format("42[\"{0}\"]", eventName) : string.Format("42[\"{0}\", {1}]", eventName, data);
        //Debug.Log(query);
        _registeredQueries.Enqueue(query);
        return Guid.NewGuid().ToString();
    }

    public void AwaitOneResponse(string guid, string eventName, Action<string> callback)
    {
        if (!_registeredAwaitedResponse.ContainsKey(guid))
        {
            _registeredAwaitedResponse[guid] = new Dictionary<string, Action<string>>();
        } else if (_registeredAwaitedResponse[guid].ContainsKey(eventName))
        {
            throw new Exception("Already waiting a message of this type : " + eventName);
        }
        _registeredAwaitedResponse[guid][eventName] = callback;
    }
}
