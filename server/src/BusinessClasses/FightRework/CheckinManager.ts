import Checkin from "./Checkin";
import uuid from "uuid/v4";
import SoftCheckin from "./SoftCheckin";
import ComplexCheckin from "./ComplexCheckin";

export default class CheckinManager {
    checkins: {[id: string]: Checkin };

    constructor() {}

    createSoftCheckin(socketId: string, callback: (id: string) => void ): string {
        const checkId = uuid();
        this.checkins[checkId] =  new SoftCheckin(socketId, callback);
        return checkId;
    }

    createComplexCheckin(socketIds: string[], callback: (ids: string[]) => void, timeout = 0, callbackTimeout: (ids: {id: string, checked: boolean}[]) => void = undefined): string {
        const checkId = uuid();
        const mergedCallback = (ids: {id: string, checked: boolean}[]) => {
            if (callbackTimeout) {
                callbackTimeout(ids);
            }
            this.deleteCheck(checkId);
        };
        this.checkins[checkId] =  new ComplexCheckin(checkId, socketIds, callback, timeout, mergedCallback);
        return checkId;
    }

    check(checkId: string, socketId: string) {
        if (this.checkins[checkId]) {
           const checkFinished = this.checkins[checkId].check(socketId);
           if (checkFinished) {
            this.deleteCheck(checkId);
           }
        } else {
            console.log("Try to acced to non existing check !");
        }
    }

    deleteCheck(checkId: string) {
        delete this.checkins[checkId];
    }
  }