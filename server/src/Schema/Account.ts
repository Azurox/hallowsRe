import Mongoose from "mongoose";
import Player, { IPlayer } from "./Player";
export interface IAccount extends Mongoose.Document {
  email: string;
  password: string;
  players: [Mongoose.Types.ObjectId | IPlayer];
  newPlayer(): void;
}

export const AccountSchema = new Mongoose.Schema({
  email: String,
  password: String,
  players: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Player" }]
});

AccountSchema.method("newPlayer", function() {
  const account: IAccount = this;
  const player = new Player();
  player.name = "test2";
  player.position = { x: 0, y: 0 };
  player.mapPosition = { x: 0, y: 0 };
  player.save();
  account.players.push(player._id);
  account.save();
});

const Account = Mongoose.model<IAccount>("Account", AccountSchema);
export default Account;
