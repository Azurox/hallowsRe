import GSocket from "../../../BusinessClasses/GSocket";
import MapController from "../../../BusinessClasses/MapController";
import PlayerController from "../../../BusinessClasses/PlayerController";
import State from "../../../BusinessClasses/State";
import FightController from "../../../BusinessClasses/FightController";
import Position from "../../../BusinessClasses/RelationalObject/Position";

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
    this.socket.on("startFight", this.startFight.bind(this));
    this.socket.on("teleportPreFight", this.teleportPreFight.bind(this));
  }

  async startFight(target: { id: string }) {
    const firstTeam = [this.socket.player];
    const secondTeam = await this.P.RetrievePlayers([target.id]);
    const map = await this.M.getLeanMap(this.socket.player.mapPosition.x, this.socket.player.mapPosition.y);
    this.F.startFight(firstTeam, secondTeam, map);
  }

  async teleportPreFight(data: { x: number; y: number; fightId: string }) {
    const fight = this.F.retrieveFight(data.fightId);
    try {
      const fighter = fight.retrieveFighterFromPlayerId(this.socket.player.id);
      fight.teleportPlayerPhase0(fighter, new Position(data.x, data.y));
    } catch (error) {
      console.log(error);
    }
  }
}
