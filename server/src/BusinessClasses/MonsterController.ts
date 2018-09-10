import State from "./State";
import Monster, { IMonster } from "../Schema/Monster";
import Position from "./RelationalObject/Position";
import Zone from "../Schema/Zone";
import MonsterGroup, { IMonsterGroup } from "../Schema/MonsterGroup";
import { IMap } from "../Schema/Map";
import GSocket from "./GSocket";

export default class MonsterController {
  state: State;
  MAX_MONSTERS_GROUP_BY_MAP: number = 3;
  constructor(state: State) {
    this.state = state;
  }

  async SendMapMonsters(socket: GSocket, map: IMap) {
    const monsterGroups = [];
    for (let i = 0; i < map.monsterGroups.length; i++) {
      const group = map.monsterGroups[i];
      monsterGroups.push(await MonsterGroup.findById(group).lean());
    }
    this.SendMonsterGroups(socket, monsterGroups);
  }

  async SendMonsterGroups(socket: GSocket, groups: IMonsterGroup[]) {
    const filteredGroups = [];
    for (let i = 0; i < groups.length; i++) {
      const group = groups[i];
      const filteredGroup: any = {
        monster: [],
        position: group.position
      };

      for (let j = 0; j < group.monsters.length; j++) {
        const monster = await Monster.findById(group.monsters[j]);
        filteredGroup.monster.push({
          id: monster.id,
          name: monster.name,
          level: monster.level
        });
      }
      filteredGroups.push(filteredGroup);
    }
    socket.emit("spawnMonsterGroup", filteredGroups);
  }

  async SpawnMonsters(mapPosition: Position) {
    const map = await this.state.MapController.getMap(mapPosition.x, mapPosition.y);
    if (map.monsterGroups.length < this.MAX_MONSTERS_GROUP_BY_MAP) {
      const zone = await Zone.findById(map.zone);
      const monsters: IMonster[] = [];
      if (zone && zone.forceSpecificMonster) {
        map.monsterGroups.push(zone.specificMonsterGroup);
        await map.save();
        const monsterGroup: IMonsterGroup = await MonsterGroup.findById(zone.specificMonsterGroup).lean();
        for (let i = 0; i < monsterGroup.monsters.length; i++) {
          monsters.push(await Monster.findById(monsterGroup.monsters[i]));
        }
      } else {
        // select random monster from the pool
      }
      this.state.io.to(map.id).emit("spawnMonsterGroup", monsters);
    }
  }
}
