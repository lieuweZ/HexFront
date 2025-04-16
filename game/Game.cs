using System;
using Blok3Game.Engine.AssetHandler;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;

namespace BaseProject
{
    public class Game : GameEnvironment
    {      
        protected override void LoadContent()
        {
            base.LoadContent();
			
            screen = new Point(800, 600);
            ApplyResolutionSettings();
			
			AssetManager.AudioManager.PlaySong("main_menu", true);

            GameStateManager.AddGameState(GameStateManager.LOBBY_JOIN_OR_CREATE_STATE, new LobbyJoinOrCreateState());
            GameStateManager.AddGameState(GameStateManager.LOBBY_CREATE_GAME_STATE, new LobbyCreateGameState());
            GameStateManager.AddGameState(GameStateManager.LOBBY_JOIN_GAME_STATE, new LobbyJoinGameState());
            GameStateManager.AddGameState(GameStateManager.LOBBY_WAIT_FOR_PLAYERS_STATE, new LobbyWaitForPlayersState());
            GameStateManager.AddGameState(GameStateManager.GAME_STATE, new GameState());
            GameStateManager.SwitchTo(GameStateManager.GAME_STATE);
        }

		protected override void OnExiting(object sender, EventArgs args)
		{
			base.OnExiting(sender, args);

			SocketClient.Instance.SendDataPacket(new LeaveRoomData());
		}
	}
}
