import Checkin from "./Checkin";
import uuid from "uuid/v4";
import SoftCheckin from "./SoftCheckin";

export default class CheckinManager {
    checkins: {[id: string]: Checkin };

    constructor() {}

    createSoftCheckin(socketId: string, callback: (id: string) => void ): string {
        const checkId = uuid();
        this.checkins[checkId] =  new SoftCheckin(socketId, callback);
        return checkId;
    }
  }