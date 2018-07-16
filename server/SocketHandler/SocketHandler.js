// @flow
import MapHandler from "./Class/MapHandler";

export default class SocketHandler {
    socket: any;
    mapHandler: MapHandler;

    constructor(socket: any, state: any){
        this.socket = socket;
        this.mapHandler = new MapHandler(socket, state);
    }
};