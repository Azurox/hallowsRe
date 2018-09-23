import Position from "../RelationalObject/Position";
import { ISpell } from "../../Schema/Spell";

export default class AIAction {
  path: Position[];
  spell: { spell: ISpell; position: Position };
}
