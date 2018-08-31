import { IPlayer } from "../../Schema/Player";
import Position from "../RelationalObject/Position";

export default class Fighter {
  player: IPlayer;
  socketId: string;
  side: Side;
  position: Position = new Position(0, 0);
  ready: boolean = false;

  /** Stats */
  readonly life: number;
  currentLife: number;
  readonly speed: number;
  currentSpeed: number;
  readonly armor: number;
  currentArmor: number;
  readonly magicResistance: number;
  currentMagicResistance: number;
  readonly attackDamage: number;
  currentAttackDamage: number;
  readonly movementPoint: number;
  currentMovementPoint: number;
  readonly actionPoint: number;
  currentActionPoint: number;
  dead: boolean = false;


  constructor(player: IPlayer, side: Side) {
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
}
