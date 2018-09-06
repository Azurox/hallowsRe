import GSocket from "../../../BusinessClasses/GSocket";
import MapController from "../../../BusinessClasses/MapController";
import PlayerController from "../../../BusinessClasses/PlayerController";
import State from "../../../BusinessClasses/State";
import FightController from "../../../BusinessClasses/FightController";
import Position from "../../../BusinessClasses/RelationalObject/Position";
import Spell from "../../../Schema/Spell";
import QuestController from "../../../BusinessClasses/QuestController";

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

  async finishScenario(data: {scenarioId: string, responseIndex: number, questId: string}) {

    if (data.questId) {
        const quest = await this.Q.retrieveQuest(data.scenarioId);
        const canFinishQuest = await this.Q.checkQuestRequirement(quest, this.socket.player);
        if (canFinishQuest) {
            this.Q.applyQuest(quest, this.socket.player);
        }
    }
  }

}
