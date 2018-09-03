import Mongoose from "mongoose";
import Position from "../BusinessClasses/RelationalObject/Position";
import { IPlayer } from "./Player";

export interface ICell extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  x: number;
  y: number;
  isAccessible: boolean;
  obstacle: boolean;
  offScreen: boolean;
  players: Mongoose.Types.ObjectId[];
  addPlayer(id: Mongoose.Types.ObjectId): Promise<void>;
  getPlayer(id: Mongoose.Types.ObjectId): Mongoose.Types.ObjectId;
  removePlayer(id: Mongoose.Types.ObjectId): Promise<void>;
  getPlayers(): Mongoose.Types.ObjectId[];
}

export const CellSchema = new Mongoose.Schema({
  x: Number,
  y: Number,
  isAccessible: Boolean,
  obstacle: Boolean,
  offScreen: Boolean,
  players: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Player" }]
});

CellSchema.method("addPlayer", async function(id: Mongoose.Types.ObjectId) {
  const cell: ICell = this;
  if (cell.isAccessible) {
    cell.players.push(id);
    await cell.save();
  } else {
    throw Error("Unauthorized Movement");
  }
});

CellSchema.method("getPlayer", function(id: string): Mongoose.Types.ObjectId {
  const cell: ICell = this;
  for (let i = 0; i < cell.players.length; i++) {
    if (cell.players[i].equals(id)) {
      return cell.players[i];
    }
  }
});

CellSchema.method("removePlayer", async function(id: Mongoose.Types.ObjectId) {
  const cell: ICell = this;
  for (let i = 0; i < cell.players.length; i++) {
    if (id.equals(cell.players[i])) {
      cell.players.splice(i, 1);
      break;
    }
  }
  await cell.save();
});

CellSchema.method("getPlayers", function() {
  return this.players;
});

const Cell = Mongoose.model<ICell>("Cell", CellSchema);
export default Cell;
