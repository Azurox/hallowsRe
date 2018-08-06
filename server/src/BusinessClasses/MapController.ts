import GSocket from "./GSocket";
import State from "./State";
import WorldMap from "./Map/WorldMap";
import Position from "./RelationalObject/Position";

export default class MapController {
    worldMap: WorldMap;
    state: State;

    constructor(state: State) {
        this.state = state;
        this.worldMap = new WorldMap();
    }

    async movePlayer(socket: GSocket, position: Position) {

    }

    async registerNewPosition(socket: GSocket, position: Position) {

    }

    async spawnPlayer(socket: GSocket) {
        await this.state.PlayerController.RetrievePlayer(socket);
        const mapName = this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y).name;
        socket.to(mapName).emit("spawnPlayer");
        socket.join(mapName);
        console.log("emit new map");
        socket.emit("loadMap", {
            mapName: mapName,
            position: socket.player.position
        });
    }
}