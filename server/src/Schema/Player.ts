import Mongoose from "mongoose";
import Position from "../BusinessClasses/RelationalObject/Position";
export interface IPlayer extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  socketId: string;
  name: string;
  mapPosition: { x: number; y: number };
  position: { x: number; y: number };
  isMoving: boolean;
  path: Position[];
}

export const PlayerSchema = new Mongoose.Schema({
  name: String,
  socketId: String,
  mapPosition: { x: Number, y: Number },
  position: { x: Number, y: Number },
  isMoving: Boolean,
  path: [{ x: Number, y: Number }]
});

const Player = Mongoose.model<IPlayer>("Player", PlayerSchema);
export default Player;
