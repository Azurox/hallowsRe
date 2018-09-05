using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioHolderUIComponent : MonoBehaviour {
    public Image NpcImage;
    public RawImage NpcRawImage;
    public RawImage PlayerImage;
    public Text Discussion;
    public GameObject NpcDiscussions;
    public GameObject PlayersResponses;
    public ScenarioResponseUIComponent ScenarioResponseUIComponent;
    private List<ScenarioResponseUIComponent> responses = new List<ScenarioResponseUIComponent>();
    private Scenario scenario;
    private int indexDiscussion = 0;

    public void InitScenario(Scenario scenario, NpcDTG npc)
    {
        this.scenario = scenario;
        ClearResponse();
        NpcDiscussions.SetActive(true);
        PlayersResponses.SetActive(false);
        indexDiscussion = 0;
        Discussion.text = scenario.discussions[0];

        PlayerImage.texture = PlayerInformation.Instance.GetPlayerImage();
        PlayerImage.color = new Color(1, 1, 1, 0.3f);

        var npcSprite = npc.GetSprite();
        if(npcSprite == null)
        {
            var npcTexture = npc.GetBackupImage();
            NpcRawImage.texture = npcTexture;
            NpcRawImage.gameObject.SetActive(true);
            NpcImage.gameObject.SetActive(false);
        }
        else
        {
            NpcImage.sprite = npcSprite;
            NpcImage.gameObject.SetActive(true);
            NpcRawImage.gameObject.SetActive(false);
        }

        NpcImage.color = new Color(1, 1, 1);
        NpcRawImage.color = new Color(1, 1, 1);

    }

    public void NextDiscussion()
    {
        if(indexDiscussion + 1 >= scenario.discussions.Length)
        {
            Debug.Log("end of discussion");
            if(scenario.responses.Length == 0)
            {
                CloseDiscussion();
            }
            else
            {
                ShowResponse();
            }
        }
        else
        {
            indexDiscussion++;
            Discussion.text = scenario.discussions[indexDiscussion];
        }
    }

    public void ShowResponse()
    {
        NpcImage.color = new Color(1, 1, 1, 0.3f);
        NpcRawImage.color = new Color(1, 1, 1, 0.3f);
        PlayerImage.color = new Color(1,1,1);
        for (int i = 0; i < scenario.responses.Length; i++)
        {
            var go = Instantiate(ScenarioResponseUIComponent, PlayersResponses.transform);
            var res = go.GetComponent<ScenarioResponseUIComponent>();
            res.SetResponse(i, scenario.responses[i].response);
            responses.Add(res);
        }
        NpcDiscussions.SetActive(false);
        PlayersResponses.SetActive(true);
    }

    public void ChooseResponse(int index)
    {
        if(index < scenario.responses.Length)
        {
            if (scenario.responses[index].end)
            {
                CloseDiscussion();
            }
        }
    }

    public void CloseDiscussion()
    {
        transform.parent.GetComponent<WorldUIManager>().HideScenario();
    }


    private void ClearResponse()
    {
        foreach (var response in responses)
        {
            Destroy(response.gameObject);
        }
        responses.Clear();
    }



}
