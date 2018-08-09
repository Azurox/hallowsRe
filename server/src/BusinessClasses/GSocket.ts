import { Socket } from "../../node_modules/@types/socket.io";
import { IPlayer } from "../Schema/Player";

export default interface GSocket extends Socket {
  player: IPlayer;
}
