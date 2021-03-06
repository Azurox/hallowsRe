import uuid from "uuid/v4";
import Fighter from "./Fighter";
import { IMap } from "../../Schema/Map";
import Position from "../RelationalObject/Position";
import { IPlayer } from "../../Schema/Player";
import { ISpell } from "../../Schema/Spell";
import SpellProcessor from "./SpellProcessor";
import SpellImpact from "./SpellImpact";
import FightEndProcessor from "./FightEndProcessor";

export default class Fight {
  /* CONST */
  PREPARATION_TIME: number = 90;
  TURN_TIME: number = 30;
  MAX_MEMBER_BY_SIDE: number = 4;

  io: SocketIO.Server;
  id: string;
  started: boolean = false;
  redTeam: Fighter[] = [];
  blueTeam: Fighter[] = [];
  fightOrder: Fighter[] = [];
  phase: number = 0;
  clock: number = 0;
  blueCells: { position: Position; taken: boolean }[];
  redCells: { position: Position; taken: boolean }[];
  obstacles: Position[];
  acceptedId: string;
  winners: Side;
  isFinished: Boolean = false;

  constructor(io: SocketIO.Server, map: IMap) {
    this.io = io;
    this.id = uuid();
    this.blueCells = map.blueCells.map(cell => {
      return { position: cell, taken: false };
    });
    this.redCells = map.redCells.map(cell => {
      return { position: cell, taken: false };
    });

    this.obstacles = map.getObstacles();
  }

  addFighter(fighter: Fighter) {
    if (fighter.side === "blue") {
      this.blueTeam.push(fighter);
    } else {
      this.redTeam.push(fighter);
    }
  }

  orderFighter() {
    this.fightOrder = this.blueTeam.concat(this.redTeam);
    this.fightOrder.sort((a, b) => b.speed - a.speed);
  }

  placeFighter() {
    for (let i = 0; i < this.fightOrder.length; i++) {
      if (this.fightOrder[i].side === "blue") {
        for (let j = 0; j < this.blueCells.length; j++) {
          if (this.blueCells[j].taken == false) {
            this.fightOrder[i].position = this.blueCells[j].position;
            this.blueCells[j].taken = true;
            break;
          }
        }
      } else {
        for (let j = 0; j < this.redCells.length; j++) {
          if (this.redCells[j].taken == false) {
            this.fightOrder[i].position = this.redCells[j].position;
            this.redCells[j].taken = true;
            break;
          }
        }
      }
    }
  }

  startFight() {
    this.orderFighter();
    this.placeFighter();

    for (let i = 0; i < this.fightOrder.length; i++) {
      // Leave the current map and join the fight
      this.io.sockets.connected[this.fightOrder[i].socketId].leave(this.fightOrder[i].player.mapName);
      this.io.sockets.connected[this.fightOrder[i].socketId].join(this.id);

      // return every player
      this.io.to(this.fightOrder[i].socketId).emit("fightStarted", {
        players: this.fightOrder.map((fighter: Fighter) => {
          return {
            isMainPlayer: fighter.socketId == this.fightOrder[i].socketId,
            id: fighter.player.id,
            name: fighter.player.name,
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
            spells: fighter.socketId == this.fightOrder[i].socketId ? this.fightOrder[i].player.spells : undefined
          };
        }),
        id: this.id,
        blueCells: this.blueCells,
        redCells: this.redCells
      });
    }

    this.started = true;
    console.log("send start to both teams");
  }

  tick() {
    if (!this.started) {
      return;
    }

    this.clock++;

    if (this.phase == 0) {
      if (this.clock > this.PREPARATION_TIME) {
        this.startFightPhase1();
      }
    } else {
      if (this.clock > this.TURN_TIME) {
        this.nextTurn();
      }
    }
  }

  retrieveFighterFromPlayerId(id: string): Fighter {
    for (let i = 0; i < this.fightOrder.length; i++) {
      if (this.fightOrder[i].player.id === id) {
        return this.fightOrder[i];
      }
    }
    throw Error("Fighter not found");
  }

  teleportPlayerPhase0(fighter: Fighter, position: Position) {
    if (this.phase != 0) return;

    if (fighter.side == "blue") {
      for (let i = 0; i < this.blueCells.length; i++) {
        if (
          this.blueCells[i].position.x === position.x &&
          this.blueCells[i].position.y === position.y &&
          this.blueCells[i].taken === false
        ) {
          for (let j = 0; j < this.blueCells.length; j++) {
            if (
              this.blueCells[j].position.x === fighter.position.x &&
              this.blueCells[j].position.y === fighter.position.y
            ) {
              this.blueCells[j].taken = false;
            }
          }
          fighter.position = position;
          this.blueCells[i].taken = true;
          this.io.to(this.id).emit("teleportPreFight", { position: position, playerId: fighter.player.id });
        }
      }
    } else {
      for (let i = 0; i < this.redCells.length; i++) {
        if (
          this.redCells[i].position.x === position.x &&
          this.redCells[i].position.y === position.y &&
          this.redCells[i].taken === false
        ) {
          for (let j = 0; j < this.redCells.length; j++) {
            if (
              this.redCells[j].position.x === fighter.position.x &&
              this.redCells[j].position.y === fighter.position.y
            ) {
              this.redCells[j].taken = false;
            }
          }
          fighter.position = position;
          this.redCells[i].taken = true;
          this.io.to(this.id).emit("teleportPreFight", { position: position, playerId: fighter.player.id });
        }
      }
    }
  }

  setFighterReady(id: string) {
    const fighter = this.retrieveFighterFromPlayerId(id);
    fighter.ready = true;
    if (this.checkEveryFighterReady()) {
      this.startFightPhase1();
    } else {
      this.io.to(this.id).emit("setReady", { playerId: fighter.player.id });
    }
  }

  checkEveryFighterReady(): boolean {
    for (let i = 0; i < this.fightOrder.length; i++) {
      if (!this.fightOrder[i].ready) {
        return false;
      }
    }
    return true;
  }

  startFightPhase1() {
    console.log("Entered in phase 1");
    this.clock = 0;
    this.phase = 1;

    for (let i = 0; i < this.fightOrder.length; i++) {
      this.fightOrder[i].ready = true;
    }

    this.acceptedId = this.fightOrder[0].player.id;
    this.io.to(this.id).emit("fightPhase1", { playerId: this.acceptedId });
  }

  checkPlayerTurn(id: string): boolean {
    if (this.acceptedId == id) {
      return true;
    } else {
      return false;
    }
  }

  nextTurn() {
    this.clock = 0;
    this.softResetCurrentPlayerStats();
    for (let i = 0; i < this.fightOrder.length; i++) {
      if (this.fightOrder[i].player.id == this.acceptedId) {
        if (i + 1 < this.fightOrder.length) {
          this.acceptedId = this.fightOrder[i + 1].player.id;
        } else {
          this.acceptedId = this.fightOrder[0].player.id;
        }
        break;
      }
    }
    this.io.to(this.id).emit("nextTurn", { playerId: this.acceptedId });
  }

  softResetCurrentPlayerStats() {
    const fighter = this.retrieveFighterFromPlayerId(this.acceptedId);
    fighter.currentActionPoint = fighter.actionPoint;
    fighter.currentMovementPoint = fighter.movementPoint;
  }

  moveFighter(id: string, path: Position[]) {
    if (!this.checkPlayerTurn(id)) return;
    const fighter = this.retrieveFighterFromPlayerId(id);
    let canMove = true;
    for (let i = 0; i < this.fightOrder.length; i++) {
      for (let j = 0; j < path.length; j++) {
        if (
          this.fightOrder[i].player.id != id &&
          this.fightOrder[i].position.x == path[j].x &&
          this.fightOrder[i].position.y == path[j].y
        ) {
          canMove = false;
        }
      }
    }

    if (path.length > fighter.currentMovementPoint) {
      canMove = false;
    }

    for (let i = 0; i < path.length; i++) {
      const appliedMove = this.applyCell(fighter, path[i]);
      if (appliedMove) {
        fighter.currentMovementPoint--;
        fighter.position = path[i];
      }
    }

    if (canMove) {
      this.io.to(this.id).emit("fighterMove", { playerId: id, path: path });
    }
  }

  /** Apply cell effect, return false if player cannot continue his path
   * will be usefull later if trap remove Movement point or if invisible item block the pah
   */
  applyCell(fighter: Fighter, position: Position): boolean {
    return true;
  }

  getFightersPosition(): Position[] {
    const positions = [];
    for (let i = 0; i < this.fightOrder.length; i++) {
      positions.push(this.fightOrder[i].position);
    }
    return positions;
  }

  async useSpell(id: string, spell: ISpell, position: Position) {
    if (!this.checkPlayerTurn(id)) return;
    const fighter = this.retrieveFighterFromPlayerId(id);
    const processor = new SpellProcessor(
      this,
      spell,
      fighter,
      position,
      this.getFightersPosition().concat(this.obstacles)
    );
    try {
      const impacts = processor.process();
      fighter.currentActionPoint -= spell.actionPointCost;
      const fightIsFinished = this.applySpellImpacts(impacts);

      if (!fightIsFinished) {
        this.io.to(this.id).emit("fighterUseSpell", {
          playerId: id,
          position: position,
          spellId: spell.id,
          impacts: impacts
        });
      } else {
        const fighters = this.blueTeam.concat(this.redTeam);
        for (let i = 0; i < fighters.length; i++) {
          const fightEndProcessor = new FightEndProcessor();
          console.log(fighters[i].side == this.winners);
          const fightResult = fightEndProcessor.process(fighters[i].side == this.winners);
          this.io.sockets.connected[fighters[i].socketId].leave(this.id);
          await fighters[i].player.leaveFight();

          this.io.to(fighters[i].socketId).emit("fighterUseSpell", {
            playerId: id,
            position: position,
            spellId: spell.id,
            impacts: impacts,
            fightEnd: fightResult
          });
        }
        this.isFinished = true;
      }
    } catch (error) {
      console.log(error);
    }
  }

  applySpellImpacts(impacts: SpellImpact[]): boolean {
    let fightIsFinished = false;
    for (let i = 0; i < impacts.length; i++) {
      const fighter = this.retrieveFighterFromPlayerId(impacts[i].playerId);
      fighter.currentLife += impacts[i].life;
      /* Need to apply other effect as Buff or Debuff */
      if (impacts[i].death) {
        const finish = this.killFighter(fighter);
        fightIsFinished = fightIsFinished === false ? finish : true;
      }
    }
    return fightIsFinished;
  }

  killFighter(fighter: Fighter): boolean {
    fighter.dead = true;
    for (let i = this.fightOrder.length - 1; i >= 0; --i) {
      if (this.fightOrder[i].player.id == fighter.player.id) {
        this.fightOrder.splice(i, 1);
        break;
      }
    }

    let fightIsFinished = false;
    let isAPlayerAlive = false;

    if (fighter.side == "blue") {
      for (let i = 0; i < this.blueTeam.length; i++) {
        if (!this.blueTeam[i].dead) {
          isAPlayerAlive = true;
          break;
        }
      }

      if (!isAPlayerAlive) {
        console.log(`Red team won`);
        this.winners = "red";
        fightIsFinished = true;
      }
    } else {
      for (let i = 0; i < this.redTeam.length; i++) {
        if (!this.redTeam[i].dead) {
          isAPlayerAlive = true;
          break;
        }
      }

      if (!isAPlayerAlive) {
        console.log(`Blue team won`);
        this.winners = "blue";
        fightIsFinished = true;
      }
    }

    if (this.acceptedId == fighter.player.id) {
      this.nextTurn();
    }
    console.log(`${fighter.player.name} is dead !`);
    return fightIsFinished;
  }
}
