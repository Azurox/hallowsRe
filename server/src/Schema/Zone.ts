import Mongoose from "mongoose";

export interface IZone extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: String;
  monsterPool: { monster: Mongoose.Types.ObjectId; probability: number }[];
  forceSpecificMonster: boolean;
  specificMonsterGroup: Mongoose.Types.ObjectId;
  maximumMonsterGroups: number;
  maximumMonsterGroupSize: number;
}

export const ZoneSchema = new Mongoose.Schema({
  name: String,
  monsterPool: [{ monster: { type: Mongoose.Schema.Types.ObjectId, ref: "Monster" }, probability: Number }],
  maximumMonsterGroups: Number,
  maximumMonsterGroupSize: Number,
  forceSpecificMonster: Boolean,
  specificMonsterGroup: { type: Mongoose.Schema.Types.ObjectId, ref: "MonsterGroup" }
});

const Zone = Mongoose.model<IZone>("Zone", ZoneSchema);
export default Zone;
