import Mongoose from "mongoose";

export interface IStats extends Mongoose.Document {
  life: number;
  speed: number;
  armor: number;
  magicResistance: number;
  attackDamage: number;
  movementPoint: number;
  actionPoint: number;

  resetStats(): void;
}

export const StatsSchema = new Mongoose.Schema({
  life: Number,
  speed: Number,
  armor: Number,
  magicResistance: Number,
  attackDamage: Number,
  movementPoint: Number,
  actionPoint: Number
});

const Stats = Mongoose.model<IStats>("Stats", StatsSchema);
export default Stats;
