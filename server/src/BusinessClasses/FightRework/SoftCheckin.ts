import Checkin from "./Checkin";

export default class SoftCheckin extends Checkin {
    socketId: string;
    callback: (id: string) => void;

    constructor(socketId: string, callback: (id: string) => void) {
        super();
        this.socketId = socketId;
        this.callback = callback;
    }

    check(socketId: string): boolean {
        this.callback(socketId);
        return true;
    }
  }