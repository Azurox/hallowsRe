import MapController from "../../BusinessClasses/MapController";
import PlayerController from "../../BusinessClasses/PlayerController";
import State from "../../BusinessClasses/State";
import GSocket from "../../BusinessClasses/GSocket";

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

    async move() {
        await this.M.movePlayer();
    }

}