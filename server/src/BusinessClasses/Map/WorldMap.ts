import Map, { IMap } from "../../Schema/Map";
import Cell from "../../Schema/Cell";
import Account from "../../Schema/Account";
import Player from "../../Schema/Player";
import Position from "../RelationalObject/Position";
import Stats from "../../Schema/Stats";
import Spell from "../../Schema/Spell";
import Monster from "../../Schema/Monster";
import MonsterGroup from "../../Schema/MonsterGroup";
import Zone from "../../Schema/Zone";
import State from "../State";

export default class WorldMap {
  state: State;
  constructor(state: State) {
    this.state = state;
    this.saveMap();
    this.saveAccount();
  }

  async saveMap() {
    await Map.remove({});
    await Cell.remove({});
    await MonsterGroup.remove({});
    await Monster.remove({});
    const map = new Map();
    map.x = 0;
    map.y = 0;
    map.name = "0-0";
    map.cells = [];
    for (let i = 0; i < 30; i++) {
      map.cells[i] = [];
      for (let j = 0; j < 30; j++) {
        const cell = new Cell();
        cell.x = i;
        cell.y = j;
        cell.isAccessible = true;
        cell.obstacle = false;
        cell.offScreen = false;

        if (i + j <= 14) {
          cell.offScreen = true;
        }

        if (i + j > 44) {
          cell.offScreen = true;
        }

        if (i - j > 15 || j - i > 16) {
          cell.offScreen = true;
        }

        await cell.save();
        map.cells[i][j] = cell.id;
      }
    }
    map.placementCells = [
      { position: new Position(20, 11).toSimplePosition(), color: "blue" },
      { position: new Position(20, 14).toSimplePosition(), color: "blue" },
      { position: new Position(20, 17).toSimplePosition(), color: "blue" },
      { position: new Position(17, 15).toSimplePosition(), color: "blue" },
      { position: new Position(14, 17).toSimplePosition(), color: "red" },
      { position: new Position(14, 14).toSimplePosition(), color: "red" },
      { position: new Position(14, 11).toSimplePosition(), color: "red" },
      { position: new Position(17, 16).toSimplePosition(), color: "red" }
    ];

    const mobStats = new Stats();
    mobStats.life = 15;
    mobStats.speed = 5;
    mobStats.armor = 3;
    mobStats.magicResistance = 3;
    mobStats.attackDamage = 6;
    mobStats.movementPoint = 4;
    mobStats.actionPoint = 6;
    await mobStats.save();

    const mobSpell = await Spell.findOne({ name: "Peck" });
    mobSpell.hitArea = [new Position(0, 0)];
    mobSpell.name = "Peck";
    mobSpell.actionPointCost = 3;
    mobSpell.physicalDamage = 4;
    mobSpell.magicDamage = 0;
    mobSpell.range = 1;
    mobSpell.selfUse = false;
    mobSpell.line = false;
    mobSpell.heal = false;
    mobSpell.ignoreObstacle = false;
    await mobSpell.save();
    console.log("mob spell saved with id : " + mobSpell.id);

    const mob = new Monster();
    mob.name = "Bims";
    mob.level = 1;
    mob.stats = mobStats;
    mob.spells = [mobSpell.id];
    mob.loot = [];
    await mob.save();

    const mob2 = new Monster();
    mob2.name = "Grand Bims";
    mob2.level = 1;
    mob2.stats = mobStats;
    mob2.spells = [mobSpell.id];
    mob2.loot = [];
    await mob2.save();

    const mobGroup = new MonsterGroup();
    mobGroup.monsters = [mob.id, mob2.id];
    mobGroup.volatile = false;
    mobGroup.position = { x: 15, y: 15 };
    await mobGroup.save();
    map.monsterGroups = [mobGroup.id];

    const zone = new Zone();
    zone.name = "Freljord";
    zone.forceSpecificMonster = true;
    zone.specificMonsterGroup = mobGroup.id;
    zone.maximumMonsterGroups = 4;
    await zone.save();

    map.zone = zone.id;
    await map.save();
    this.state.MonsterController.SetMovingMonsterGroup(new Position(map.x, map.y), mobGroup.id);
  }

  async saveAccount() {
    await Account.remove({});
    await Player.remove({});
    await Stats.remove({});
    // await Spell.remove({});

    const spell = await Spell.findOne({ name: "Punch" });
    spell.hitArea = [new Position(0, 0)];
    spell.name = "Punch";
    spell.actionPointCost = 4;
    spell.physicalDamage = 30;
    spell.magicDamage = 0;
    spell.range = 4;
    spell.selfUse = false;
    spell.line = false;
    spell.heal = false;
    spell.ignoreObstacle = false;
    await spell.save();
    console.log("spell saved with id : " + spell.id);

    const stats1 = new Stats();
    stats1.life = 25;
    stats1.speed = 10;
    stats1.armor = 3;
    stats1.magicResistance = 3;
    stats1.attackDamage = 6;
    stats1.movementPoint = 15;
    stats1.actionPoint = 6;
    await stats1.save();

    const player1 = new Player();
    player1.name = "test1";
    player1.position = { x: 16, y: 10 };
    player1.mapPosition = { x: 0, y: 0 };
    player1.stats = stats1;
    player1.spells = [spell.id];
    await player1.save();

    const stats2 = new Stats();
    stats2.life = 30;
    stats2.speed = 8;
    stats2.armor = 10;
    stats2.magicResistance = 10;
    stats2.attackDamage = 4;
    stats2.movementPoint = 3;
    stats2.actionPoint = 6;
    await stats2.save();

    const player2 = new Player();
    player2.name = "test2";
    player2.position = { x: 7, y: 16 };
    player2.mapPosition = { x: 0, y: 0 };
    player2.stats = stats2;
    player2.spells = [spell.id];
    await player2.save();

    const account = new Account();
    account.players = [player1.id, player2.id];
    await account.save();
  }

  async getMap(x: number, y: number): Promise<IMap> {
    const map = await Map.findOne({ x: x, y: y }).populate({
      path: "cells",
      model: "Cell"
    });
    return map;
  }

  async getLeanMap(x: number, y: number): Promise<IMap> {
    const map = await Map.findOne({ x: x, y: y }).lean();
    return map;
  }
}
