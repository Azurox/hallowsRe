import State from "./State";
import Fight from "./Fight/Fight";
import { IPlayer } from "../Schema/Player";
import Fighter from "./Fight/Fighter";

export default class FightController {
  state: State;
  fights: { [id: string]: Fight } = {};

  constructor(state: State) {
    this.state = state;
    setInterval(this.tick, 1000);
  }

  startFight(firstTeam: IPlayer[], secondTeam: IPlayer[]): string {
    const fight = new Fight(this.state.io);
    this.fights[fight.id] = fight;

    for (let i = 0; i < firstTeam.length; i++) {
      fight.addFighter(new Fighter(firstTeam[i]), "blue");
    }

    for (let i = 0; i < secondTeam.length; i++) {
      fight.addFighter(new Fighter(firstTeam[i]), "red");
    }

    fight.startFight();

    return fight.id;
  }

  tick() {
    for (const id in this.fights) {
      this.fights[id].tick();
    }
  }
}
