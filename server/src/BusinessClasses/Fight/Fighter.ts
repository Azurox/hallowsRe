import { IPlayer } from "../../Schema/Player";
import Position from "../RelationalObject/Position";

export default class Fighter {
  player: IPlayer;
  socketId: string;
  maxLife: number = 60;
  life: number = 60;
  position: Position = new Position(0, 0);

  constructor(player: IPlayer) {
    this.player = player;
    this.socketId = player.socketId;
  }
}
