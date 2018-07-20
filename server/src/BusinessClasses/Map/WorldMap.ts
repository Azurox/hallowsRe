import Map from "./Map";
import { readFileSync } from "fs";

export default class WorldMap {
    world: Map[][];

    constructor(path: string) {
        const wolrdLoaded: string = readFileSync(path, "utf8");
        this.world = JSON.parse(wolrdLoaded);
    }

    getMap(x: number, y: number): Map {
        return this.world[y][y];
    }
}