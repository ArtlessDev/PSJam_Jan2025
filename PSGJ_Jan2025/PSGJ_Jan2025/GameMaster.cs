using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    public static class GameMaster
    {
        public static GamePhases CurrentPhase = GamePhases.Start;
        public static GamePhases PreviousPhase = GamePhases.Start;
        public static bool AbleToChangePhases = true;
        static List<NPC> enemyWave = new List<NPC>();
        public static Task task;

        public static void ChangePhase(List<CustomGameUI> actions, Rectangle mouseRect, MouseStateExtended mouseState)
        {
            if (CurrentPhase == GamePhases.Start)
            {
                StartState(mouseState);
            }
            switch (CurrentPhase)
            {
                case GamePhases.EnemySpawn:
                    SpawnEnemies(mouseState);
                    break;
                case GamePhases.SelectAbility:
                    SelectAbility(mouseState);
                    break;
                case GamePhases.PlayerTurn:
                    PlayerTurn(actions, mouseRect, mouseState);
                    break;
                case GamePhases.EnemyTurn:
                    EnemyTurn(mouseRect, mouseState);
                    break;
                case GamePhases.ResolveTurn:
                    ResolveTurn(mouseRect, mouseState);
                    break;
            }
        }

        public static async Task ResetPhaseChangeFlag()
        {
            await Task.Delay(2000);
            AbleToChangePhases = true;
        }

        public static void StartState(MouseStateExtended mouseState)
        {
            Debug.WriteLine("In the Start State");

            if (mouseState.WasButtonPressed(MouseButton.Left))
            {
                CurrentPhase = GamePhases.EnemySpawn;
                task = ResetPhaseChangeFlag();
            }
        }

        public static void SpawnEnemies(MouseStateExtended mouseState)
        {
            if (mouseState.WasButtonPressed(MouseButton.Left))
            {
                //this section spawns enemies for the wave
                int numOfEnemies = Random.Shared.Next(20, 30);
                for (int i = 0; i < numOfEnemies; i++)
                {
                    enemyWave.Add(new NPC());
                }
                Debug.WriteLine($"spawned {numOfEnemies} enemies");

                CurrentPhase = GamePhases.SelectAbility;
                task = ResetPhaseChangeFlag();
            }
        }

        public static void SelectAbility(MouseStateExtended mouseState)
        {
            if (mouseState.WasButtonPressed(MouseButton.Left))
            {
                // this section will allow the player to choose 2 new abilities
                Debug.WriteLine("In phase select ability");

                CurrentPhase = GamePhases.PlayerTurn;
                task = ResetPhaseChangeFlag();
            }
        }
        
        public static void PlayerTurn(List<CustomGameUI> actions, Rectangle mouseRect, MouseStateExtended mouseState)
        {
            Debug.WriteLine("In phase player turn");

            foreach (CustomGameUI action in actions)
            {
                if (mouseRect.Intersects(action.Rect) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    CurrentPhase = GamePhases.EnemyTurn;
                    Debug.WriteLine("Run the selected move");
                    task = ResetPhaseChangeFlag();
                }
            }
        }

        public static void EnemyTurn(Rectangle mouseRect, MouseStateExtended mouseState)
        {
            Debug.WriteLine("In phase enemy turn");

            if (mouseState.WasButtonPressed(MouseButton.Left)) //&& mouseRect.Intersects(this.Rect))
            {
                task = ResetPhaseChangeFlag();
                CurrentPhase = GamePhases.ResolveTurn;
            }
        }

        public static void ResolveTurn(Rectangle mouseRect, MouseStateExtended mouseState)
        {
            AbleToChangePhases = false;
            PreviousPhase = CurrentPhase;

            Debug.WriteLine("In phase resolve turn");

            ///if player health is 0
            ///     game over
            ///if player health is above 0 and wave done
            ///     go to spawn enemies state
            ///if player health is above 0 and wave not done
            ///     go to player turn
            if (mouseState.WasButtonPressed(MouseButton.Left)) //&& mouseRect.Intersects(this.Rect))
            {
                task = ResetPhaseChangeFlag();
                CurrentPhase = GamePhases.PlayerTurn;
            }
        }
    }

    public enum GamePhases
    {
        Start,
        EnemySpawn,
        SelectAbility, //new
        PlayerTurn,
        EnemyTurn,
        ResolveTurn,
        GameOver,
    }
}
