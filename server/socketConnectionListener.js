const MySqlDatabase = require("./framework/utils/mySqlDatabase");
const GameMessageHandler = require("./api/gameMessageHandler");
const RoomMessageHandler = require("./api/roomMessageHandler");

const DatabaseHandler = require("./api/databaseHandler");

class SocketConnectionListener {
	#databaseConnector;
	#roomMessageHandler;
	#gameMessageHandler;
	#DatabaseHandler = null;
	
	#io = null;
	#rooms = { };
	#cryptoHelper = require("./framework/utils/cryptoHelper");
	#sessionStore = require("./framework/utils/sessionStore");

	#getRandomId = () => this.#cryptoHelper.generateHexId().toString("hex");

	initializeServer(server) {
		this.#databaseConnector = new MySqlDatabase();
		this.#io = require("socket.io")(server);
		
		this.#roomMessageHandler = new RoomMessageHandler(this.#io, this.#databaseConnector, this.#rooms);
		this.#gameMessageHandler = new GameMessageHandler(this.#io, this.#databaseConnector, this.#rooms);
		this.#DatabaseHandler = new DatabaseHandler(this.#io, this.#databaseConnector, this.#rooms);

		this.#handleSocketSession();
		this.#handleIncomingConnections();
	}

	exectutePreparedQuery(query, parameters) {
		return this.#databaseConnector.executePreparedQuery(query, parameters);
	}

	#handleSocketSession() {
		this.#io.use((socket, next) => {
			//get the session id from the connection attempt.
			const sessionId = socket.handshake.query.sessionId;

			//if the session id is present, find the session in the session store.
			const session = this.#sessionStore.findSession(sessionId);

			//if the session is found, then the user is reconnected.
			if (session) {
				//restore the session id and user id on the socket.
				socket.sessionId = sessionId;
				socket.userId = session.userId;
				
				//if the session contains a room id, then the user was in a room before the connection was lost.
				if (session.roomId) {
					//restore the room id and player name on the socket.
					socket.roomId = session.roomId;
					socket.playerName = session.playerName;

					//join the room again.
					this.#roomMessageHandler.enterRoom(socket, session.roomId, session.playerName);

					//emit a message to all players in the room that the player has reconnected.
					this.#emitPlayerStateChangeToAllPlayersInRoom(socket, "player reconnected");

					console.log(socket.sessionId, "restored in room", session.roomId);
				} else
				{
					console.log(socket.sessionId, "restored");
				}
			} else {
				//If the session id is not present, create a new one.
				socket.sessionId = this.#getRandomId();
				socket.userId = this.#getRandomId();

				console.log(socket.sessionId, "connected");
			}
			socket.emit("session established", { sessionId: socket.sessionId, userId: socket.userId });
			return next();
		});
	}

	async #handleIncomingConnections() {
		this.#io.on('connection', (socket) => {
			//make sure that this socket can receive id based messages by entering the room.
			socket.join(socket.userId);
			this.#storeSession(socket);
			this.#handleDisconnect(socket);
			
			this.#roomMessageHandler.handleIncomingMessages(socket);
			this.#gameMessageHandler.handleIncomingMessages(socket);
			this.#DatabaseHandler.handleIncomingMessages(socket);

			socket.emit("session established", {
				sessionId: socket.sessionId,
				userId: socket.userId
			});
		});
	}

	#leaveRoomAfterDisconnect(socket, timer) {
		setTimeout(() => {
			//check if the socket is still disconnected.
			if (!socket.connected) {
				//leave the room.
				this.#roomMessageHandler.leaveRoom(socket);
			}
		}, timer);
	}

	#emitPlayerStateChangeToAllPlayersInRoom(socket, state) {
		this.#roomMessageHandler.emitPlayerStateChangeToAllPlayersInRoom(socket, state);
	}

	#handleDisconnect(socket) { 
		socket.on("disconnect", async () => {
			//first emit a message to all players in the room that the player has disconnected.
			this.#emitPlayerStateChangeToAllPlayersInRoom(socket, "player disconnected");

			//wait for 10 seconds to see if the connection is reestablished. If it is not, then leave the room.
			//if the connection is reestablished after this period, the player will be added to the room again.
			//this is done to prevent the room from being removed if the connection is lost for a short period of time.
			this.#leaveRoomAfterDisconnect(socket, 10000);
			console.log(socket.sessionId, "disconnected");

			//store the session, so that it can be restored if the connection is reestablished.
			this.#storeSession(socket);
		});
	}

	#storeSession(socket) {
		this.#sessionStore.saveSession(socket.sessionId, {
			userId: socket.userId,
			roomId: socket.roomId,
			playerName: socket.playerName
		});
	}
}

module.exports = SocketConnectionListener;