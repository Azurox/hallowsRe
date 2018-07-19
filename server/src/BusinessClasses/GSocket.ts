import { Socket } from "../../node_modules/@types/socket.io";
import Player from "./Player";

export default interface GSocket extends Socket {
    player: Player;
}