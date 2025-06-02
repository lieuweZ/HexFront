const MessageHandler = require('./messageHandler.js');
class DamageHandler extends MessageHandler {

	constructor(io, databaseConnector, rooms) {
		super(io, databaseConnector, rooms);
	}

	handleIncomingMessages(socket) {
		this.#handleDamageMessages(socket);
	}

	#handleDamageMessages(socket) {
        socket.on("object to be damaged found", (data) =>
        {
			if (this._rooms[data.roomId])
			{
				console.log(data.attackDamage);
			this._io.to(data.roomId).emit("object to be damaged found", { roomId: data.roomId, targetPosition: data.targetPosition, attackDamage: data.attackDamage});
			}
        });
    }
}

module.exports = DamageHandler;