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
    this.socket.on("move", this.move.bind(this));
    this.socket.on("disconnect", this.disconnect.bind(this));
  }

  async spawnPlayer() {
    await this.P.RetrievePlayer(this.socket);
    await this.M.spawnPlayer(this.socket);
  }

  async move(position: Position) {
    await this.M.movePlayer(this.socket, position);
  }

  async disconnect() {
    await this.M.disconnectPlayer(this.socket);
  }
}
