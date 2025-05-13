const MessageHandler = require("./messageHandler.js");
class GameMessageHandler extends MessageHandler {
  constructor(io, databaseConnector, rooms) {
    super(io, databaseConnector, rooms);
  }

  handleIncomingMessages(socket) {
    this.#handleIncomingStartGameMessages(socket);
  }

  #handleIncomingStartGameMessages(socket) {
    socket.on("start game", (data) => {
      this._rooms[data.roomId].started = true;
      const roomseed = this._rooms[data.roomId].roomseed;

      //send a message to all players that the game has started.
      //the client in the specific room can then start the game.
      //other clients can remove the game from the list of active games.
      this._io.emit("start game", { roomId: data.roomId, roomseed });
    });
  }
}

module.exports = GameMessageHandler;
