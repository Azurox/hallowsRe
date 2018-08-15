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
    this.socket.on("loadMap", () => console.log("loadMap"));
    this.socket.on("newPath", this.newPath.bind(this));
    this.socket.on("newPosition", this.newPosition.bind(this));
    this.socket.on("disconnect", this.disconnect.bind(this));
  }

  async spawnPlayer() {
    await this.P.RetrievePlayer(this.socket);
    await this.M.spawnPlayer(this.socket);
  }

  async newPath(positions: Position[]) {
    console.log("New path");
    await this.M.playerIsMoving(this.socket, positions);
  }

  async newPosition(position: Position) {
    await this.M.registerNewPosition(this.socket, position);
  }

  async disconnect() {
    await this.M.disconnectPlayer(this.socket);
  }
}
