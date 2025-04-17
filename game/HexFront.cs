using System;
using Blok3Game.Engine.AssetHandler;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaseProject
{


    public class HexFront : GameEnvironment
    {
        public static HexFront self;
        protected override void LoadContent()
        {
            base.LoadContent();

            self = this;

            screen = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            ApplyResolutionSettings();

            AssetManager.AudioManager.PlaySong("main_menu", true);

            GameStateManager.AddGameState(GameStateManager.LOBBY_JOIN_OR_CREATE_STATE, new LobbyJoinOrCreateState());
            GameStateManager.AddGameState(GameStateManager.LOBBY_CREATE_GAME_STATE, new LobbyCreateGameState());
            GameStateManager.AddGameState(GameStateManager.LOBBY_JOIN_GAME_STATE, new LobbyJoinGameState());
            GameStateManager.AddGameState(GameStateManager.LOBBY_WAIT_FOR_PLAYERS_STATE, new LobbyWaitForPlayersState());
            GameStateManager.AddGameState(GameStateManager.GAME_STATE, new GameState());
            GameStateManager.AddGameState(GameStateManager.MAIN_MENU, new MainMenuState());
            GameStateManager.SwitchTo(GameStateManager.GAME_STATE);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            SocketClient.Instance.SendDataPacket(new LeaveRoomData());
        }

        public void Quit()
        {
            Exit();
        }
    }
}
