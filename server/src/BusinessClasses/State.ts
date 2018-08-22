import MapController from "./MapController";
import PlayerController from "./PlayerController";
import FightController from "./FightController";

export default class State {
  MapController: MapController;
  PlayerController: PlayerController;
  FightController: FightController;
  io: SocketIO.Server;

  constructor(io: SocketIO.Server) {
    this.io = io;
    this.MapController = new MapController(this);
    this.PlayerController = new PlayerController(this);
    this.FightController = new FightController(this);
  }
}
