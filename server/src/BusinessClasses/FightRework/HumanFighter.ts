import { IPlayer } from "../../Schema/Player";
import Fighter from "./Fighter";
import mongoose from "mongoose";
import CheckinHistory from "./CheckinHistory";

export default class HumanFighter extends Fighter {
  player: IPlayer;
  socketId: string;
  disconnected = false;
  checkinHistory = new CheckinHistory();

  constructor(player: IPlayer, side: Side) {
    super();
    this.player = player;
    this.socketId = player.socketId;
    this.side = side;

    this.life = player.stats.life;
    this.currentLife = player.stats.life;
    this.speed = player.stats.speed;
    this.currentSpeed = player.stats.speed;
    this.armor = player.stats.armor;
    this.currentArmor = player.stats.armor;
    this.magicResistance = player.stats.magicResistance;
    this.currentMagicResistance = player.stats.magicResistance;
    this.attackDamage = player.stats.attackDamage;
    this.currentAttackDamage = player.stats.attackDamage;
    this.movementPoint = player.stats.movementPoint;
    this.currentMovementPoint = player.stats.movementPoint;
    this.actionPoint = player.stats.actionPoint;
    this.currentActionPoint = player.stats.actionPoint;
  }

  getId(): string {
    return this.player.id;
  }

  isRealPlayer(): boolean {
    return true;
  }
  getName(): string {
    return this.player.name;
  }
  getSocketId(): string {
    return this.socketId;
  }

  getSpells(): mongoose.Types.ObjectId[] {
    return this.player.spells;
  }
}
