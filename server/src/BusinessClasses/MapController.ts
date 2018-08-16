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

  async spawnPlayer(socket: GSocket) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);

    const spawned = await map.spawnPlayer(socket.player.position.x, socket.player.position.y, socket.player); // TODO:use it

    socket.to(map.name).emit("spawnPlayer", {
      position: socket.player.position,
      id: socket.player._id
    });

    socket.emit("loadMap", { mapName: map.name, position: socket.player.position });

    socket.emit("spawnMainPlayer", { position: socket.player.position });

    socket.join(map.name);
  }

  async checkMovementsPossibility(socket: GSocket, positions: Position[]) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);
    return map.checkPath(positions);
  }

  async checkMovementPossibility(socket: GSocket, position: Position) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);
    return map.checkPosition(socket.player.position, position);
  }

  async movePlayer(socket: GSocket, position: Position) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);
    map.movePlayer(socket.player, position);
  }

  async disconnectPlayer(socket: GSocket) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);

    await map.removePlayer(socket.player._id);
    socket.to(map.name).emit("disconnectPlayer", { id: socket.player._id });
  }
}
