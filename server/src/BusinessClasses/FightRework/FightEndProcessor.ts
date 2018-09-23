import FightResult from "./FightResult";

export default class FightEndProcessor {
  constructor() {}

  process(win: boolean): FightResult {
    if (win) {
      return new FightResult(true, 50, 15, undefined);
    } else {
      return new FightResult(false, 0, 0, undefined);
    }
  }
}
