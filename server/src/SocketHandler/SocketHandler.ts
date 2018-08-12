import State from "../BusinessClasses/State";
import GSocket from "../BusinessClasses/GSocket";
import MapHandler from "./Class/MapHandler/MapHandler";

export default class SocketHandler {
  socket: GSocket;
  mapHandler: MapHandler;

  constructor(socket: GSocket, state: State) {
    this.socket = socket;
    this.mapHandler = new MapHandler(socket, state);
  }
}
