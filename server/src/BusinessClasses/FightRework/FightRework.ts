import uuid from "uuid/v4";
import shuffle from "shuffle-array";
import { EventEmitter } from "events";
import Fighter from "./Fighter";
import { IMap } from "../../Schema/Map";
import Position from "../RelationalObject/Position";
import HumanFighter from "./HumanFighter";
export default class FightRework extends EventEmitter {
  /* CONST */
  PREPARATION_TIME: number = 90;
  TURN_TIME: number = 30;
  MAX_MEMBER_BY_SIDE: number = 5;

  id: string;
  io: SocketIO.Server;
  clock: number = 0;
  phase: number = 0;
  started: boolean = false;
  blueTeam: Fighter[];
  redTeam: Fighter[];
  fightOrder: Fighter[];
  map: IMap;
  mapObstacles: Position[];
  placementCells: { position: Position; taken: boolean; side: Side }[];

  constructor(io: SocketIO.Server, blueTeam: Fighter[], redTeam: Fighter[], map: IMap) {
    super();
    this.io = io;
    this.blueTeam = blueTeam;
    this.redTeam = redTeam;
    this.map = map;
    this.initFight();
  }

  initFight() {
    // Initialize fight ID.
    this.id = uuid();

    // Initialize placement cells
    this.placementCells = this.map.placementCells.map(cell => {
      return { position: cell.position, taken: false, side: cell.color };
    });

    this.mapObstacles = this.map.getObstacles(); // Get all obstacles of the current map.
    shuffle(this.placementCells); // Allow to have player placed in a random way.
    this.fightOrder = this.blueTeam.concat(this.redTeam).sort((a, b) => b.speed - a.speed); // contains all player sorted by speed.

    // Place fighter on the first available placement cell.
    for (let i = 0; i < this.fightOrder.length; i++) {
      for (let j = 0; j < this.placementCells.length; j++) {
        if (!this.placementCells[j].taken && this.placementCells[j].side == this.fightOrder[i].side) {
          this.fightOrder[i].position = this.placementCells[j].position;
          this.placementCells[j].taken = true;
          break;
        }
      }
    }
  }

  /* Send the fight to every human player */
  startFight() {
    const humans = this.getHumansFighter();
    for (const human of humans) {
      this.io.sockets.connected[human.getSocketId()].join(this.id);
      this.io.to(human.getSocketId()).emit("fightStarted", {
        id: this.id,
        placementCells: this.placementCells,
        fighters: this.fightOrder.map((fighter: Fighter) => {
          return {
            isMainPlayer: fighter.getId() == human.getId(),
            id: fighter.getId(),
            name: fighter.getName(),
            position: fighter.position,
            side: fighter.side,
            life: fighter.life,
            currentLife: fighter.currentLife,
            speed: fighter.speed,
            armor: fighter.armor,
            magicResistance: fighter.magicResistance,
            attackDamage: fighter.attackDamage,
            movementPoint: fighter.movementPoint,
            actionPoint: fighter.actionPoint,
            spells: fighter.getId() == human.getId() ? human.getSpells() : undefined
          };
        })
      });
    }
    this.started = true;
  }

  teleportPlayerPhase0(fighter: Fighter, position: Position) {
    if (this.phase != 0) return;
    for (const targetCell of this.placementCells) {
      if (targetCell.position.equals(position) && !targetCell.taken) {
        for (const oldCell of this.placementCells) {
          if (oldCell.position.equals(position)) {
            oldCell.taken = false;
          }
        }
        fighter.position = position;
        targetCell.taken = false;
        this.io.to(this.id).emit("teleportPreFight", { position: position, playerId: fighter.getId() });
      }
    }
  }

  setFighterReady(id: string) {
    const fighter = this.retrieveHumanFighterFromPlayerId(id);
    fighter.ready = true;
    if (this.checkEveryFighterReady()) {
      // this.startFightPhase1();
    } else {
      this.io.to(this.id).emit("setReady", { playerId: fighter.getId() });
    }
  }

  checkEveryFighterReady(): boolean {
    const humans = this.getHumansFighter();
    for (const human of humans) {
      if (!human.ready) return false;
    }
    return true;
  }

  getHumansFighter(): HumanFighter[] {
    const humans: HumanFighter[] = [];
    for (let i = 0; i < this.fightOrder.length; i++) {
      if (this.fightOrder[i].isRealPlayer) humans.push(<HumanFighter>this.fightOrder[i]);
    }
    return humans;
  }

  retrieveHumanFighterFromPlayerId(id: string): HumanFighter {
    const humans = this.getHumansFighter();
    for (const human of humans) {
      if (human.getSocketId() == id) return human;
    }
    throw Error("Fighter not found");
  }

  tick() {
    if (!this.started) {
      return;
    }
    this.clock++;
    if (this.phase == 0) {
      if (this.clock > this.PREPARATION_TIME) {
        // this.startFightPhase1();
      }
    } else {
      if (this.clock > this.TURN_TIME) {
        // this.nextTurn();
      }
    }
  }
}
