using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectionScreen.AccountReceiver
{
    public class ConnectRequest : IRequest
    {
        public string Email;
        public string Password;
    }
}