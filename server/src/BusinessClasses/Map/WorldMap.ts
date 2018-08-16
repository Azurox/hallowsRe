import { readFileSync } from "fs";
import Map, { IMap } from "../../Schema/Map";
import Cell from "../../Schema/Cell";
import Account from "../../Schema/Account";
import Player from "../../Schema/Player";

export default class WorldMap {
  constructor() {
    /*
    if (path) {
      const worldLoaded: string = readFileSync(path, "utf8");
      this.world = JSON.parse(worldLoaded);
    } else {
      const cells: Cell[][] = [
        [new Cell(0, 0), new Cell(1, 0), new Cell(2, 0)],
        [new Cell(0, 1), new Cell(1, 1), new Cell(2, 1)]
      ];

      this.world = [
        [new Map("0-0.json", 0, 0, cells), new Map("1-0.json", 1, 0, cells)],
        [new Map("0-1.json", 0, 1, cells), new Map("1-1.json", 1, 1, cells)]
      ];
    }*/
    this.saveMap();
    this.saveAccount();
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
        // cell.players = [];
        await cell.save();
        map.cells[i][j] = cell._id;
      }
    }
    await map.save();
  }

  async saveAccount() {
    await Account.remove({});
    await Player.remove({});

    const player1 = new Player();
    player1.name = "test1";
    player1.position = { x: 0, y: 0 };
    player1.mapPosition = { x: 0, y: 0 };
    player1.save();

    const player2 = new Player();
    player2.name = "test1";
    player2.position = { x: 2, y: 2 };
    player2.mapPosition = { x: 0, y: 0 };
    player2.save();

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
}
