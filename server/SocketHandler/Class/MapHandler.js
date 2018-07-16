// @flow
export default class MapHandler {
    socket: any;
    W: any;

    constructor(socket: any, state: any){
        this.socket = socket;
        this.W = state;
        this.initSocket(socket);
    }

    initSocket(){
        this.socket.on('initWorld', this.spawnPlayer);
        this.socket.on('move', ()=>{});
    }

    spawnPlayer(){
        console.log('spawn player');
        this.socket.emit('loadMap', {
            name: this.W.worldMap[0][0].name
        })
    }
};