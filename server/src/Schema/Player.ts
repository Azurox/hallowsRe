import Mongoose from "mongoose";
export interface IPlayer extends Mongoose.Document {
    mapPosition: {x: number, y: number};
    position: {x: number, y: number};
  }

  export const PlayerSchema = new Mongoose.Schema({
    mapPosition: {x: Number, y: Number},
    position: {x: Number, y: Number}
  });

  const Player = Mongoose.model<IPlayer>("Player", PlayerSchema);
  export default Player;