import Position from "../RelationalObject/Position";

export default class SpellImpact {
    playerId: string;
    life: number;
    armor: number;
    magicResistance: number;
    attackDamage: number;
    actionPoint: number;
    movementPoint: number;
    position: Position;

    constructor(id: string) {
        this.playerId = id;
    }
}