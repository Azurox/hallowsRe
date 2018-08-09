import Mongoose from "mongoose";
import { IPlayer } from "./Player";
export interface IAccount extends Mongoose.Document {
    email: string;
    password: string;
    players: [IPlayer];
  }

  export const AccountSchema = new Mongoose.Schema({
    email: String,
    password: String,
    players: [{ type : Mongoose.Types.ObjectId, ref: "Player" }]
  });

  const Account = Mongoose.model<IAccount>("Account", AccountSchema);
  export default Account;