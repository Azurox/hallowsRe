using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroupDTG : MonoBehaviour
{
    public MonsterDTG MonsterDtg;
    private MonsterGroup monsterGroup;
    private List<MonsterDTG> monsters;

    public void SetMonsterGroup(MonsterGroup group)
    {
        monsterGroup = group;
        gameObject.name = group.id;
        foreach (var monster in monsterGroup.monsters)
        {
            var go = Instantiate(MonsterDtg, transform).GetComponent<MonsterDTG>();
            go.name = monster.monsterId;
            go.SetMonster(monster);
        }
    }

    public void UpdateMonsterGroupPosition(Vector2 position)
    {
        monsterGroup.position = position;
        if (monsters.Count == 1)
        {
            monsters[0].AttributePosition(position);
        }
        else
        {
            foreach (var monster in monsters)
            {
                monster.AttributeRandomPosition(position);
            }
        }
    }

    public void ClickOnMonsterGroup()
    {
        transform.parent.GetComponent<MonsterGroupContainer>().ClickOnMonster(monsterGroup.position);
    }
}
