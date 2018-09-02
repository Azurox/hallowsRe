export default class FightResult {
  win: boolean;
  xp: number;
  gold: number;
  loot: string[];

  constructor(win: boolean, xp: number, gold: number, loot: string[]) {
    this.win = win;
    this.xp = xp;
    this.gold = gold;
    this.loot = loot;
  }
}
