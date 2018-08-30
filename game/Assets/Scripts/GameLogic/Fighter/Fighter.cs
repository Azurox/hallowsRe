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

    #region stats
    public int Life { get; set; }
    public int CurrentLife { get; set; }
    public int Speed { get; set; }
    public int CurrentSpeed { get; set; }
    public int Armor { get; set; }
    public int CurrentArmor { get; set; }
    public int MagicResistance { get; set; }
    public int CurrentMagicResistance { get; set; }
    public int AttackDamage { get; set; }
    public int CurrentAttackDamage { get; set; }
    public int MovementPoint { get; set; }
    public int CurrentMovementPoint { get; set; }
    public int ActionPoint { get; set; }
    public int CurrentActionPoint { get; set; }
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
        this.CurrentActionPoint = ActionPoint;
        this.CurrentMovementPoint = MovementPoint;
    }

    public void TakeImpact(Impact impact)
    {
        CurrentLife += impact.life;
    }
}
