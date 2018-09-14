import Mongoose from "mongoose";
import Player, { IPlayer } from "./Player";
export interface IAccount extends Mongoose.Document {
  email: string;
  password: string;
  players: IPlayer[];
  newPlayer(): Promise<void>;
}

export const AccountSchema = new Mongoose.Schema({
  email: String,
  password: String,
  players: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Player" }]
});

AccountSchema.method("newPlayer", async function() {
  const account: IAccount = this;
  const player = new Player();
  player.name = "test2";
  player.position = { x: 0, y: 0 };
  player.mapPosition = { x: 0, y: 0 };
  await player.save();
  account.players.push(player.id);
  await account.save();
});

const Account = Mongoose.model<IAccount>("Account", AccountSchema);
export default Account;
