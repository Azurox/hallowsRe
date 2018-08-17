import uuid from "uuid/v4";
import Fighter from "./Fighter";

type Side = "red" | "blue";

export default class Fight {
  io: SocketIO.Server;
  id: string;
  started: boolean = false;
  redTeam: Fighter[] = [];
  blueTeam: Fighter[] = [];
  fightOrder: Fighter[] = [];
  clock: number = 0;

  constructor(io: SocketIO.Server) {
    this.io = io;
    this.id = uuid();
  }

  addFighter(fighter: Fighter, side: Side) {
    if (side === "blue") {
      this.blueTeam.push(fighter);
    } else {
      this.redTeam.push(fighter);
    }
  }

  startFight() {
    this.started = true;
    this.fightOrder = this.blueTeam.concat(this.redTeam);
    for (let i = 0; i < this.redTeam.length; i++) {
      this.io.sockets.connected[this.redTeam[i].socketId].join(this.id);
      this.io.to(this.redTeam[i].socketId).emit("fightStarted");
    }

    for (let i = 0; i < this.redTeam.length; i++) {
      this.io.sockets.connected[this.redTeam[i].socketId].join(this.id);
      this.io.to(this.redTeam[i].socketId).emit("fightStarted");
    }

    console.log("send start to both teams");
  }

  tick() {
    if (!this.started) {
      return;
    }

    this.clock++;
    console.log("Fight ticked to : " + this.clock);
  }
}
