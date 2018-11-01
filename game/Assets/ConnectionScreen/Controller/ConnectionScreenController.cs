using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionScreenController : MonoBehaviour {
    public SocketManager socket;
    public Text title;
    public Button button;
    public Text buttonText;

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
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Register);
            switchState = 1;
        }
        else
        {
            title.text = "Connect";
            buttonText.text = "Connect";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Connect);
            switchState = 0;
        }
    }

    public void Connect()
    {
        Debug.Log("connect");
    }

    public void Register()
    {
        Debug.Log("register");
    }
}
