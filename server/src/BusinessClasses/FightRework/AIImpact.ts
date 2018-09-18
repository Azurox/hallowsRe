import AIAction from "./AIAction";
import Position from "../RelationalObject/Position";

export default class AIImpact {
  id: string;
  actions: AIAction[] = [];
  constructor(id: string) {
    this.id = id;
  }

  addPath(path: Position[]) {
    const action = new AIAction();
    action.path = path;
    this.actions.push(action);
  }

  addSpell(spell: string, position: Position) {
    const action = new AIAction();
    action.spell = { spell: spell, position: position };
    this.actions.push(action);
  }
}
