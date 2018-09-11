using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter
{
    public string Name { get; set; }
    public string Id { get; set; }
    public Vector2 Position { get; set; }
    public bool IsMainPlayer { get; set; }
    public Side Side { get; set; }
    public bool Ready { get; set; }
    private List<Spell> spells;

    public delegate void OnStatsChange();
    public OnStatsChange StatsChange;

    public delegate void OnDeath(string id);
    public OnDeath Death;

    #region stats
    private int Life;
    private int CurrentLife;
    private int Speed;
    private int CurrentSpeed;
    private int Armor;
    private int CurrentArmor;
    private int MagicResistance;
    private int CurrentMagicResistance;
    private int AttackDamage;
    private int CurrentAttackDamage;
    private int MovementPoint;
    private int CurrentMovementPoint;
    private int ActionPoint;
    private int CurrentActionPoint;
    private bool dead = false;
    #endregion

    public Fighter(FighterReponse fighterData)
    {
        Id = fighterData.id;
        Name = fighterData.name;
        IsMainPlayer = fighterData.isMainPlayer;
        Side = (Side)System.Enum.Parse(typeof(Side), fighterData.side);
        Position = fighterData.position;
        Ready = false;

        spells = new List<Spell>();
        if (fighterData.spells != null)
        {
            foreach (var spell in fighterData.spells)
            {
                spells.Add(ResourcesLoader.Instance.GetSpell(spell));
            }
        }

        #region stats
        Life = fighterData.life;
        CurrentLife = fighterData.currentLife;
        Speed = fighterData.speed;
        CurrentSpeed = Speed;
        Armor = fighterData.armor;
        CurrentArmor = Armor;
        MagicResistance = fighterData.magicResistance;
        CurrentMagicResistance = MagicResistance;
        AttackDamage = fighterData.attackDamage;
        CurrentAttackDamage = AttackDamage;
        MovementPoint = fighterData.movementPoint;
        CurrentMovementPoint = MovementPoint;
        ActionPoint = fighterData.actionPoint;
        CurrentActionPoint = ActionPoint;
        #endregion
    }

    public List<Spell> GetSpells()
    {
        if (!IsMainPlayer) Debug.LogError("!!!!! Cannot get spell of a basic fighter !!!!");
        return spells;
    }

    public void ResetTurnStats()
    {
        CurrentActionPoint = ActionPoint;
        CurrentMovementPoint = MovementPoint;
        if (StatsChange != null)
        {
            StatsChange();
        }
    }

    public void TakeImpact(Impact impact)
    {
        UpdateCurrentLife(impact.life);
        UpdateDeathStatus(impact.death);
    }

    public int GetCurrentLife()
    {
        return CurrentLife;
    }

    public int GetLife()
    {
        return Life;
    }

    public void UpdateCurrentLife(int num)
    {
        CurrentLife += num;
        if (StatsChange != null)
        {
            StatsChange();
        }
    }

    public int GetMovementPoint()
    {
        return MovementPoint;
    }

    public int GetCurrentMovementPoint()
    {
        return CurrentMovementPoint;
    }

    public void UpdateCurrentMovementPoint(int num)
    {
        CurrentMovementPoint += num;
        if (StatsChange != null)
        {
            StatsChange();
        }
    }

    public int GetActionPoint()
    {
        return ActionPoint;
    }

    public int GetCurrentActionPoint()
    {
        return CurrentActionPoint;
    }

    public void UpdateCurrentActionPoint(int num)
    {
        CurrentActionPoint += num;
        if (StatsChange != null)
        {
            StatsChange();
        }
    }

    public int GetCurrentSpeed()
    {
        return CurrentSpeed;
    }

    public int GetSpeed()
    {
        return Speed;
    }

    private void UpdateDeathStatus(bool isDead)
    {
        if (isDead)
        {
            dead = true;
            if (Death != null)
            {
                Death(Id);
            }
        }
    }
}
