using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioResponseUIComponent : MonoBehaviour {

    public Text responseText;
    private int index;

    public void SetResponse(int index, string response)
    {
        responseText.text = response;
        this.index = index;
    }

    public void ChooseResponse()
    {
        GetComponentInParent<ScenarioHolderUIComponent>().ChooseResponse(index);
    }
}
