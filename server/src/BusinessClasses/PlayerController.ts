import GSocket from "./GSocket";
import State from "./State";
import Account, { IAccount } from "../Schema/Account";
import { IPlayer } from "../Schema/Player";

export default class PlayerController {
  state: State;
  pair = 0; // TODO: remove

  constructor(state: State) {
    this.state = state;
  }

  async RetrievePlayer(socket: GSocket) {
    const randomAccount = await Account.findOne().populate("players");
    socket.player = <IPlayer>randomAccount.players[this.pair % 2];
    this.pair++;
  }
}
