import Cell from "./Cell";

export default class Map {
    name: string;
    x: number;
    y: number;
    cells: Cell[][];

    constructor(name: string, x: number, y: number, cells: Cell[][]) {
        this.name = name;
        this.x = x;
        this.y = y;
        this.cells = cells;
    }
}