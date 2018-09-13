import { Socket } from "socket.io";
import { IPlayer } from "../Schema/Player";

export default interface GSocket extends Socket {
  player: IPlayer;
}
