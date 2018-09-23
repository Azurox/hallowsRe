import State from "./State";
import { IPlayer } from "../Schema/Player";
import HumanFighter from "./FightRework/HumanFighter";
import { IMap } from "../Schema/Map";
import Monster, { IMonster } from "../Schema/Monster";
import MonsterFighter from "./FightRework/MonsterFighter";
import Position from "./RelationalObject/Position";
import FightRework from "./FightRework/FightRework";

export default class FightController {
  state: State;
  fights: { [id: string]: FightRework } = {};

  constructor(state: State) {
    this.state = state;
    setInterval(this.tick.bind(this), 1000);
  }

  startFight(firstTeam: IPlayer[], secondTeam: IPlayer[], map: IMap) {
    /*const fight = new FightRework(this.state.io, map);
    // this.fights[fight.id] = fight;

    for (let i = 0; i < firstTeam.length; i++) {
      fight.addFighter(new HumanFighter(firstTeam[i], "blue"));
    }

    for (let i = 0; i < secondTeam.length; i++) {
      fight.addFighter(new HumanFighter(secondTeam[i], "red"));
    }

    fight.startFight();*/
  }

  /* startMonsterFight(
    firstTeam: IPlayer[],
    secondTeam: IMonster[],
    monsterGroupId: string,
    map: IMap,
    callback: (id: string, mapPosition: Position) => void
  ) {
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
  }*/

  startMonsterFight(firstTeam: IPlayer[], secondTeam: IMonster[], map: IMap) {
    const blueTeam: HumanFighter[] = [];
    for (let i = 0; i < firstTeam.length; i++) {
      blueTeam.push(new HumanFighter(firstTeam[i], "blue"));
    }

    const redTeam: MonsterFighter[] = [];
    for (let i = 0; i < secondTeam.length; i++) {
      redTeam.push(new MonsterFighter(secondTeam[i], "red"));
    }

    const fight = new FightRework(this.state.io, blueTeam, redTeam, map);
    this.fights[fight.id] = fight;

    fight.startFight();
    fight.on("end", () => {
      this.removeFight(fight.id);
    });
  }

  tick() {
    for (const id in this.fights) {
      this.fights[id].tick();
    }
  }

  retrieveFight(id: string): FightRework {
    return this.fights[id];
  }

  removeFight(id: string) {
    this.fights[id] = undefined;
    delete this.fights[id];
  }
}
