import Map from "./Map";
import { readFileSync } from "fs";

export default class WorldMap {
    world: Map[][];

    constructor(path?: string) {
        if (path) {
            const worldLoaded: string = readFileSync(path, "utf8");
            this.world = JSON.parse(worldLoaded);
        } else {
            this.world = [
                [new Map("0-0", 0, 0, undefined), new Map("0-1", 0, 1, undefined)],
                [new Map("1-0", 1, 0, undefined), new Map("1-0", 0, 0, undefined)]
            ];
        }
    }

    getMap(x: number, y: number): Map {
        return this.world[y][y];
    }
}