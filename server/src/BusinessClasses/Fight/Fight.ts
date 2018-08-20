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
    this.fightOrder = this.blueTeam.concat(this.redTeam);

    for (let i = 0; i < this.blueTeam.length; i++) {
      this.io.sockets.connected[this.blueTeam[i].socketId].leave(this.blueTeam[i].player.mapName);
      this.io.sockets.connected[this.blueTeam[i].socketId].join(this.id);
      const players = [];
      players.push({
        isMainPlayer: true,
        name: "mainPlayer",
        position: this.blueTeam[i].position,
        side: "blue"
      });

      for (let j = 0; j < this.blueTeam.length; j++) {
        if (j == i) {
          continue;
        }
        players.push({
          isMainPlayer: false,
          id: this.blueTeam[j].player._id,
          name: this.blueTeam[j].player.name,
          position: this.blueTeam[j].position,
          side: "blue"
        });
      }

      for (let j = 0; j < this.redTeam.length; j++) {
        players.push({
          isMainPlayer: false,
          id: this.redTeam[j].player._id,
          name: this.redTeam[j].player.name,
          position: this.redTeam[j].position,
          side: "red"
        });
      }

      this.io.to(this.blueTeam[i].socketId).emit("fightStarted", { players: players });
    }

    for (let i = 0; i < this.redTeam.length; i++) {
      this.io.sockets.connected[this.redTeam[i].socketId].leave(this.redTeam[i].player.mapName);
      this.io.sockets.connected[this.redTeam[i].socketId].join(this.id);

      const players = [];
      players.push({
        isMainPlayer: true,
        name: "mainPlayer",
        position: this.redTeam[i].position,
        side: "red"
      });

      for (let j = 0; j < this.redTeam.length; j++) {
        if (j == i) {
          continue;
        }
        players.push({
          isMainPlayer: false,
          id: this.redTeam[j].player._id,
          name: this.redTeam[j].player.name,
          position: this.redTeam[j].position,
          side: "red"
        });
      }

      for (let j = 0; j < this.blueTeam.length; j++) {
        players.push({
          isMainPlayer: false,
          id: this.blueTeam[j].player._id,
          name: this.blueTeam[j].player.name,
          position: this.blueTeam[j].position,
          side: "blue"
        });
      }
      this.io.to(this.redTeam[i].socketId).emit("fightStarted", { players: players });
    }

    this.started = true;
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
