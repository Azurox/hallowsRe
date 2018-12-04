using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionScreenController : MonoBehaviour {
    public SocketManager socket;
    public Text title;
    public Button button;
    public Text buttonText;
    public Text buttonSwitchText;
    public InputField emailInput;
    public InputField passwordInput;

    private int switchState = 0;

    private void Start()
    {
        button.onClick.AddListener(Connect);
    }


    public void SwitchRegisterConnect()
    {
        if(switchState == 0)
        {
            title.text = "Register";
            buttonText.text = "Register";
            buttonSwitchText.text = "Switch to connect";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Register);
            switchState = 1;
        }
        else
        {
            title.text = "Connect";
            buttonText.text = "Connect";
            buttonSwitchText.text = "Switch to register";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Connect);
            switchState = 0;
        }
    }

    public void Connect()
    {
        Debug.Log("connect");
        // socket.Emit("")
    }

    public void Register()
    {
        Debug.Log("register");
        var guid = socket.Emit(RegisterRequestAlias.REGISTER, new RegisterRequest()
        {
            Email = emailInput.text,
            Password = passwordInput.text
        });
        socket.AwaitOneResponse(guid, AccountReceiverAlias.ACCOUNT_CREATED, AccountCreated);
        socket.AwaitOneResponse(guid, AccountReceiverAlias.EMAIL_ALREADY_TAKEN, EmailAlreadyTaken);
    }

    private void AccountCreated(string _)
    {
        Debug.Log("Account created");
    }

    private void EmailAlreadyTaken(string _)
    {
        Debug.Log("Email already taken");
    }
}
