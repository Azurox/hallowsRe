import State from "./State";
import { IPlayer } from "../Schema/Player";
import Scenario, { IScenario } from "../Schema/Scenario";
import GSocket from "./GSocket";
import Mongoose from "mongoose";
import Npc from "../Schema/Npc";

export default class QuestController {
  state: State;

  constructor(state: State) {
    this.state = state;
  }

  async getLeanScenario(socket: GSocket, npcId: string, scenarioId: string): Promise<IScenario> {
    const npc = await Npc.findById(npcId).lean();
    if (!npc) throw new Error("NPC Not found !");
    if (npc.mapPosition.x != socket.player.mapPosition.x || npc.mapPosition.y != socket.player.mapPosition.y)
      throw new Error("NPC not on player position !");
    let hasScenario = false;
    for (let i = 0; i < npc.scenarios.length; i++) {
      const response = npc.scenarios[i];
      if (response.equals(scenarioId)) {
        hasScenario = true;
      }
    }
    if (!hasScenario) throw new Error("Npc doesn't have this scenario !");
    const scenario = await Scenario.findById(scenarioId).lean();
    if (!scenario) throw new Error("Scenario not found !");
    return scenario;
  }

  async retrieveScenario(id: string): Promise<IScenario> {
    return await Scenario.findById(id);
  }

  async checkScenarioRequirement(quest: IScenario, player: IPlayer): Promise<boolean> {
    return true;
  }

  async applyScenario(socket: GSocket, quest: IScenario, response: number) {
    const questResponse = quest.responses[response];
    if (questResponse) {
      if (questResponse.startQuest) {
        this.addQuest(socket, questResponse.startQuest);
      }

      if (questResponse.completeQuest) {
        this.completeQuest(socket, questResponse.completeQuest);
      }
    }
  }

  async addQuest(socket: GSocket, questId: Mongoose.Types.ObjectId) {}

  async completeQuest(socket: GSocket, questId: Mongoose.Types.ObjectId) {}
}
