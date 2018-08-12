import Mongoose from "mongoose";
export interface IPlayer extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: string;
  mapPosition: { x: number; y: number };
  position: { x: number; y: number };
  mappedId: string;
}

export const PlayerSchema = new Mongoose.Schema({
  name: String,
  mapPosition: { x: Number, y: Number },
  position: { x: Number, y: Number }
});

const Player = Mongoose.model<IPlayer>("Player", PlayerSchema);
export default Player;
