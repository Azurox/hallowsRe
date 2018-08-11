import { IPlayer } from "../../Schema/Player";

export default class MappedPlayer {
    isMoving: boolean;
    path: [{x: number, y: number}];
    player: IPlayer;

    constructor(player: IPlayer) {
        this.player = player;
    }
}