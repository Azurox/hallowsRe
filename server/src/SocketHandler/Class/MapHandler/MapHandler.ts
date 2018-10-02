import MapController from "../../../BusinessClasses/MapController";
import PlayerController from "../../../BusinessClasses/PlayerController";
import State from "../../../BusinessClasses/State";
import GSocket from "../../../BusinessClasses/GSocket";
import Position from "../../../BusinessClasses/RelationalObject/Position";

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
    console.log("init socket");
    this.socket.on("initWorld", this.spawnPlayer.bind(this));
    this.socket.on("loadMap", this.loadMap.bind(this));
    this.socket.on("initializeMovement", this.initializeMovement.bind(this));
    this.socket.on("newPosition", this.newPosition.bind(this));
    this.socket.on("disconnect", this.disconnect.bind(this));
  }

  async spawnPlayer() {
    await this.P.RetrievePlayer(this.socket);
    await this.M.spawnPlayer(this.socket);
  }

  async loadMap(mapName: string) {
    await this.M.spawnPlayer(this.socket);
  }

  async initializeMovement(positions: Position[]) {
    const positionsObject = Position.ToPositions(positions);
    const possible = await this.M.checkMovementsPossibility(this.socket, positionsObject);
    if (possible) {
      await this.M.playerIsMoving(this.socket, positionsObject);
      this.P.playerIsMoving(this.socket, positionsObject);
    } else {
      console.log("unauthorized movement");
    }
  }

  async newPosition(position: Position) {
    const positionsObject = Position.ToPosition(position);
    const possible = await this.M.checkMovementPossibility(this.socket, positionsObject);
    if (possible) {
      await this.M.movePlayer(this.socket, positionsObject);
      this.P.movePlayer(this.socket, positionsObject);
    } else {
      console.log("unauthorized movement");
    }
  }

  async disconnect() {
    await this.M.disconnectPlayer(this.socket);
  }
}
