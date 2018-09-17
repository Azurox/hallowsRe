import Position from "../RelationalObject/Position";

export default class AIAction {
  path: Position[] = [];
  spell: { spell: string; position: Position };
}
