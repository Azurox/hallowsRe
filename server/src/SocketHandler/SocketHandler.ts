// @flow
import MapHandler from "./Class/MapHandler";
import State from "../BusinessClasses/State";
import GSocket from "../BusinessClasses/GSocket";

export default class SocketHandler {
    socket: GSocket;
    mapHandler: MapHandler;

    constructor(socket: GSocket, state: State) {
        this.socket = socket;
        this.mapHandler = new MapHandler(socket, state);
    }
}