import SocketIo, { Socket } from "socket.io";
import SocketHandler from "./SocketHandler/SocketHandler";
import State from "./BusinessClasses/State";
import GSocket from "./BusinessClasses/GSocket";
const server = require("http").createServer();
const io = SocketIo(server, { origins: "*:*"});
const socketHandlers: { [name: string]: SocketHandler } = {};
const state = new State();

io.on("connection", function(socket: Socket) {
  console.log("Someone connected");
  socketHandlers[socket.id] = new SocketHandler(<GSocket>socket, state);
  socket.on("disconnect", () => console.log("disconnected"));
});

server.listen(3000);

export default server;
