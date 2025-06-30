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

  async executePreparedQuery(query, parameters = []) {
    const conn = await this.#connectionPool.getConnection();
    try {
      console.log("Acquired DB connection");
      const [rows, fields] = await conn.execute(query, parameters);
      console.log(" Query succeeded:", {
        query,
        parameters,
        affectedRows: rows.affectedRows || rows.length
      });
      return { rows, fields };
    } catch (err) {
      console.error("Query failed:", {
        query,
        parameters,
        code: err.code,
        message: err.message
      });
      throw err;
    } finally {
      conn.release();
      console.log("Released DB connection");
    }
  }

  async beginTransaction() {
    const conn = await this.#connectionPool.getConnection();
    await conn.beginTransaction();
    return conn;
  }

  async commit(conn) {
    await conn.commit();
    conn.release();
  }

  async rollback(conn) {
    await conn.rollback();
    conn.release();
  }
}

module.exports = MySqlDatabase;