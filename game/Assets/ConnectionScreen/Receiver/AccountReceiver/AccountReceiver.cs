using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountReceiver : MonoBehaviour {

    private SocketManager _socketManager;
	void Start () {
        _socketManager = FindObjectOfType<SocketManager>();
        _socketManager.On(AccountReceiverAlias.ACCOUNT_CREATED, AccountReceiverFactory.Make(AccountReceiverAlias.ACCOUNT_CREATED).Listen);
        _socketManager.On(AccountReceiverAlias.EMAIL_ALREADY_TAKEN, AccountReceiverFactory.Make(AccountReceiverAlias.EMAIL_ALREADY_TAKEN).Listen);
    }
}
