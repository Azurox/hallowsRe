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

    public void AttributeRandomPosition(Vector2 position)
    {

    }

    public void AttributePosition(Vector2 position)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.GetComponent<MonsterGroupDTG>().ClickOnMonsterGroup();
    }
}
