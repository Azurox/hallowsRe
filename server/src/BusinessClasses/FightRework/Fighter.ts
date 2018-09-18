import Position from "../RelationalObject/Position";
import { ISpell } from "../../Schema/Spell";
import mongoose from "mongoose";
export default abstract class Fighter {
    side: Side;
    position: Position;
    ready: boolean = false;

    abstract getId(): string;
    abstract getName(): string;
    abstract getSocketId(): string;
    abstract getSpells(): mongoose.Types.ObjectId[];
    abstract isRealPlayer(): boolean;

    /** Stats */
    life: number;
    currentLife: number;
    speed: number;
    currentSpeed: number;
    armor: number;
    currentArmor: number;
    magicResistance: number;
    currentMagicResistance: number;
    attackDamage: number;
    currentAttackDamage: number;
    movementPoint: number;
    currentMovementPoint: number;
    actionPoint: number;
    currentActionPoint: number;
    dead: boolean = false;
}