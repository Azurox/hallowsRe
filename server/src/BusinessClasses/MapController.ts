import GSocket from "./GSocket";
import State from "./State";
import WorldMap from "./Map/WorldMap";
import Position from "./RelationalObject/Position";
import Player from "../Schema/Player";

export default class MapController {
  worldMap: WorldMap;
  state: State;

  constructor(state: State) {
    this.state = state;
    this.worldMap = new WorldMap();
  }

  async movePlayer(socket: GSocket, position: Position) {
    const map = this.worldMap.getMap(
      socket.player.mapPosition.x,
      socket.player.mapPosition.y
    );

    const moved = map.movePlayer(position.x, position.y, socket.player, socket.id);
  }

  async registerNewPosition(socket: GSocket, position: Position) {}

  async spawnPlayer(socket: GSocket) {
    await this.state.PlayerController.RetrievePlayer(socket);
    const map = this.worldMap.getMap(
      socket.player.mapPosition.x,
      socket.player.mapPosition.y
    );

    map.spawnPlayer(socket.player.position.x, socket.player.position.y, socket.player, socket.id);

    socket.to(map.name).emit("spawnPlayer", { position: socket.player.position });
    socket.join(map.name);

    socket.emit("loadMap", {
      mapName: map.name,
      position: socket.player.position
    });
    socket.emit("spawnMainPlayer", { position: socket.player.position });
  }
}
