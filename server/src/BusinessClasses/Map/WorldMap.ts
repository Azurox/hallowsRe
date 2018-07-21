import Map from "./Map";
import { readFileSync } from "fs";
import Cell from "./Cell";

export default class WorldMap {
    world: Map[][];

    constructor(path?: string) {
        if (path) {
            const worldLoaded: string = readFileSync(path, "utf8");
            this.world = JSON.parse(worldLoaded);
        } else {

            const cells: Cell[][] = [[new Cell(0, 0), new Cell(1, 0), new Cell(2, 0)],
            [new Cell(0, 1), new Cell(1, 1), new Cell(2, 1)]];

            this.world = [
                [new Map("0-0.json", 0, 0, cells), new Map("1-0.json", 1, 0, cells)],
                [new Map("0-1.json", 0, 1, cells), new Map("1-1.json", 1, 1, cells)]
            ];
        }
    }

    getMap(x: number, y: number): Map {
        return this.world[x][y];
    }
}