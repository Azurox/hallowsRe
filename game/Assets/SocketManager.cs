using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using WebSocketSharp;

public class SocketManager : MonoBehaviour
{

    private WebSocket _socket;
    private readonly Dictionary<string, Action<string>> _registeredActions = new Dictionary<string, Action<string>>();
    private readonly  Queue<string> _registeredQueries = new Queue<string>();
    private readonly  Queue<KeyValuePair<Action<string>, string>> _callBackQueue = new Queue<KeyValuePair<Action<string>, string>>();
    private const float MAX_PING_TIMEOUT = 25f;
    private float _pingTimer = 0;
    

    // Use this for initialization
    private IEnumerator Start()
    {

        using (_socket = new WebSocket("ws://127.0.0.1:3000/socket.io/?EIO=4&transport=websocket"))
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
                    foreach (var query in _registeredQueries)
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
        if (_callBackQueue.Count == 0) return;
        foreach (var callBackData in _callBackQueue)
        {
            callBackData.Key.Invoke(callBackData.Value);
        }

        _callBackQueue.Clear();
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
                _callBackQueue.Enqueue(new KeyValuePair<Action<string>, string>(_registeredActions[eventName], data));
            }
            else
            {
                Debug.Log("Event named " + eventName + " isn't registered !");
            }
        }
        else
        {
            Debug.Log("Unknown message received : " + message);
        }
    }


    public void On(string eventName, Action<string> callback)
    {
        if (_registeredActions.ContainsKey(eventName)) throw new Exception("Cannot register event with the same name");
        _registeredActions[eventName] = callback;
    }

    public void Emit(string eventName, string data = "")
    {
        var query = data.IsNullOrEmpty() ? string.Format("42[\"{0}\"]", eventName) : string.Format("42[\"{0}\", {1}]", eventName, data);
        Debug.Log(query);
        _registeredQueries.Enqueue(query);
    }


}
