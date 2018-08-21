import { IPlayer } from "../../Schema/Player";
import Position from "../RelationalObject/Position";

export default class Fighter {
  player: IPlayer;
  socketId: string;
  side: Side;
  maxLife: number = 60;
  life: number = 60;
  position: Position = new Position(0, 0);
  order: number = 0;
  ready: boolean = false;

  constructor(player: IPlayer, side: Side) {
    this.player = player;
    this.socketId = player.socketId;
    this.side = side;
  }
}
