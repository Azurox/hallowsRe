import State from "../BusinessClasses/State";
import GSocket from "../BusinessClasses/GSocket";
import MapHandler from "./Class/MapHandler/MapHandler";
import FightHandler from "./Class/FightHandler/FightHandler";
import QuestHandler from "./Class/QuestHandler/QuestHandler";

export default class SocketHandler {
  socket: GSocket;
  mapHandler: MapHandler;
  fightHandler: FightHandler;

  questHandler: QuestHandler;

  constructor(socket: GSocket, state: State) {
    this.socket = socket;
    this.mapHandler = new MapHandler(socket, state);
    this.fightHandler = new FightHandler(socket, state);
    this.questHandler = new QuestHandler(socket, state);
  }
}
