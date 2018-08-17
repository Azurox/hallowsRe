import { IPlayer } from "../../Schema/Player";

export default class Fighter {
  player: IPlayer;
  socketId: string;
  maxLife: number = 60;
  life: number = 60;
  position: Position;

  constructor(player: IPlayer) {
    this.player = player;
    this.socketId = player.socketId;
  }
}
