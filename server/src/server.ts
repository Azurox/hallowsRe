import SocketIo, { Socket } from "socket.io";
import SocketHandler from "./SocketHandler/SocketHandler";
import State from "./BusinessClasses/State";
import GSocket from "./BusinessClasses/GSocket";
import Account, { IAccount } from "./Schema/Account";
import Player from "./Schema/Player";
const server = require("http").createServer();
const mongoose = require("mongoose");
const io = SocketIo(server, { origins: "*:*" });
const socketHandlers: { [name: string]: SocketHandler } = {};
const state = new State();

/** DB */
// mongoose.connect("mongodb://localhost/test");

/** Socket.io */
io.on("connection", function(socket: Socket) {
  console.log("Someone connected");
  socketHandlers[socket.id] = new SocketHandler(<GSocket>socket, state);
  socket.on("disconnect", () => console.log("disconnected"));
});

server.listen(3000);

export default server;
