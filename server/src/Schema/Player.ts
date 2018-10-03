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
  inFight: boolean;
  lastFightId: string;
  hasSpell(id: string): boolean;
  enterInFight(): Promise<void>;
  setLastFightId(id: string): Promise<void>;
  leaveFight(): Promise<void>;
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
  spells: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Spell" }],
  inFight: { type: Boolean, default: false },
  lastFightId: String
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

PlayerSchema.method("enterInFight", async function(): Promise<void> {
  const player: IPlayer = this;
  if (player.inFight) {
    throw new Error("Player already in fight !!");
  }
  player.inFight = true;
  await player.save();
});

PlayerSchema.method("setLastFightId", async function(fightId: string): Promise<void> {
  const player: IPlayer = this;
  if (!player.inFight) {
    throw new Error("Player not in fight !!");
  }
  player.lastFightId = fightId;
  await player.save();
});


PlayerSchema.method("leaveFight", async function(): Promise<void> {
  const player: IPlayer = this;
  if (!player.inFight) {
    throw new Error("Player not in a fight !!");
  }
  player.inFight = false;
  player.lastFightId = undefined;
  await player.save();
});

const Player = Mongoose.model<IPlayer>("Player", PlayerSchema);
export default Player;
