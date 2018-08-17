import State from "../BusinessClasses/State";
import GSocket from "../BusinessClasses/GSocket";
import MapHandler from "./Class/MapHandler/MapHandler";
import FightHandler from "./Class/FightHandler/FightHandler";

export default class SocketHandler {
  socket: GSocket;
  mapHandler: MapHandler;
  fightHandler: FightHandler;

  constructor(socket: GSocket, state: State) {
    this.socket = socket;
    this.mapHandler = new MapHandler(socket, state);
    this.fightHandler = new FightHandler(socket, state);
  }
}
