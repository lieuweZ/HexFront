/**
 * Server application - contains all server config and api endpoints
 *
 * @author Pim Meijer & Jur van Oerle
 */
const corsConfig = require("./framework/utils/corsConfigHelper");
const express = require("express");
const bodyParser = require("body-parser");
const morgan = require("morgan");
const path = require("path");
const SocketConnectionListener = require("./socketConnectionListener");

const app = express();
//front-end as static directory
app.use(express.static(path.join(__dirname, '../src')));

//logger library  - 'short' is basic logging info
app.use(morgan("short"));

//helper libraries for parsing request bodies from json to javascript objects
app.use(bodyParser.urlencoded({extended: false}));
app.use(bodyParser.json());

//CORS config - Cross Origin Requests
app.use(corsConfig);

//------- END ROUTES -------

async function listen(port, callback) {
    const server = app.listen(port, callback);
	const socketConnectionListener = new SocketConnectionListener();

	socketConnectionListener.initializeServer(server);
}

module.exports = {
    listen: listen
};