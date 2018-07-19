import MapController from "../../BusinessClasses/MapController";

// @flow
export default class MapHandler {
    socket: any;
    M: MapController;

    constructor(socket: any, state: any){
        this.socket = socket;
        this.M = state.MapController;
        this.initSocket();
    }

    initSocket(){
        this.socket.on('initWorld', this.spawnPlayer);
        this.socket.on('move', this.move);
    }

    async spawnPlayer(){
        console.log('spawn player');
        await this.M.spawnPlayer();
        this.socket.emit('loadMap', {
            name: this.M.worldMap[0][0].name
        })
    }

    async move(){
        await this.M.movePlayer();
    }

};