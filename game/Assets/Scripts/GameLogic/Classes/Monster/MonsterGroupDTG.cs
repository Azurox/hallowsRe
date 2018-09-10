using System.Collections.Generic;
using UnityEngine;

public class MonsterGroupDTG : MonoBehaviour
{
    public MonsterDTG MonsterDtg;
    private MapPathFinding mapPathFinding;
    private MonsterGroup monsterGroup;
    private List<MonsterDTG> monsters;

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
        }
    }

    public void UpdateMonsterGroupPosition(Vector2 position)
    {
        monsterGroup.position = position;
        if (monsters.Count == 1)
        {
            var path = mapPathFinding.FindPath(monsters[0].GetMonster().position, position);
            monsters[0].AttributePosition(position, path);
        }
        else
        {

            var availableCell = mapPathFinding.FindAvailableCellInRange(position, 2);

            foreach (var monster in monsters)
            {
                var path = mapPathFinding.FindPath(monsters[0].GetMonster().position, availableCell[Utils.Instance.Random.Next(availableCell.Count)]);
                monster.AttributePosition(position, path);
            }
        }
    }

    public void ClickOnMonsterGroup()
    {
        transform.parent.GetComponent<MonsterGroupContainer>().ClickOnMonster(monsterGroup.position);
    }
}
