import uuid from "uuid/v4";
import Fighter from "./Fighter";
import { IMap } from "../../Schema/Map";
import Position from "../RelationalObject/Position";
import { IPlayer } from "../../Schema/Player";

export default class Fight {
  /* CONST */
  PREPARATION_TIME: number = 90;
  TURN_TIME: number = 90;
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

  constructor(io: SocketIO.Server, map: IMap) {
    this.io = io;
    this.id = uuid();
    this.blueCells = map.blueCells.map(cell => {
      return { position: cell, taken: false };
    });
    this.redCells = map.redCells.map(cell => {
      return { position: cell, taken: false };
    });
  }

  addFighter(fighter: Fighter) {
    if (fighter.side === "blue") {
      this.blueTeam.push(fighter);
    } else {
      this.redTeam.push(fighter);
    }
  }

  orderFighter() {
    // TODO: Order by speed
    this.fightOrder = this.blueTeam.concat(this.redTeam);
    for (let i = 0; i < this.fightOrder.length; i++) {
      this.fightOrder[i].order = i;
    }
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
            order: fighter.order,
            life: fighter.life,
            maxLife: fighter.maxLife
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
        this.phase = 1;
        this.clock = 0;
        console.log("Entered in phase 1");
      }
    } else {
      // Check if the clock is > of max turn time;
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
    if (this.phase != 0 ) return;

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
}
