import Mongoose from "mongoose";
import MappedPlayer from "../BusinessClasses/Map/MappedPlayer";

export interface ICell extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  x: number;
  y: number;
  isAccessible: boolean;
  players: { [id: string]: MappedPlayer };
  addPlayer(id: string, player: MappedPlayer): Promise<void>;
  getPlayer(id: string): MappedPlayer;
  removePlayer(id: string): Promise<void>;
  getPlayers(): MappedPlayer[];
}

export const CellSchema = new Mongoose.Schema({
  x: Number,
  y: Number,
  isAccessible: Boolean,
  players: { type: Mongoose.Schema.Types.Mixed, default: {} }
});

CellSchema.method("addPlayer", async function(
  id: string,
  player: MappedPlayer
) {
  if (this.isAccessible) {
    this.players[id] = player;
    await this.save();
    // console.log("saved new player to position");
    // console.log(this.players);
  } else {
    throw Error("Unauthorized Movement");
  }
});

CellSchema.method("getPlayer", function(id: string) {
  return this.players[id];
});

CellSchema.method("removePlayer", async function(id: string) {
  delete this.players[id];
  await this.save();
});

CellSchema.method("getPlayers", function() {
  return Object.values(this.players);
});

const Cell = Mongoose.model<ICell>("Cell", CellSchema);
export default Cell;
