import Checkin from "./Checkin";

export default class SoftCheckin extends Checkin {
  checkList: { [id: string]: boolean } = {};
  callback: (id: string) => void;
  endCallBack: () => void;

  constructor(socketIds: string[], callback: (id: string) => void, endCallBack: () => void) {
    super();
    for (const socketId of socketIds) {
      this.checkList[socketId] = false;
    }
    this.callback = callback;
    this.endCallBack = endCallBack;
  }

  check(socketId: string): boolean {
    if (socketId in this.checkList && this.checkList[socketId] == false) {
      this.checkList[socketId] = true;
      this.callback(socketId);
    } else {
      console.log("has already checked or doesn't exist");
    }

    const everyoneHasCheked = this.everyoneHasCheked();
    if (everyoneHasCheked) {
      this.endCallBack();
      return true;
    } else {
      return false;
    }
  }

  everyoneHasCheked(): boolean {
    let everyOneChecked = true;
    for (const socketId in this.checkList) {
      if (!this.checkList[socketId]) everyOneChecked = false;
    }
    return everyOneChecked;
  }
}
