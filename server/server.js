const express = require("express");
const http = require("http");
const morgan = require("morgan");
const { Server } = require("socket.io");

const app = express();
app.use(morgan("tiny"));

app.get("/", (_req, res) => res.send("HexFront server is running"));

const server = http.createServer(app);

const io = new Server(server, {
  path: "/socket.io",
  cors: { origin: "*" }
});

const rooms = new Map();
const sessions = new Map();

function makeId(prefix) {
  return prefix + "_" + Math.random().toString(36).slice(2, 10);
}

io.on("connection", (socket) => {
  const incomingSessionId = socket.handshake.query.sessionId;
  let sessionId = incomingSessionId && sessions.has(incomingSessionId)
      ? incomingSessionId
      : makeId("sess");
  let userId = sessions.get(sessionId) || makeId("user");
  sessions.set(sessionId, userId);

  socket.data.sessionId = sessionId;
  socket.data.userId = userId;

  socket.emit("sessionData", { sessionId, userId });

  const relayEvents = [
    "movePiece",
    "cellChange",
    "cellDelete",
    "cellInsert",
    "cellUpdate",
    "cellType",
    "cellEffects",
    "damage",
    "turnChange",
    "gameOver",
    "chatMessage",
    "achievementUnlocked"
  ];

  for (const ev of relayEvents) {
    socket.on(ev, (payload) => {
      const roomId = socket.data.roomId;
      if (!roomId) return;
      socket.to(roomId).emit(ev, payload);
    });
  }

  socket.on("joinRoom", ({ roomId }) => {
    const id = roomId || makeId("room");
    socket.join(id);
    socket.data.roomId = id;
    if (!rooms.has(id)) rooms.set(id, new Set());
    rooms.get(id).add(socket.id);
    socket.emit("roomJoined", { roomId: id });
  });

  socket.on("leaveRoom", () => {
    const id = socket.data.roomId;
    if (id) {
      socket.leave(id);
      rooms.get(id)?.delete(socket.id);
    }
    socket.data.roomId = null;
  });

  socket.on("disconnect", () => {
    const id = socket.data.roomId;
    if (id) rooms.get(id)?.delete(socket.id);
  });
});

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => console.log(`HexFront server listening on ${PORT}`));