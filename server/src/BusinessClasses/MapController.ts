import GSocket from "./GSocket";
import State from "./State";
import WorldMap from "./Map/WorldMap";
import Position from "./RelationalObject/Position";
import Player, { IPlayer } from "../Schema/Player";
import { IMap } from "../Schema/Map";

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

    const players = map.getPlayers();
    for (let i = 0; i < players.length; i++) {
      if (!players[i].equals(socket.player._id)) {
        const player: IPlayer = await Player.findById(players[i]).lean();
        socket.emit("spawnPlayer", {
          position: player.position,
          id: player._id,
          path: player.isMoving ? player.path : undefined
        });
      }
    }

    this.state.MonsterController.SendMapMonsters(socket, map);

    socket.player.mapName = map.name;
    socket.join(map.name);
    socket.player.save();
  }

  async checkMovementsPossibility(socket: GSocket, positions: Position[]) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);
    return map.checkPath(positions);
  }

  async checkMovementPossibility(socket: GSocket, position: Position) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);
    return map.checkPosition(socket.player.position, position);
  }

  async playerIsMoving(socket: GSocket, positions: Position[]) {
    const map = await this.worldMap.getMap(socket.player.mapPosition.x, socket.player.mapPosition.y);

    socket.broadcast.to(map.name).emit("playerMove", {
      id: socket.player._id,
      path: positions
    });
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

  async getLeanMap(x: number, y: number): Promise<IMap> {
    const map = await this.worldMap.getLeanMap(x, y);
    return map;
  }

  async getMap(x: number, y: number): Promise<IMap> {
    const map = await this.worldMap.getMap(x, y);
    return map;
  }

  async checkIfMapHasMonsterGroup(x: number, y: number, groupId: string): Promise<boolean> {
    const map = await this.worldMap.getLeanMap(x, y);
    let found = false;
    for (let i = 0; i < map.monsterGroups.length; i++) {
      const element = map.monsterGroups[i];
      if (element.equals(groupId)) { found = true; }
    }

    return found;
  }

  async removeMonsterGroupFromMap(x: number, y: number, groupId: string) {
    const map = await this.worldMap.getMap(x, y);
    for (let i = 0; i < map.monsterGroups.length; i++) {
      const element = map.monsterGroups[i];
      if (element.equals(groupId)) {
        map.monsterGroups.splice(i, 1);
        await map.save();
        return;
       }
    }
  }
}
