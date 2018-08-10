import MappedPlayer from "./MappedPlayer";

export default class Cell {
    x: number;
    y: number;
    isAccessible: boolean;
    players: { [id: string]: MappedPlayer; } = {};

    constructor(x: number, y: number, isAccessible: boolean = true) {
        this.x = x;
        this.y = y;
        this.isAccessible = isAccessible;
    }

    addPlayer(id: string, player: MappedPlayer) {
        if (this.isAccessible) {
            this.players[id] = player;
        } else {
            throw Error("Unauthorized Movement");
        }
    }

    getPlayer(id: string): MappedPlayer {
        return this.players[id];
    }

    removePlayer(id: string): void {
        delete this.players[id];
    }
}