import MapController from './MapController'

export default class State {
    worldMap: Array<Array<any>>;
    MapController: MapController;

    constructor(){
        this.MapController = new MapController();
    }

};