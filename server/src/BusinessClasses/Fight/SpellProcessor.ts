import Fight from "./Fight";
import { ISpell } from "../../Schema/Spell";
import Fighter from "./Fighter";
import SpellImpact from "./SpellImpact";
import Position from "../RelationalObject/Position";

export default class SpellProcessor {
    fight: Fight;
    spell: ISpell;
    user: Fighter;
    target: Position;
    constructor(fight: Fight, spell: ISpell, user: Fighter, target: Position) {
        this.fight = fight;
        this.spell = spell;
        this.user = user;
        this.target = target;
    }

    process(): SpellImpact[] {

        // Firt check who's hit by the spell
        const impactedFighter: Fighter[] = [];
        for (let i = 0; i < this.fight.fightOrder.length; i++) {
            for (let j = 0; j < this.spell.hitArea.length; j++) {
                if (this.fight.fightOrder[i].position.x == this.spell.hitArea[j].x && this.fight.fightOrder[i].position.y == this.spell.hitArea[j].y) {
                    impactedFighter.push(this.fight.fightOrder[i]);
                    break;
                }
            }
        }

        /* Later i will need to calculate new position if the spell move fighter */

        const impactedStats: SpellImpact[] = [];
        for (let i = 0 ; i < impactedFighter.length; i++) {
            const impact = new SpellImpact(impactedFighter[i].player.id);
            if (this.spell.physicalDamage) {
                impact.life = this.calculatePhysicalDamage(impactedFighter[i], this.spell.physicalDamage);
            }

            if (this.spell.magicDamage) {
                impact.life = this.calculateMagicalDamage(impactedFighter[i], this.spell.magicDamage);
            }

            impactedStats.push(impact);
        }

        return impactedStats;
        // Then create an object with impacted stats
    }

    calculatePhysicalDamage(fighter: Fighter, physicalDamage: number): number {
        return -physicalDamage;
    }

    calculateMagicalDamage(fighter: Fighter, magicalDamage: number): number {
        return -magicalDamage;
    }
}