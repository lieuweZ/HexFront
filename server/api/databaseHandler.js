const MessageHandler = require('./messageHandler.js');
class DatabaseHandler extends MessageHandler {

	constructor(io, databaseConnector, rooms) {
		super(io, databaseConnector, rooms);
	}

	handleIncomingMessages(socket) {
		this.#handleCellTypeMessages(socket);
	}

	#handleCellTypeMessages(socket) {
		socket.on('cell type', async (data) => {
			const roomId = data.roomId;
			const tile = data.tileID;
			const name = data.playerName;

			this.GetCellData(socket, roomId, tile, name);
		});


		socket.on('cell change', async (data) => {
			this.UpdateCellData(data.tileID, data.property, data.data);
		});

		socket.on('cell insert', async (data) => {

			this.InsertCellData(data.tilename, data.cellcolor, data.passable);
		});

		socket.on('cell delete', async (data) => {
			this.DeleteCellData(data.tileID);
		});
		
		socket.on('cell effects', async (data) => {
			const roomId = data.roomId;
			this.GetCellEffectData(roomId, data.cell, data.celleffect);
		});
	}

	async GetCellData(socket, roomId, cellType, name) {
		if (this._rooms[roomId]) {
			const players = this._rooms[roomId].players;
			
			let data = await this._databaseConnector.executePreparedQuery("SELECT * FROM pb4gdg2425_xooqaadaatuu78_.tile where tileID=" + cellType);
			
			//send a message to all players in the room that a new player has joined.
			//since the socket is now subscribed to the room, it will also receive the message.
			this._io.to(roomId).emit('cell type', {roomId:roomId, username: name, color: data.rows[0].color,passable : (data.rows[0].passable == 1)
				, name: data.rows[0].name, tileID: data.rows[0].tileID

			});
		}
	}

	async UpdateCellData(tiletypeId, property, datavalue) {

		let data = "";

		if(property == 1)
		{
			data = "name";
		}

		if(property == 2)
		{
			data = "color";
			datavalue = parseInt(datavalue);
		}

		if(property == 3)
		{
			data = "passable";
		}
		
		this._databaseConnector.executePreparedQuery("UPDATE `pb4gdg2425_xooqaadaatuu78_`.`tile` SET `" + data + "` = '" + datavalue + "' WHERE (`tileID` = '" + tiletypeId + "')");
	}

	async InsertCellData(name, cellColor, passable) {
			let data2 = await this._databaseConnector.executePreparedQuery("SELECT COUNT(*) as TileTypes FROM pb4gdg2425_xooqaadaatuu78_.tile");
			this._databaseConnector.executePreparedQuery("INSERT INTO `pb4gdg2425_xooqaadaatuu78_`.`tile` (`tileID`, `name`, `color`, `passable`) VALUES ('" + data2.rows[0].TileTypes + "', '" + name + "', '" + cellColor + "', '" + (passable == true ? 1 :0) + "');");
	}

	async DeleteCellData(cellType) {
		this._databaseConnector.executePreparedQuery("DELETE FROM pb4gdg2425_xooqaadaatuu78_.tile where tileID=" + cellType);
	}

	async GetCellEffectData(roomId, cellType, name, celldata) {
		if (this._rooms[roomId]) {			
			let data = await this._databaseConnector.executePreparedQuery
			("SELECT * FROM pb4gdg2425_xooqaadaatuu78_.tileeffects INNER JOIN pb4gdg2425_xooqaadaatuu78_.effect ON pb4gdg2425_xooqaadaatuu78_.effect.ideffect=pb4gdg2425_xooqaadaatuu78_.tileeffects.effect_ideffect where pb4gdg2425_xooqaadaatuu78_.tileeffects.tile_tileID = " + cellType);
			
			//send a message to all players in the room that a new player has joined.
			//since the socket is now subscribed to the room, it will also receive the message.

			celldata = data.rows[0].ideffect + "=";
			celldata += data.rows[0].name + "=";
			celldata += data.rows[0].damage + "=";
			celldata += data.rows[0].stunTurns + "=";
			celldata += data.rows[0].startDelay + "=";
			celldata += data.rows[0].endDelay;

			this._io.to(roomId).emit('cell effects', {roomId:roomId, username: name, cell: cellType, celleffect : celldata});
			let i = 0;
		}
	}

			/*0; */
}

module.exports = DatabaseHandler;