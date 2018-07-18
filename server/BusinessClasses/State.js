// @flow
export default class State {
    worldMap: any;
    MapController: any;

    constructor(){
        // Load the real world map.
        this.worldMap = [
            [{"name": "0-0.json"}, {"name": "0-0.json"}],
            [{"name": "0-0.json"}, {"name": "0-0.json"}]
        ];
        this.MapController = new MapController();
    }

};