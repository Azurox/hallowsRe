import MapController from "./MapController";
import PlayerController from "./PlayerController";

export default class State {
    MapController: MapController;
    PlayerController: PlayerController;

    constructor() {
        this.MapController = new MapController(this);
        this.PlayerController = new PlayerController(this);
    }

}