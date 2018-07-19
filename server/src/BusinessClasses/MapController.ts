import GSocket from "./GSocket";
import State from "./State";

export default class MapController {
    worldMap: { name: string; }[][];
    state: State;

    constructor(state: State) {
        this.state = state;
        this.worldMap = [
            [{name: "0-0.json"}, {name: "0-0.json"}],
            [{name: "0-0.json"}, {name: "0-0.json"}]
        ];
    }

    async movePlayer() {

    }

    async spawnPlayer(socket: GSocket) {
        await this.state.PlayerController.RetrievePlayer(socket);
        const mapName = this.worldMap[socket.player.mapPosition.x][socket.player.mapPosition.y].name;
        socket.to(mapName).emit("spawnPlayer");
        socket.join(mapName);
        socket.emit("loadMap", {
            mapName: mapName,
            position: socket.player.position
        });
    }
}