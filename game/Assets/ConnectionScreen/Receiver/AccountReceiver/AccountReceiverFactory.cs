using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountReceiverFactory {

    private AccountReceiverFactory() { }

    public static IReceiver Make(string eventName)
    {
        switch (eventName)
        {
            case AccountReceiverAlias.ACCOUNT_CREATED:
                return new AccountCreatedReceiver();
            case AccountReceiverAlias.EMAIL_ALREADY_TAKEN:
                return new EmailAlreadyTakenReceiver();
            default:
                return null;
        }
    }
}
