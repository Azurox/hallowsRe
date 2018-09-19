import Fight from "./FightRework";
import { ISpell } from "../../Schema/Spell";
import SpellImpact from "./SpellImpact";
import Position from "../RelationalObject/Position";
import Fighter from "./Fighter";

export default class SpellProcessor {
  fight: Fight;
  spell: ISpell;
  user: Fighter;
  target: Position;
  obstacles: Position[];
  constructor(fight: Fight, spell: ISpell, user: Fighter, target: Position, obstacles: Position[]) {
    this.fight = fight;
    this.spell = spell;
    this.user = user;
    this.target = target;
    this.obstacles = obstacles;
  }

  process(): SpellImpact[] {
    const impactedFighter: Fighter[] = [];
    const impactedStats: SpellImpact[] = [];

    if (!this.spell.selfUse && this.target.x == this.user.position.x && this.target.y == this.user.position.y)
      throw new Error("Cannot use this spell on self");
    if (this.user.currentActionPoint < this.spell.actionPointCost) throw new Error("Fighter doesn't have enough AP");
    const distance = new Position(this.target.x - this.user.position.x, this.target.y - this.user.position.y);
    if (this.spell.line && !(distance.x == 0 || distance.y == 0)) throw new Error("Spell not in line");
    if (Math.abs(distance.x) + Math.abs(distance.y) > this.spell.range) throw new Error("Spell outta range");
    if (!this.spell.ignoreObstacle && !this.isSightLineEmpty()) throw new Error("Line sight not empty");

    // Firt check who's hit by the spell
    for (let i = 0; i < this.fight.fightOrder.length; i++) {
      for (let j = 0; j < this.spell.hitArea.length; j++) {
        if (
          this.fight.fightOrder[i].position.x == this.spell.hitArea[j].x + this.target.x &&
          this.fight.fightOrder[i].position.y == this.spell.hitArea[j].y + this.target.y
        ) {
          impactedFighter.push(this.fight.fightOrder[i]);
          break;
        }
      }
    }

    /* Later i will need to calculate new position if the spell move fighter */

    for (let i = 0; i < impactedFighter.length; i++) {
      const impact = new SpellImpact(impactedFighter[i].getId());
      if (this.spell.physicalDamage) {
        impact.life = this.calculatePhysicalDamage(impactedFighter[i], this.spell.physicalDamage);
      }

      if (this.spell.magicDamage) {
        impact.life = this.calculateMagicalDamage(impactedFighter[i], this.spell.magicDamage);
      }

      if (impactedFighter[i].currentLife + impact.life <= 0) {
        impact.death = true;
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

  isSightLineEmpty(): boolean {
    const firstLine = this.bresenhamAlgorithm(this.target.x, this.target.y, this.user.position.x, this.user.position.y);
    const secondLine = this.bresenhamAlgorithm(
      this.user.position.x,
      this.user.position.y,
      this.target.x,
      this.target.y
    );

    const totalLine = firstLine.concat(secondLine);

    for (let i = 0; i < totalLine.length; i++) {
      for (let j = 0; j < this.obstacles.length; j++) {
        if (this.obstacles[j].x == this.user.position.x && this.obstacles[j].y == this.user.position.y) continue; // Si l'obstacle est le joueur on lui même ça ne sert a rien de checker
        if (this.obstacles[j].x == this.target.x && this.obstacles[j].y == this.target.y) continue; // Pareil si l'obstacle est la cible du sort (qui est donc obligatoirement un joueur)

        if (totalLine[i].x == this.obstacles[j].x && totalLine[i].y == this.obstacles[j].y) {
          console.log(
            `cell at X:${totalLine[i].x} Y:${totalLine[i].y} collide with obstacle X:${this.obstacles[j].x} Y:${
              this.obstacles[j].y
            }`
          );
          return false;
        }
      }
    }
    return true;
  }

  bresenhamAlgorithm(x0: number, y0: number, x1: number, y1: number): Position[] {
    const arr: Position[] = [];

    const distanceX = x1 - x0;
    const distanceY = y1 - y0;
    const distanceXAbs = Math.abs(distanceX);
    const distanceYAbs = Math.abs(distanceY);
    let eps = 0;
    const sx = distanceX > 0 ? 1 : -1;
    const sy = distanceY > 0 ? 1 : -1;
    if (distanceXAbs > distanceYAbs) {
      for (let x = x0, y = y0; sx < 0 ? x >= x1 : x <= x1; x += sx) {
        arr.push(new Position(x, y));
        eps += distanceYAbs;
        if (eps << 1 >= distanceXAbs) {
          y += sy;
          eps -= distanceXAbs;
        }
      }
    } else {
      for (let x: number = x0, y: number = y0; sy < 0 ? y >= y1 : y <= y1; y += sy) {
        arr.push(new Position(x, y));
        eps += distanceXAbs;
        if (eps << 1 >= distanceYAbs) {
          x += sx;
          eps -= distanceYAbs;
        }
      }
    }
    return arr;
  }
}
