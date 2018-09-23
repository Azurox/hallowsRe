import Fight from "./FightRework";
import { IMap } from "../../Schema/Map";
import Fighter from "./Fighter";
import MonsterFighter from "./MonsterFighter";
import PathFinding from "pathfinding";
import AIAction from "./AIAction";
import AIImpact from "./AIImpact";
import Position from "../RelationalObject/Position";
import Spell from "../../Schema/Spell";

export default class AIProcessor {
  fight: Fight;
  monster: MonsterFighter;
  map: IMap;
  blueTeam: Fighter[];
  redTeam: Fighter[];
  fighterOrder: Fighter[];

  constructor(
    fight: Fight,
    monster: MonsterFighter,
    map: IMap,
    blueTeam: Fighter[],
    redTeam: Fighter[],
    fightOrder: Fighter[]
  ) {
    this.fight = fight;
    this.monster = monster;
    this.map = map;
    this.blueTeam = blueTeam;
    this.redTeam = redTeam;
    this.fighterOrder = fightOrder;
  }

  async process(): Promise<AIImpact> {
    const weakestTarget = this.findWeakestTarget();
    const grid = this.constructGrid();
    grid.setWalkableAt(weakestTarget.position.x, weakestTarget.position.y, true);
    grid.setWalkableAt(this.monster.position.x, this.monster.position.y, true);
    const finder = new PathFinding.BestFirstFinder();
    const path = finder.findPath(
      this.monster.position.x,
      this.monster.position.y,
      weakestTarget.position.x,
      weakestTarget.position.y,
      grid
    );
    const impact = new AIImpact(this.monster.getId());
    path.splice(0, 1);
    const clearedPath: Position[] = [];
    let useSpell = false;
    for (let i = 0; i < this.monster.currentMovementPoint; i++) {
      if (path[i]) {
        if (path[i][0] == weakestTarget.position.x && path[i][1] == weakestTarget.position.y) {
          useSpell = true;
          break;
        } else {
          clearedPath.push(new Position(path[i][0], path[i][1]));
        }
      }
    }
    impact.addPath(clearedPath);
    if (useSpell) {
      impact.addSpell(await Spell.findById(this.monster.getSpells()[0]), weakestTarget.position);
    }
    return impact;
  }

  findWeakestTarget(): Fighter {
    let searchList;
    if (this.monster.side == "blue") {
      searchList = this.redTeam;
    } else {
      searchList = this.blueTeam;
    }

    let weakestTarget;
    for (let i = 0; i < searchList.length; i++) {
      if (!weakestTarget) {
        if (!searchList[i].dead) {
          weakestTarget = searchList[i];
        }
      } else {
        if (!searchList[i].dead && searchList[i].currentLife < weakestTarget.currentLife) {
          weakestTarget = searchList[i];
        }
      }
    }
    return weakestTarget;
  }

  constructGrid(): PathFinding.Grid {
    const grid = new PathFinding.Grid(this.map.cells.length, this.map.cells[0].length);
    for (let i = 0; i < this.map.cells.length; i++) {
      for (let j = 0; j < this.map.cells[i].length; j++) {
        const cell = this.map.cells[i][j];
        if (cell.offScreen || !cell.isAccessible || cell.obstacle) {
          grid.setWalkableAt(i, j, false);
        }
      }
    }

    for (let i = 0; i < this.fighterOrder.length; i++) {
      grid.setWalkableAt(this.fighterOrder[i].position.x, this.fighterOrder[i].position.y, false);
    }
    return grid;
  }
}
