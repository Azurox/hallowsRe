using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter {
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

    public Fighter(JSONObject data)
    {
        Name = data["name"] != null ? data["name"].str : null;
        Id = data["id"] != null ? data["id"].str : null;
        IsMainPlayer = data["isMainPlayer"] != null ? data["isMainPlayer"].b : false;
        Side = data["side"] != null ? (Side)System.Enum.Parse(typeof(Side), data["side"].str) : Side.blue;
        Position = new Vector2(data["position"]["x"].n, data["position"]["y"].n);
        Ready = false;

        spells = new List<Spell>();
        var spellsData = data["spells"];
        if(spellsData != null)
        {
            foreach (var spell in spellsData.list)
            {
                spells.Add(ResourcesLoader.Instance.GetSpell(spell.str));
            }
        }

        #region stats
        Life = data["life"] != null ? (int)data["life"].n : 0;
        CurrentLife = data["currentLife"] != null ? (int)data["currentLife"].n : 0;
        Speed = data["speed"] != null ? (int)data["speed"].n : 0;
        CurrentSpeed = Speed;
        Armor = data["armor"] != null ? (int)data["armor"].n : 0;
        CurrentArmor = Armor;
        MagicResistance = data["magicResistance"] != null ? (int)data["magicResistance"].n : 0;
        CurrentMagicResistance = MagicResistance;
        AttackDamage = data["attackDamage"] != null ? (int)data["attackDamage"].n : 0;
        CurrentAttackDamage = AttackDamage;
        MovementPoint = data["movementPoint"] != null ? (int)data["movementPoint"].n : 0;
        CurrentMovementPoint = MovementPoint;
        ActionPoint = data["actionPoint"] != null ? (int)data["actionPoint"].n : 0;
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
        if(StatsChange != null)
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
            if(Death != null)
            {
                Death(Id);
            }
        }
    }
}
