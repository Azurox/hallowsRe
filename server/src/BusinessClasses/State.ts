import MapController from "./MapController";
import PlayerController from "./PlayerController";
import FightController from "./FightController";
import QuestController from "./QuestController";
import MonsterController from "./MonsterController";

export default class State {
  MapController: MapController;
  PlayerController: PlayerController;
  FightController: FightController;
  QuestController: QuestController;
  MonsterController: MonsterController;
  io: SocketIO.Server;

  constructor(io: SocketIO.Server) {
    this.io = io;
    this.MapController = new MapController(this);
    this.PlayerController = new PlayerController(this);
    this.FightController = new FightController(this);
    this.QuestController = new QuestController(this);
    this.MonsterController = new MonsterController(this);
  }
}
