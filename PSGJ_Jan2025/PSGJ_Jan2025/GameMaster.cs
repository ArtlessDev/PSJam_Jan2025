using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public static List<NPC> enemyWave = new List<NPC>();
        public static Task task;
        internal static int WaveNumber = 1;
        public static ContentManager CustomContent;
        static Font thisFont = new();

        public static void ChangePhase(List<CustomGameUI> actions, Rectangle mouseRect, MouseStateExtended mouseState, Character zilla, CustomGameUI[] zones)
        {

            switch (CurrentPhase)
            {
                case GamePhases.Start:
                    StartState(mouseState);
                    break;
                case GamePhases.EnemySpawn:
                    SpawnEnemies(mouseState);
                    break;
                case GamePhases.SelectAbility:
                    SelectAbility(mouseState, zones, mouseRect);
                    break;
                case GamePhases.PlayerTurn:
                    PlayerTurn(actions, mouseRect, mouseState);
                    break;
                case GamePhases.EnemyTurn:
                    EnemyTurn(mouseRect, mouseState, zilla);
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

            if (mouseState.WasButtonPressed(MouseButton.Left))
            {
                Debug.WriteLine("In the Start State");
                thisFont.FontText = "in the start state";

                CurrentPhase = GamePhases.EnemySpawn;
                task = ResetPhaseChangeFlag();
            }
        }

        public static void SpawnEnemies(MouseStateExtended mouseState)
        {
            if (mouseState.WasButtonPressed(MouseButton.Left))
            {
                //this section spawns enemies for the wave
                int numOfEnemies = Random.Shared.Next(5*WaveNumber, 10*WaveNumber);
                for (int i = 0; i < numOfEnemies; i++)
                {
                    enemyWave.Add(new NPC());
                }
                Debug.WriteLine($"spawned {numOfEnemies} enemies");
                thisFont.FontText = $"spawned {numOfEnemies} enemies!";

                CurrentPhase = GamePhases.SelectAbility;
                WaveNumber++;
                task = ResetPhaseChangeFlag();
            }
        }

        public static void SelectAbility(MouseStateExtended mouseState, CustomGameUI[] zones, Rectangle mouseRect)
        {
            foreach (CustomGameUI zone in zones)
            {
                zone.changeColor(mouseRect);
                if (mouseState.WasButtonPressed(MouseButton.Left))
                {
                    // this section will allow the player to choose 2 new abilities
                    Debug.WriteLine("In phase select ability");
                    thisFont.FontText = "In phase select ability";

                    CurrentPhase = GamePhases.PlayerTurn;
                    task = ResetPhaseChangeFlag();
                }
            }
        }
        
        public static void PlayerTurn(List<CustomGameUI> actions, Rectangle mouseRect, MouseStateExtended mouseState)
        {

            foreach (CustomGameUI action in actions)
            {

                if (mouseRect.Intersects(action.Rect) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    CurrentPhase = GamePhases.EnemyTurn;
                    Debug.WriteLine("Run the selected move");
                    thisFont.FontText = "Run the selected move";
                    task = ResetPhaseChangeFlag();
                }
            }
        }

        public static void EnemyTurn(Rectangle mouseRect, MouseStateExtended mouseState, Character zilla)
        {

            if (mouseState.WasButtonPressed(MouseButton.Left)) //&& mouseRect.Intersects(this.Rect))
            {
                Debug.WriteLine("In phase enemy turn");
                thisFont.FontText = "Run the selected move";
                foreach (var enemy in enemyWave)
                {
                    enemy.chooseAction(zilla);
                }
                task = ResetPhaseChangeFlag();
                CurrentPhase = GamePhases.ResolveTurn;
            }
        }

        public static void ResolveTurn(Rectangle mouseRect, MouseStateExtended mouseState)
        {
            
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
        SelectAbility,
        SelectZone,     //new
        PlayerTurn,
        EnemyTurn,
        ResolveTurn,
        GameOver,
    }
}
