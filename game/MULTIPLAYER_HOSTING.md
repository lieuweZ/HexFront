# Multiplayer Without HvA Server

This client no longer requires the HvA URL. You can point it to your own Socket.IO backend.

## Option 1: Configure in file (recommended for Itch uploads)

Edit `Content/ServerLocation.txt` before publishing:

```json
{
  "debug": {
    "location": "http://localhost:3000",
    "path": ""
  },
  "release": {
    "location": "https://your-server-domain.com",
    "path": "/socket.io"
  }
}
```

## Option 2: Runtime override (local testing / launcher)

Set environment variables before starting the game:

- `HEXFRONT_SERVER_URL` (example: `https://your-server-domain.com`)
- `HEXFRONT_SERVER_PATH` (example: `/socket.io`)

If `HEXFRONT_SERVER_URL` is set, it overrides `ServerLocation.txt`.

## Hosting idea for Itch

Host your Socket.IO server on services like Render, Railway, Fly.io, or your own VPS with HTTPS enabled.  
Then point the game to that URL using one of the options above.
