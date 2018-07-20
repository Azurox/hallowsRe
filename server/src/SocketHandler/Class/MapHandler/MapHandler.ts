import MapController from "../../../BusinessClasses/MapController";
import PlayerController from "../../../BusinessClasses/PlayerController";
import State from "../../../BusinessClasses/State";
import GSocket from "../../../BusinessClasses/GSocket";
import Position from "../../../BusinessClasses/RelationnalObject/Position";

export default class MapHandler {
    socket: GSocket;
    M: MapController;
    P: PlayerController;

    constructor(socket: GSocket, state: State) {
        this.socket = socket;
        this.M = state.MapController;
        this.P = state.PlayerController;
        this.initSocket();
    }

    initSocket() {
        this.socket.on("initWorld", this.spawnPlayer);
        this.socket.on("move", this.move);
    }

    async spawnPlayer() {
        console.log("spawn player");
        await this.M.spawnPlayer(this.socket);
    }

    async move(position: Position) {
        await this.M.movePlayer(this.socket, position);
    }

}