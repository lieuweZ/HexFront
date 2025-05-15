const { generateHexId } = require("../framework/utils/cryptoHelper.js");
const MessageHandler = require("./messageHandler.js");
class RoomMessageHandler extends MessageHandler {
  constructor(io, databaseConnector, rooms) {
    super(io, databaseConnector, rooms);
  }

  handleIncomingMessages(socket) {
    this.#handleIncomingCreateRoomMessages(socket);
    this.#handleIncomingEnterRoomMessages(socket);
    this.#handleIncomingLeaveRoomMessages(socket);
    this.#handleIncomingGetActiveRoomsMessages(socket);
    this.#handleIncomingGetActivePlayersInRoomMessages(socket);

    this.#handleIncomingCellUpdateMessages(socket);
    this.#handleIncomingChatMessages(socket);
    this.#handleIncomingTurnChanges(socket);
  }

  emitPlayerStateChangeToAllPlayersInRoom(socket, message) {
    const roomId = socket.roomId;
    const userId = socket.userId;

    const room = this._rooms[roomId];

    if (room) {
      const player = this._getPlayerById(room, userId);
      this._io.to(roomId).emit(message, { player });
    }
  }

  #handleIncomingCreateRoomMessages(socket) {
    socket.on("create room", (data) => {
      //create a new room and store it in the rooms object.
      const roomId = data.roomId;
      const multiplier = 1e6;
      const seed = Math.floor(Math.random() * multiplier);

      //check if the room already exists.
      //if it does, then send an error message to the client.
      if (this._rooms[roomId]) {
        socket.emit("handle error", { reason: "room already exists" });
        return;
      }

      //Add an empty player array to the room, so that players can be added later.
      this._rooms[roomId] = {
        author: socket.userId,
        players: [],
        roomseed: seed,
      };

      //Send a message to all connected sockets that a new room has been created.
      //The client can then join the room by sending a message to the server.
      //UserId is needed to make sure that the client that created the room can join it.
      this._io.emit("create room", { roomId, author: socket.userId, seed });
    });
  }

  #handleIncomingGetActiveRoomsMessages(socket) {
    socket.on("active rooms", () => {
      const rooms = Object.keys(this._rooms)
        .filter((element) => {
          return !this._rooms[element].started;
        })
        .map((key) => {
          return { roomId: key, players: this._rooms[key].players };
        });
      socket.emit("active rooms", { rooms: rooms });
    });
  }

  #handleIncomingGetActivePlayersInRoomMessages(socket) {
    socket.on("room active players", () => {
      const room = this._rooms[socket.roomId];
      if (room) {
        const players = room.players;
        socket.emit("room active players", { players });
      }
    });
  }

  #handleIncomingEnterRoomMessages(socket) {
    socket.on("enter room", (data) => {
      const roomId = data.roomId;
      const playerName = data.player.name;

      this.enterRoom(socket, roomId, playerName);
    });
  }

  #handleIncomingCellUpdateMessages(socket) {
    socket.on("cell update", (data) => {
      const roomId = data.roomId;
      const pos = data.cell;
      const piecetype = data.piece;
      const name = data.playerName;

      this.UpdateCell(socket, roomId, pos, piecetype, name);
    });
  }

  #handleIncomingChatMessages(socket) {
    socket.on("chat msg", (data) => {
      const roomId = data.roomId;

      this.SendChatMessage(socket, roomId, data.message, data.sender);
    });
  }

  SendChatMessage(socket, roomId, message, name) {
    if (this._rooms[roomId]) {
      const players = this._rooms[roomId].players;

      /*for (let i = 0; i < players.length; i++) {
				
				socket.emit('chat msg', {roomId:roomId, sender: name, message: message});
			}*/
      //send a message to all players in the room that a new player has joined.
      //since the socket is now subscribed to the room, it will also receive the message.
      this._io
        .to(roomId)
        .emit("chat msg", { roomId: roomId, sender: name, message: message });

      //store the room id and player name on the socket so that it can be restored if the connection is lost.
      //see the #handleDisconnect function.
      //socket.roomId = roomId;
      //socket.playerName = playerName;
    }
  }

  UpdateCell(socket, roomId, position, piecdata, name) {
    if (this._rooms[roomId]) {
      const players = this._rooms[roomId].players;

      if (this._rooms[roomId].currentTurnPlayer !== name) {
        console.log(`Not ${name}'s turn!`);
        return;
      }

      //send a message to all players in the room that a new player has joined.
      //since the socket is now subscribed to the room, it will also receive the message.
      this._io.to(roomId).emit("cell update", {
        roomId: roomId,
        cell: position,
        piece: piecdata,
        playerName: name,
      });

      //store the room id and player name on the socket so that it can be restored if the connection is lost.
      //see the #handleDisconnect function.
      //socket.roomId = roomId;
      //socket.playerName = playerName;
    }
    //}
  }

  #handleIncomingTurnChanges(socket) {
    socket.on("turn changed", (data) => {
      const roomId = data.roomId;
      const name = data.playerName;

      this.EndTurn(socket, roomId, name);
    });
  }

  EndTurn(socket, roomId, name) {
    if (!this._rooms[roomId]) return;

    const players = this._rooms[roomId].players;

    if (this._rooms[roomId].currentTurnPlayer !== name) {
      console.log(`Not ${name}'s turn to end!`);
      return;
    }

    const nextPlayer = players.find((player) => player.name !== name);
    this._rooms[roomId].currentTurnPlayer = nextPlayer.name;

    this._io.to(roomId).emit("turn changed", {
      roomId: roomId,
      playerName: nextPlayer.name,
    });

    console.log(`Turn changed: ${JSON.stringify(nextPlayer)}`);
  }

  enterRoom(socket, roomId, playerName) {
    if (this._rooms[roomId]) {
      const players = this._rooms[roomId].players;

      for (let i = 0; i < players.length; i++) {
        const player = players[i];
        if (player.name === playerName) {
          socket.emit("handle error", { reason: "that name is taken" });
          return;
        }
      }
      socket.join(roomId);
      this._rooms[roomId].players.push({
        name: playerName,
        userId: socket.userId,
        role: 0,
      });

      const enterRoomData = {
        player: {
          name: playerName,
          userId: socket.userId,
        },
        roomId: roomId,
      };
      this._io.to(roomId).emit("enter room", enterRoomData);

      socket.roomId = roomId;
      socket.playerName = playerName;

      if (this._rooms[roomId].players.length === 2) {
        this._rooms[roomId].players[0].role = 1;
        this._rooms[roomId].players[1].role = 2;

        this._rooms[roomId].currentTurnPlayer =
          this._rooms[roomId].players[0].name;
        console.log(
          `Starting game in room ${roomId}. First turn: ${this._rooms[roomId].currentTurnPlayer}`
        );

        this._io.to(roomId).emit("turn changed", {
          roomId: roomId,
          playerName: this._rooms[roomId].currentTurnPlayer,
        });
      }
    }
  }

  leaveRoom(socket) {
    const roomId = socket.roomId;
    const userId = socket.userId;

    if (this._rooms[roomId]) {
      const players = this._rooms[roomId].players;
      const index = players.findIndex((player) => player.userId === userId);
      if (index !== -1) {
        //then remove the player from the room and
        players.splice(index, 1);
        socket.leave(roomId);
      }

      //if the room is empty, then remove it from the rooms object.
      if (players.length === 0) {
        delete this._rooms[roomId];
        //let the clients know that the room has been removed.
        this._io.emit("remove room", { roomId });
      }
    }
  }

  #handleIncomingLeaveRoomMessages(socket) {
    socket.on("player leave room", () => {
      if (socket.roomId) {
        //first emit a message to all players in the room that the player has left.
        this.emitPlayerStateChangeToAllPlayersInRoom(
          socket,
          "player leave room"
        );

        //then leave the room.
        this.leaveRoom(socket);
      }
    });
  }
}

module.exports = RoomMessageHandler;
