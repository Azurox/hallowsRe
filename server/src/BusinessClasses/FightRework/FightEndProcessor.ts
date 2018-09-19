import FightResult from "./FightResult";

export default class FightEndProcessor {
  constructor() {}

  process(win: boolean): FightResult {
    if (win) {
      return new FightResult(win, 50, 15, undefined);
    } else {
      return new FightResult(!win, 0, 0, undefined);
    }
  }
}
