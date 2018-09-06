import State from "./State";
import Fight from "./Fight/Fight";
import { IPlayer } from "../Schema/Player";
import Fighter from "./Fight/Fighter";
import { IMap } from "../Schema/Map";
import Quest, { IQuest } from "../Schema/Quest";
import Scenario, { IScenario } from "../Schema/Scenario";

export default class QuestController {
  state: State;

  constructor(state: State) {
    this.state = state;
  }

  async retrieveQuest(id: string): Promise<IQuest> {
    return await Scenario.findById(id);
  }

  async checkQuestRequirement(quest: IQuest, player: IPlayer): Promise<boolean> {

    return true;
  }

  async applyQuest(quest: IQuest, player: IPlayer) {

  }

}
