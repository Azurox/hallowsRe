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

  startFight(firstTeam: IPlayer[], secondTeam: IPlayer[], map: IMap): string {
    const blueTeam: HumanFighter[] = [];
    for (let i = 0; i < firstTeam.length; i++) {
      blueTeam.push(new HumanFighter(firstTeam[i], "blue"));
    }

    const redTeam: HumanFighter[] = [];
    for (let i = 0; i < secondTeam.length; i++) {
      redTeam.push(new HumanFighter(secondTeam[i], "red"));
    }

    const fight = new FightRework(this.state.io, blueTeam, redTeam, map);
    this.fights[fight.id] = fight;
    fight.startFight();
    return fight.id;
  }

  startMonsterFight(firstTeam: IPlayer[], secondTeam: IMonster[], map: IMap, monsterGroupId: string): string {
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
      this.state.MonsterController.monsterFightFinished(monsterGroupId, new Position(map.x, map.y));
    });

    return fight.id;
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

  disconnectPlayerFromFight(socketId: string, fightId: string) {
   if (this.fights[fightId]) {
    this.fights[fightId].disconnectPlayer(socketId);
   }
  }
}
