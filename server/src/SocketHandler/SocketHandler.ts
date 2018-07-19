// @flow
import MapHandler from "./Class/MapHandler";
import { Socket } from "../../node_modules/@types/socket.io";
import State from "../BusinessClasses/State";

export default class SocketHandler {
    socket: Socket;
    mapHandler: MapHandler;

    constructor(socket: Socket, state: State) {
        this.socket = socket;
        this.mapHandler = new MapHandler(socket, state);
    }
}