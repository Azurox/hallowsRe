import AIAction from "./AIAction";
import Position from "../RelationalObject/Position";
import { ISpell } from "../../Schema/Spell";

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

  addSpell(spell: ISpell, position: Position) {
    const action = new AIAction();
    action.spell = { spell: spell, position: position };
    this.actions.push(action);
  }
}
