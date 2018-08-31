export default class FightResult {
    xp: number;
    gold: number;
    loot: string[];
    levelUp: number;

    constructor(xp: number, gold: number, loot: string[], levelUp: number) {
        this.xp = xp;
        this.gold = gold;
        this.loot = loot;
        this.levelUp = levelUp;
    }
}