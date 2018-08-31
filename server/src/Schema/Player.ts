import Mongoose from "mongoose";
import Position from "../BusinessClasses/RelationalObject/Position";
import { IStats } from "./Stats";

export interface IPlayer extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  socketId: string;
  name: string;
  mapPosition: { x: number; y: number };
  mapName: string;
  position: { x: number; y: number };
  isMoving: boolean;
  path: Position[];
  stats: IStats;
  spells: Mongoose.Types.ObjectId[];
  hasSpell(id: string): boolean;
}

export const PlayerSchema = new Mongoose.Schema({
  name: String,
  socketId: String,
  mapPosition: { x: Number, y: Number },
  mapName: String,
  position: { x: Number, y: Number },
  isMoving: Boolean,
  path: [{ x: Number, y: Number }],
  stats: { type: Mongoose.Schema.Types.ObjectId, ref: "Stats" },
  spells: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Spell" }]
});

PlayerSchema.method("hasSpell", function(id: string): boolean {
  const player: IPlayer = this;
  let hasSpell = false;
  for (let i = 0; i < player.spells.length; i++) {
    if (player.spells[i].equals(id)) {
      hasSpell = true;
    }
  }

  return hasSpell;
});

const Player = Mongoose.model<IPlayer>("Player", PlayerSchema);
export default Player;
