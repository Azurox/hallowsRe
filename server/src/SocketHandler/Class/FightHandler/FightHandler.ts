import GSocket from "../../../BusinessClasses/GSocket";
import MapController from "../../../BusinessClasses/MapController";
import PlayerController from "../../../BusinessClasses/PlayerController";
import State from "../../../BusinessClasses/State";
import FightController from "../../../BusinessClasses/FightController";

export default class FightHandler {
  socket: GSocket;
  M: MapController;
  P: PlayerController;
  F: FightController;

  constructor(socket: GSocket, state: State) {
    this.socket = socket;
    this.M = state.MapController;
    this.P = state.PlayerController;
    this.F = state.FightController;
    this.initSocket();
  }

  initSocket() {
    console.log("init fight socket");
    this.socket.on("startFight", this.startFight.bind(this));
  }

  async startFight(target: string) {
    const firstTeam = [this.socket.player];
    const secondTeam = await this.P.RetrievePlayers([target]);
    this.F.startFight(firstTeam, secondTeam);
  }
}
