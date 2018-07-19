import MapController from './MapController'

export default class State {
    worldMap: Array<Array<any>>;
    MapController: MapController;

    constructor(){
        // Load the real world map.
        this.worldMap = [
            [{"name": "0-0.json"}, {"name": "0-0.json"}],
            [{"name": "0-0.json"}, {"name": "0-0.json"}]
        ];
        this.MapController = new MapController();
    }

};