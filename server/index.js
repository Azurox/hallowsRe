import SocketHandler from "./SocketHandler/SocketHandler";
var server = require('http').createServer();
var io = require('socket.io')(server);
var socketHandlers = {};
var state = {};


state.worldMap = [
  [{"name": "0-0.json"}, {"name": "0-0.json"}],
  [{"name": "0-0.json"}, {"name": "0-0.json"}]
];

io.on('connection', function(socket){
  socketHandlers[socket.id] = new SocketHandler(socket, state);
  socket.on('event', function(data){});
  socket.on('disconnect', function(){});
});

server.listen(3000);