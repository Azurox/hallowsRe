import Cell from "./Cell";
import { IPlayer } from "../../Schema/Player";
import MappedPlayer from "./MappedPlayer";

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

    spawnPlayer(x: number, y: number, player: IPlayer, id: string): boolean {
        let moved = true;
        try {
            this.cells[x][y].addPlayer(id , new MappedPlayer(player));
        } catch (error) {
            console.log(error);
            moved = false;
        }
        return moved;
    }

    movePlayer(x: number, y: number, player: IPlayer, id: string): boolean {
        let moved = true;
        try {
            const mappedPlayer = this.cells[player.position.x][player.position.y].getPlayer(id);
            this.cells[x][y].addPlayer(id, mappedPlayer);
            this.cells[player.position.x][player.position.y].removePlayer(id);
        } catch (error) {
            console.log(error);
            moved = false;
        }

        return moved;
    }
}