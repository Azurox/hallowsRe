import Mongoose from "mongoose";
import Position from "../BusinessClasses/RelationalObject/Position";

export interface IMonsterGroup extends Mongoose.Document {
  monsters: Mongoose.Types.ObjectId[];
  position: { x: number; y: number };
  volatile: boolean;
}

export const MonsterGroupSchema = new Mongoose.Schema({
  monsters: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Monster", default: [] }],
  position: { x: Number, y: Number },
  volatile: { type: Boolean, default: true }
});

const MonsterGroup = Mongoose.model<IMonsterGroup>("MonsterGroup", MonsterGroupSchema);
export default MonsterGroup;
