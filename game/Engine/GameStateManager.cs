using System.Collections.Generic;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameStateManager : IGameLoopObject
{
    public const string LOBBY_CREATE_GAME_STATE = "Lobby Create Game";
    public const string LOBBY_JOIN_OR_CREATE_STATE = "Lobby Join Or Create";
    public const string LOBBY_WAIT_FOR_PLAYERS_STATE = "Lobby Wait For Players";
    public const string LOBBY_JOIN_GAME_STATE = "Lobby Join Game";
    public const string GAME_STATE = "Game";
    public const string GO_TO_PREVIOUS_SCREEN = "GO TO PREVIOUS SCREEN";

    private readonly Stack<string> previousGameStates = new Stack<string>();
    private string currentStateName = string.Empty;
    Dictionary<string, IGameLoopObject> gameStates;
    IGameLoopObject currentGameState;

    public GameStateManager()
    {
        gameStates = new Dictionary<string, IGameLoopObject>();
        currentGameState = null;
    }

    public void AddGameState(string name, IGameLoopObject state)
    {
        gameStates[name] = state;
    }

    public IGameLoopObject GetGameState(string name)
    {
        return gameStates[name];
    }

    public void SwitchTo(string name)
    {
        if (name == GO_TO_PREVIOUS_SCREEN)
        {
            GoToPreviousScreen();
        }
        else
        {
			SwitchToState(name);
        }
    }

    private void SwitchToState(string name, bool addStateToStack = true)
    {
        if (gameStates.ContainsKey(name))
        {
            if (addStateToStack && currentStateName != string.Empty)
            {
                previousGameStates.Push(currentStateName);
            }
            currentGameState = gameStates[name];
            currentStateName = name;
            currentGameState.Reset();
        }
        else
        {
            throw new KeyNotFoundException("Could not find game state: " + name);
        }
    }

    private void GoToPreviousScreen()
    {
        if (previousGameStates.Count > 0)
        {
            string previousState = previousGameStates.Pop();

            //Do not add the current state to the stack because we are going back to the previous state.
            SwitchToState(previousState, false);
        }
    }

    public IGameLoopObject CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }

    public void HandleInput(InputHelper inputHelper)
    {
        if (currentGameState != null)
        {

            currentGameState.HandleInput(inputHelper);
        }
    }

    public void Update(GameTime gameTime)
    {
        if (currentGameState != null)
        {
            currentGameState.Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentGameState != null)
        {
            currentGameState.Draw(gameTime, spriteBatch);
        }
    }

    public void Reset()
    {
        if (currentGameState != null)
        {
            currentGameState.Reset();
        }
    }
}
