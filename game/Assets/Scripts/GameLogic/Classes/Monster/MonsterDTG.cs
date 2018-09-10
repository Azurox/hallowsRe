using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterDTG : MonoBehaviour, IPointerClickHandler {

    private Monster monster;

    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }

    public Monster GetMonster()
    {
        return monster;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.GetComponent<MonsterGroupDTG>().ClickOnMonsterGroup();
    }

    internal void AttributePosition(Vector2 position, List<Vector2> path)
    {
        monster.position = position;
        GetComponent<Movable>().TakePath(new Vector2(transform.position.x, transform.position.z), path);
    }
}
