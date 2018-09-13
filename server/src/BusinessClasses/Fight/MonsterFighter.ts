import Fighter from "./Fighter";
import { IMonster } from "../../Schema/Monster";
import { ISpell } from "../../Schema/Spell";
import mongoose from "mongoose";

export default class MonsterFighter extends Fighter {
  player: IMonster;

  constructor(monster: IMonster, side: Side) {
    super();

    this.player = monster;
    this.side = side;
    this.life = monster.stats.life;
    this.currentLife = monster.stats.life;
    this.speed = monster.stats.speed;
    this.currentSpeed = monster.stats.speed;
    this.armor = monster.stats.armor;
    this.currentArmor = monster.stats.armor;
    this.magicResistance = monster.stats.magicResistance;
    this.currentMagicResistance = monster.stats.magicResistance;
    this.attackDamage = monster.stats.attackDamage;
    this.currentAttackDamage = monster.stats.attackDamage;
    this.movementPoint = monster.stats.movementPoint;
    this.currentMovementPoint = monster.stats.movementPoint;
    this.actionPoint = monster.stats.actionPoint;
    this.currentActionPoint = monster.stats.actionPoint;
  }

    getId(): string {
        return this.player.id;
    }

    isRealPlayer(): boolean {
        return false;
      }

    getName(): string {
        return this.player.name;
    }

    /**
     * Warning about this. It should never be used other than for comparing with a real Fighter Id.
     */
    getSocketId(): string {
        return "noSocketId";
    }

    getSpells(): mongoose.Types.ObjectId[] {
        return this.player.spells;
    }
}
