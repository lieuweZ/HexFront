const mySql = require("mysql2/promise");

class MySqlDatabase {

	#connectionPool = null;

	constructor() {
		this.#createConnectionPool();
	}
	
	#createConnectionPool() {
		this.#connectionPool = mySql.createPool({
			host: serverConfig.database.host,
			port: serverConfig.database.port,
			user: serverConfig.database.username,
			password: serverConfig.database.password,
			database: serverConfig.database.database,
			connectionLimit: serverConfig.database.connectionLimit,
			timezone: "+01:00",
			multipleStatements: true
		});
	}

	async executePreparedQuery(query, parameters) {
		const connection = await this.#connectionPool.getConnection();
		try	{
			const [rows, fields] = await connection.execute(query, parameters);
			return {
				rows: rows,
				fields: fields
			};
		} catch (err) {
			console.log(`An error occurred while executing prepared query: ${err.code} (${err.errno}): ${err.message}`);
		} finally {
			connection.release();
		}
	};
}

module.exports = MySqlDatabase;