import Checkin from "./Checkin";

export default class ComplexCheckin extends Checkin {
  id: string;
  checkList: { [id: string]: boolean } = {};
  callback: (ids: string[]) => void;
  timeout: number;
  callbackTimeout: (ids: { [id: string]: boolean }) => void;
  timeoutReference: NodeJS.Timer;

  constructor(
    id: string,
    socketIds: string[],
    callback: (ids: string[]) => void,
    timeout: number,
    callbackTimeout: (ids: { [id: string]: boolean }) => void
  ) {
    super();
    this.id = id;
    for (const socketId of socketIds) {
      this.checkList[socketId] = false;
    }
    this.callback = callback;
    this.timeout = timeout;
    this.callbackTimeout = callbackTimeout;
    if (this.timeout) {
      this.timeoutReference = setTimeout(() => {
        this.callbackTimeout(this.checkList);
      }, this.timeout);
    }
  }

  check(socketId: string): boolean {
    if (socketId in this.checkList && this.checkList[socketId] == false) {
      this.checkList[socketId] = true;
    } else {
      console.log("has already checked or doesn't exist");
    }
    const everyoneHasCheked = this.everyoneHasCheked();

    if (everyoneHasCheked) {
      if (this.callbackTimeout) {
        if (this.timeout) {
          clearTimeout(this.timeoutReference);
        }
      }
      this.callback(Object.keys(this.checkList));
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
