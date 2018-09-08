import FightController from "../../../BusinessClasses/FightController";
import GSocket from "../../../BusinessClasses/GSocket";
import MapController from "../../../BusinessClasses/MapController";
import PlayerController from "../../../BusinessClasses/PlayerController";
import QuestController from "../../../BusinessClasses/QuestController";
import State from "../../../BusinessClasses/State";

export default class QuestHandler {
  socket: GSocket;
  M: MapController;
  P: PlayerController;
  F: FightController;
  Q: QuestController;

  constructor(socket: GSocket, state: State) {
    this.socket = socket;
    this.M = state.MapController;
    this.P = state.PlayerController;
    this.F = state.FightController;
    this.Q = state.QuestController;
    this.initSocket();
  }

  initSocket() {
    this.socket.on("finishScenario", this.finishScenario.bind(this));
  }

  async finishScenario(data: { scenarioId: string; responseIndex: number; npcId: string }) {
    try {
      const leanScenario = await this.Q.getLeanScenario(this.socket, data.npcId, data.scenarioId);
      const canFinishScenario = await this.Q.checkScenarioRequirement(leanScenario, this.socket.player);
      if (canFinishScenario) {
        this.Q.applyScenario(this.socket, leanScenario, data.responseIndex);
      }
    } catch (error) {
      console.log(error);
    }
  }
}
