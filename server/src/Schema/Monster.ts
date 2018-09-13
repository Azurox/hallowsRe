import Mongoose from "mongoose";
import { IStats } from "./Stats";
import { ISpell } from "./Spell";

export interface IMonster extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: string;
  level: number;
  stats: IStats;
  spells: Mongoose.Types.ObjectId[];
  loot: { objectId: Mongoose.Types.ObjectId; probability: number }[];
}

export const MonsterSchema = new Mongoose.Schema({
  name: String,
  level: Number,
  stats: { type: Mongoose.Schema.Types.ObjectId, ref: "Stats" },
  spells: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Spell" }],
  loot: [{ itemId: { type: Mongoose.Schema.Types.ObjectId, ref: "Item" }, probability: Number }]
});

const Monster = Mongoose.model<IMonster>("Monster", MonsterSchema);
export default Monster;
