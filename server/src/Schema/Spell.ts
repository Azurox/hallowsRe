import Position from "../BusinessClasses/RelationalObject/Position";
import Mongoose from "mongoose";

export interface ISpell extends Mongoose.Document {
  name: string;
  actionPointCost: number;
  range: number;
  hitArea: Position[];
  physicalDamage: number;
  magicDamage: number;
}

export const SpellSchema = new Mongoose.Schema({
  name: String,
  actionPointCost: Number,
  range: Number,
  hitArea: [{ x: Number, y: Number }],
  physicalDamage: Number,
  magicDamage: Number
});

const Spell = Mongoose.model<ISpell>("Spell", SpellSchema);
export default Spell;
