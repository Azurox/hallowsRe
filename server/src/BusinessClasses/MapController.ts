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
    const map = this.worldMap.getMap(
      socket.player.mapPosition.x,
      socket.player.mapPosition.y
    );

    const moved = map.movePlayer(
      position.x,
      position.y,
      socket.player,
      socket.id
    );
  }

  async registerNewPosition(socket: GSocket, position: Position) {}

  async spawnPlayer(socket: GSocket) {
    console.log("retrieve player");
    await this.state.PlayerController.RetrievePlayer(socket);
    const map = this.worldMap.getMap(
      socket.player.mapPosition.x,
      socket.player.mapPosition.y
    );

    socket.to(map.name).emit("spawnPlayer", {
      position: socket.player.position,
      id: socket.player._id
    });

    socket.emit("loadMap", {
      mapName: map.name,
      position: socket.player.position
    });

    socket.emit("spawnMainPlayer", { position: socket.player.position });

    const playerPresentOnMap = map.getPlayers();
    for (let i = 0, len = playerPresentOnMap.length; i < len; i++) {
      socket.emit("spawnPlayer", {
        position: playerPresentOnMap[i].player.position,
        id: playerPresentOnMap[i].player._id
      });
    }

    const spawned = map.spawnPlayer(
      socket.player.position.x,
      socket.player.position.y,
      socket.player,
      socket.id
    );

    socket.join(map.name);
  }
}
