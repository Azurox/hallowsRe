import State from "./State";
import Fight from "./Fight/Fight";
import { IPlayer } from "../Schema/Player";
import HumanFighter from "./Fight/HumanFighter";
import { IMap } from "../Schema/Map";
import { IMonster } from "../Schema/Monster";
import MonsterFighter from "./Fight/MonsterFighter";
import Position from "./RelationalObject/Position";

export default class FightController {
  state: State;
  fights: { [id: string]: Fight } = {};

  constructor(state: State) {
    this.state = state;
    setInterval(this.tick.bind(this), 1000);
  }

  startFight(firstTeam: IPlayer[], secondTeam: IPlayer[], map: IMap) {
    const fight = new Fight(this.state.io, map);
    this.fights[fight.id] = fight;

    for (let i = 0; i < firstTeam.length; i++) {
      fight.addFighter(new HumanFighter(firstTeam[i], "blue"));
    }

    for (let i = 0; i < secondTeam.length; i++) {
      fight.addFighter(new HumanFighter(secondTeam[i], "red"));
    }

    fight.startFight();
  }

  startMonsterFight(firstTeam: IPlayer[], secondTeam: IMonster[], monsterGroupId: string , map: IMap, callback: (id: string, mapPosition: Position) => void) {
    const fight = new Fight(this.state.io, map);
    fight.monsterGroupId = monsterGroupId;
    this.fights[fight.id] = fight;

    for (let i = 0; i < firstTeam.length; i++) {
      fight.addFighter(new HumanFighter(firstTeam[i], "blue"));
    }

    for (let i = 0; i < secondTeam.length; i++) {
      fight.addFighter(new MonsterFighter(secondTeam[i], "red"));
    }

    fight.startFight(callback);
  }

  tick() {
    for (const id in this.fights) {
      this.fights[id].tick();
    }
  }

  retrieveFight(id: string): Fight {
    return this.fights[id];
  }

  removeFight(id: string) {
    delete this.fights[id];
  }
}
