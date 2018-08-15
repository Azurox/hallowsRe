import Mongoose from "mongoose";
import uuidv4 from "uuid/v4";
import { ICell } from "./Cell";
import { IPlayer } from "./Player";
import MappedPlayer from "../BusinessClasses/Map/MappedPlayer";
import Position from "../BusinessClasses/RelationalObject/Position";

export interface IMap extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: string;
  x: number;
  y: number;
  cells: Mongoose.Types.ObjectId[][] | ICell[][];
  spawnPlayer(
    x: number,
    y: number,
    player: IPlayer
  ): Promise<[boolean, string]>;
  movePlayer(x: number, y: number, player: IPlayer): Promise<boolean>;
  getPlayers(): MappedPlayer[];
  removePlayer(id: string): Promise<void>;
  checkPath(positions: Position[]): boolean;
}

export const MapSchema = new Mongoose.Schema({
  name: String,
  x: Number,
  y: Number,
  mapPosition: { x: Number, y: Number },
  position: { x: Number, y: Number },
  cells: [[{ type: Mongoose.Schema.Types.ObjectId, ref: "Player" }]]
});

MapSchema.method("spawnPlayer", async function(
  x: number,
  y: number,
  player: IPlayer
) {
  let moved = true;
  const mappedId = uuidv4();
  try {
    await this.cells[x][y].addPlayer(mappedId, new MappedPlayer(player));
  } catch (error) {
    console.log(error);
    moved = false;
  }

  return [moved, mappedId];
});

MapSchema.method("movePlayer", async function(
  x: number,
  y: number,
  player: IPlayer
) {
  let moved = true;
  try {
    const mappedPlayer = this.cells[player.position.x][
      player.position.y
    ].getPlayer(player.mappedId);
    await this.cells[x][y].addPlayer(player.mappedId, mappedPlayer);
    await this.cells[player.position.x][player.position.y].removePlayer(
      player.mappedId
    );
  } catch (error) {
    console.log(error);
    moved = false;
  }

  return moved;
});

MapSchema.method("getPlayers", function() {
  const players: MappedPlayer[] = [];
  for (let i = 0; i < this.cells.length; i++) {
    for (let j = 0; j < this.cells[0].length; j++) {
      players.push(...this.cells[i][j].getPlayers());
    }
  }
  return players;
});

MapSchema.method("removePlayer", async function(id: string) {
  for (let i = 0; i < this.cells.length; i++) {
    for (let j = 0; j < this.cells[0].length; j++) {
      await this.cells[i][j].removePlayer(id);
    }
  }
});

MapSchema.method("checkPath", function(positions: Position[]) {
  // console.log(this.cells);
  try {
    for (let i = 0, len = positions.length; i < len; i++) {
      console.log(this.cells[positions[i].x][positions[i].y]);
      if (!this.cells[positions[i].x][positions[i].y].isAccessible) {
        return false;
      }
    }
  } catch (error) {
    console.log(error);
    return false;
  }
  return true;
});

const Map = Mongoose.model<IMap>("Map", MapSchema);
export default Map;
