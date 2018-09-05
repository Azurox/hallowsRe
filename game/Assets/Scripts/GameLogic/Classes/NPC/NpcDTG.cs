﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcDTG : MonoBehaviour {
    private Npc npc;
    private Scenario scenario;
    private Sprite sprite;
    private Texture2D npcImage;


    private void OnMouseDown()
    {
        transform.parent.GetComponent<NpcHandler>().ShowScenario(GetMostSuitedScenario(), this) ;
    }

    private Scenario GetMostSuitedScenario()
    {
        return npc.scenarios.ElementAt(0).Value;   
    }

    public void SetNpc(Npc npc)
    {
        this.npc = npc;
    }

    public void InitNpc()
    {
        foreach (var id in npc.scenariosId)
        {
            npc.scenarios[id] = ResourcesLoader.Instance.GetScenario(id);
        }
        sprite = ResourcesLoader.Instance.GetImage(npc.imageId);
        transform.position = new Vector3(npc.position.x, transform.position.y, npc.position.y);
        name = npc.name;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public Texture2D GetBackupImage()
    {
        if(npcImage == null)
        {
            npcImage = Utils.Instance.GetPhotoTaker().TakePicture(gameObject);
        }
        return npcImage;
    }

    public Npc GetNpc()
    {
        return npc;
    }
}
