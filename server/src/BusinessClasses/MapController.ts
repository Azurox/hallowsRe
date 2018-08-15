import GSocket from "./GSocket";
import State from "./State";
import WorldMap from "./Map/WorldMap";
import Position from "./RelationalObject/Position";

export default class MapController {
  worldMap: WorldMap;
  state: State;

  constructor(state: State) {
    this.state = state;
    this.worldMap = new WorldMap("dist/data/worldMap.json");
  }

  async playerIsMoving(socket: GSocket, positions: Position[]) {
    const map = this.worldMap.getMap(
      socket.player.mapPosition.x,
      socket.player.mapPosition.y
    );

    if (map.checkPath(positions) === true) {
      console.log("movement is legal");
      socket.to(map.name).emit("movePlayer", positions);
    }
    console.log(positions);
  }

  async registerNewPosition(socket: GSocket, position: Position) {
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

  async spawnPlayer(socket: GSocket) {
    const map = this.worldMap.getMap(
      socket.player.mapPosition.x,
      socket.player.mapPosition.y
    );

    const [spawned, mappedId] = await map.spawnPlayer(
      socket.player.position.x,
      socket.player.position.y,
      socket.player
    );

    socket.player.mappedId = mappedId;

    socket.to(map.name).emit("spawnPlayer", {
      position: socket.player.position,
      id: mappedId
    });

    socket.emit("loadMap", {
      mapName: map.name,
      position: socket.player.position
    });

    socket.emit("spawnMainPlayer", { position: socket.player.position });

    socket.join(map.name);
  }

  async disconnectPlayer(socket: GSocket) {
    const map = this.worldMap.getMap(
      socket.player.mapPosition.x,
      socket.player.mapPosition.y
    );

    map.removePlayer(socket.id);
    socket
      .to(map.name)
      .emit("disconnectPlayer", { id: socket.player.mappedId });
  }
}
