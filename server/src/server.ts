import SocketIo, { Socket } from "socket.io";
import SocketHandler from "./SocketHandler/SocketHandler";
import State from "./BusinessClasses/State";
var server = require('http').createServer();
var io = SocketIo(server);
var socketHandlers: { [name: string]: SocketHandler } = {};
var state = new State();

io.on('connection', function(socket : Socket){
  socketHandlers[socket.id] = new SocketHandler(socket, state);
  socket.on('event', function(){});
  socket.on('disconnect', function(){});
});

server.listen(3000);

export default server;
