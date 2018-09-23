using System.Collections.Generic;
using UnityEngine;

public class MonsterGroupDTG : MonoBehaviour
{
    public MonsterDTG MonsterDtg;

    private MapPathFinding mapPathFinding;
    private MonsterGroup monsterGroup;
    private List<MonsterDTG> monsters = new List<MonsterDTG>();

    public void Init(MapPathFinding mapPathFinding)
    {
        this.mapPathFinding = mapPathFinding;
    }

    public void SetMonsterGroup(MonsterGroup group)
    {
        monsterGroup = group;
        gameObject.name = group.id;
        foreach (var monster in monsterGroup.monsters)
        {
            var go = Instantiate(MonsterDtg, transform).GetComponent<MonsterDTG>();
            go.name = monster.monsterId;
            go.SetMonster(monster);
            go.SetBasePosition(group.position);
            monsters.Add(go);
        }
    }

    public void UpdateMonsterGroupPosition(Vector2 position, bool withoutMovement = false)
    {
        monsterGroup.position = position;
        if (monsters.Count == 1)
        {
            List<Vector2> path = null;
            if (mapPathFinding.IsReady())
            {
                path = mapPathFinding.FindPath(monsters[0].GetMonster().position, position);
            }
            monsters[0].AttributePosition(position, path, withoutMovement);
        }
        else
        {
            List<Vector2> path = null;
            List<Vector2> availableCell = null;
            if (mapPathFinding.IsReady())
            {
                availableCell = mapPathFinding.FindAvailableCellInRange(position, 2);
                path = mapPathFinding.FindPath(monsters[0].GetMonster().position, position);
            }
            monsters[0].AttributePosition(position, path, withoutMovement);

            for (int i = 1; i< monsters.Count; i++)
            {
                if (availableCell != null)
                {
                    path = mapPathFinding.FindPath(monsters[i].GetMonster().position, availableCell[Utils.Instance.Random.Next(availableCell.Count)]);
                }
                monsters[i].AttributePosition(position, path, withoutMovement);
            }

        }
    }

    public void ClickOnMonsterGroup()
    {
        transform.parent.GetComponent<MonsterGroupContainer>().ClickOnMonster(monsterGroup.position, monsterGroup.id);
    }
}
