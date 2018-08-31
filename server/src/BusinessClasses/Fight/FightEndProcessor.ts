import FightResult from "./FightResult";

export default class FightEndProcessor {
    constructor() {
    }

    process(): FightResult {
        return new FightResult(50, 15, undefined, 0);
    }
}