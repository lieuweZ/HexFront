class MessageHandler {
	_io;
	_databaseConnector;

	constructor(io, databaseConnector, rooms) {
		this._io = io;
		this._databaseConnector = databaseConnector;
		this._rooms = rooms;
	}

	handleIncomingMessages(socket) {
		throw new Error("Method 'handleIncomingMessages()' must be implemented.");
	}

	_getPlayerById(room, playerId) {
		if (room) {
			const index = Object.values(room.players).findIndex(player => player.userId === playerId);
			if (index !== -1) {
				return room.players[index];
			}
		}
	}
}

module.exports = MessageHandler;