import GSocket from "./GSocket";
import Player from "./Player";
import State from "./State";

export default class PlayerController {
    state: State;

    constructor(state: State) {
        this.state = state;
    }

    async RetrievePlayer(socket: GSocket) {
        socket.player = new Player();
    }
}