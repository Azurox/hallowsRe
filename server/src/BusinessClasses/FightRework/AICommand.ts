import AIAction from "./AIAction";
import Position from "../RelationalObject/Position";
import SpellImpact from "./SpellImpact";

export default class AICommand {
  path: Position[] = [];
  spellId: string;
  targetPosition: Position;
  spellImpacts: SpellImpact[];
}
