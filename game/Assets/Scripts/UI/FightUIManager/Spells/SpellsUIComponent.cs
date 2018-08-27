using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsUIComponent : MonoBehaviour {
    public GameObject SpellMiniature;
    private FightUIManager FightUIManager;
    private List<SpellMiniatureUIComponent> Spells = new List<SpellMiniatureUIComponent>();

    private void Start()
    {
        FightUIManager = GetComponentInParent<FightUIManager>();
    }

    public void SetSpells(List<Spell> spells)
    {
        foreach (var go in Spells)
        {
            Destroy(go);
        }
        Spells.Clear();

        foreach (var spell in spells)
        {
            var go = Instantiate(SpellMiniature);
            go.transform.SetParent(transform, false);
            var min = go.GetComponent<SpellMiniatureUIComponent>();
            min.Init(FightUIManager);
            min.SetSpell(spell);
            Spells.Add(min);
        }
    }
}
