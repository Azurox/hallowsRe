import Map, { IMap } from "../../Schema/Map";
import Cell from "../../Schema/Cell";
import Account from "../../Schema/Account";
import Player from "../../Schema/Player";
import Position from "../RelationalObject/Position";
import Stats from "../../Schema/Stats";
import Spell from "../../Schema/Spell";

export default class WorldMap {
  constructor() {
    this.saveMap();
    // this.saveAccount();
  }

  async saveMap() {
    await Map.remove({});
    await Cell.remove({});
    const map = new Map();
    map.x = 0;
    map.y = 0;
    map.name = "0-0.json";
    map.cells = [];
    for (let i = 0; i < 10; i++) {
      map.cells[i] = [];
      for (let j = 0; j < 10; j++) {
        const cell = new Cell();
        cell.x = i;
        cell.y = j;
        cell.isAccessible = true;
        if (i == j) {
          // cell.isAccessible = false;
        }
        await cell.save();
        map.cells[i][j] = cell._id;
      }
    }

    map.redCells = [new Position(0, 5), new Position(0, 7), new Position(0, 9)];
    map.blueCells = [new Position(5, 0), new Position(7, 0), new Position(9, 0)];

    await map.save();
  }

  async saveAccount() {
    await Account.remove({});
    await Player.remove({});
    await Stats.remove({});
    await Spell.remove({});

    const spell = new Spell();
    spell.hitArea = [new Position(0, 0)];
    spell.name = "Punch";
    spell.actionPointCost = 4;
    spell.physicalDamage = 7;
    spell.magicDamage = 0;
    spell.range = 1;
    await spell.save();

    const stats1 = new Stats();
    stats1.life = 25;
    stats1.speed = 10;
    stats1.armor = 3;
    stats1.magicResistance = 3;
    stats1.attackDamage = 6;
    stats1.movementPoint = 3;
    stats1.actionPoint = 6;
    await stats1.save();

    const player1 = new Player();
    player1.name = "test1";
    player1.position = { x: 0, y: 0 };
    player1.mapPosition = { x: 0, y: 0 };
    player1.stats = stats1;
    player1.spells = [spell];
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
    player2.position = { x: 2, y: 2 };
    player2.mapPosition = { x: 0, y: 0 };
    player2.stats = stats2;
    player2.spells = [spell];
    await player2.save();

    const account = new Account();
    account.players = [player1, player2];
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
