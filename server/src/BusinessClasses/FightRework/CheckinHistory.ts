export default class CheckinHistory {
    fightCheckin: string[] = [];

    addFightCheckin(id: string) {
        this.fightCheckin.push(id);
    }

    removeFightCheckin(id: string) {
        const index = this.fightCheckin.indexOf(id);
        if (index > -1) {
            this.fightCheckin.splice(index, 1);
        }
    }

    getFightCheckin(): string[] {
        return this.fightCheckin;
    }
}
