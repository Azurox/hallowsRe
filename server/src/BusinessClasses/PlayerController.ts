import mongoose from "mongoose";
import GSocket from "./GSocket";
import State from "./State";
import Account, { IAccount } from "../Schema/Account";
import Player, { IPlayer } from "../Schema/Player";
import Position from "./RelationalObject/Position";

export default class PlayerController {
  state: State;
  pair = 0; // TODO: remove

  constructor(state: State) {
    this.state = state;
  }

  async RetrievePlayer(socket: GSocket) {
    const randomAccount = await Account.findOne().populate("players");
    socket.player = <IPlayer>randomAccount.players[this.pair % 2];
    socket.player.socketId = socket.id;
    await socket.player.populate("stats").execPopulate();
    this.pair++;
  }

  async RetrievePlayers(ids: string[]): Promise<IPlayer[]> {
    const players: IPlayer[] = [];
    for (let i = 0; i < ids.length; i++) {
      players.push(await Player.findById(mongoose.Types.ObjectId(ids[i])).populate("stats"));
    }
    return players;
  }

  async playerIsMoving(socket: GSocket, positions: Position[]): Promise<void> {
    socket.player.isMoving = true;
    socket.player.path = positions;
    await socket.player.save();
  }

  async movePlayer(socket: GSocket, position: Position): Promise<void> {
    if (socket.player.path[0].x == position.x && socket.player.path[0].y == position.y) {
      socket.player.path.shift();
      if (socket.player.path.length == 0) {
        socket.player.isMoving = false;
      }
      socket.player.position = position;
      await socket.player.save();
    }
  }
}
