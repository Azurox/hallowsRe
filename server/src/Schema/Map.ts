import Mongoose from "mongoose";
import { ICell } from "./Cell";
import { IPlayer } from "./Player";
import Position from "../BusinessClasses/RelationalObject/Position";

export interface IMap extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: string;
  x: number;
  y: number;
  cells: ICell[][];
  spawnPlayer(x: number, y: number, player: IPlayer): Promise<boolean>;
  movePlayer(player: IPlayer, position: Position): Promise<void>;
  removePlayer(id: Mongoose.Types.ObjectId): Promise<void>;
  checkPath(positions: Position[]): boolean;
  checkPosition(position: Position, target: Position): boolean;
  getPlayers(): Mongoose.Types.ObjectId[];
}

export const MapSchema = new Mongoose.Schema({
  name: String,
  x: Number,
  y: Number,
  mapPosition: { x: Number, y: Number },
  position: { x: Number, y: Number },
  cells: [[{ type: Mongoose.Schema.Types.ObjectId, ref: "Player" }]]
});

MapSchema.method("spawnPlayer", async function(x: number, y: number, player: IPlayer): Promise<boolean> {
  const map: IMap = this;
  let moved = true;

  try {
    await map.cells[x][y].addPlayer(player._id);
  } catch (error) {
    console.log(error);
    moved = false;
  }

  return moved;
});

MapSchema.method("movePlayer", async function(player: IPlayer, position: Position): Promise<void> {
  const map: IMap = this;
  await map.cells[player.position.x][player.position.y].removePlayer(player._id);
  await map.cells[position.x][position.y].addPlayer(player._id);
});

MapSchema.method("removePlayer", async function(id: Mongoose.Types.ObjectId) {
  const map: IMap = this;
  for (let i = 0; i < map.cells.length; i++) {
    for (let j = 0; j < map.cells[0].length; j++) {
      await map.cells[i][j].removePlayer(id);
    }
  }
});

MapSchema.method("checkPath", function(positions: Position[]) {
  const map: IMap = this;
  try {
    for (let i = 0, len = positions.length; i < len; i++) {
      if (!map.cells[positions[i].x][positions[i].y].isAccessible) {
        return false;
      }
    }
  } catch (error) {
    console.log(error);
    return false;
  }
  return true;
});

MapSchema.method("checkPosition", function(position: Position, target: Position): boolean {
  const map: IMap = this;
  try {
    if (!map.cells[target.x][target.y].isAccessible) {
      return false;
    }
  } catch (error) {
    console.log(error);
    return false;
  }
  return true;
});

MapSchema.method("getPlayers", function(): Mongoose.Types.ObjectId[] {
  const map: IMap = this;
  const players: Mongoose.Types.ObjectId[] = [];
  for (let i = 0; i < map.cells.length; i++) {
    for (let j = 0; j < map.cells[0].length; j++) {
      players.push(...map.cells[i][j].players);
    }
  }
  return players;
});

const Map = Mongoose.model<IMap>("Map", MapSchema);
export default Map;
