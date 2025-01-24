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
        public static Font thisFont = new();

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
                case GamePhases.SelectZone:
                    SelectZone(zones, mouseRect, mouseState);
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
            thisFont.FontText = "in the start state [Mouse click to proceed]";

            if (mouseState.WasButtonPressed(MouseButton.Left))
            {
                thisFont.FontText = "moving out of start state ...";

                CurrentPhase = GamePhases.EnemySpawn;
                task = ResetPhaseChangeFlag();
            }
        }

        //COMPLETE
        public static void SpawnEnemies(MouseStateExtended mouseState)
        {
            thisFont.FontText = "oh no Zilla! there are soliders on their way...        [Mouse click to proceed]";

            if (mouseState.WasButtonPressed(MouseButton.Left))
            {
                //this section spawns enemies for the wave
                int numOfEnemies = Random.Shared.Next(5*WaveNumber, 10*WaveNumber);
                for (int i = 0; i < numOfEnemies; i++)
                {
                    enemyWave.Add(new NPC());
                }
                thisFont.FontText = $"uh oh! {numOfEnemies} enemies spawned!";

                CurrentPhase = GamePhases.SelectAbility;
                WaveNumber++;
                task = ResetPhaseChangeFlag();
            }
        }

        public static void SelectAbility(MouseStateExtended mouseState, CustomGameUI[] abilities, Rectangle mouseRect)
        {
            thisFont.FontText = "Select 1 of the 4 abilities to add them to your move set!      [Mouse click to proceed]";
            foreach (CustomGameUI ability in abilities)
            {
                ability.changeColor(mouseRect);
                if (mouseState.WasButtonPressed(MouseButton.Left))
                {

                    CurrentPhase = GamePhases.PlayerTurn;
                    task = ResetPhaseChangeFlag();
                }
            }
        }

        public static void PlayerTurn(List<CustomGameUI> actions, Rectangle mouseRect, MouseStateExtended mouseState)
        {
            thisFont.FontText = "select a learned move";

            foreach (CustomGameUI action in actions)
            {

                if (mouseRect.Intersects(action.Rect) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    CurrentPhase = GamePhases.SelectZone;
                    thisFont.FontText = "you have selected a move";
                    task = ResetPhaseChangeFlag();
                }
            }
        }
        public static void SelectZone(CustomGameUI[] zones, Rectangle mouseRect, MouseStateExtended mouseState)
        {
            thisFont.FontText = "select a zone for your attack";


            foreach (CustomGameUI zone in zones)
            {

                if (mouseRect.Intersects(zone.Rect) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    CurrentPhase = GamePhases.EnemyTurn;
                    thisFont.FontText = "you have selected a zone";
                    task = ResetPhaseChangeFlag();
                }
            }

        }


        public static void EnemyTurn(Rectangle mouseRect, MouseStateExtended mouseState, Character zilla)
        {

            if (mouseState.WasButtonPressed(MouseButton.Left)) 
            {
                thisFont.FontText = "In phase enemy turn";
                foreach (var enemy in enemyWave)
                {
                    enemy.chooseAction(zilla);
                }
                task = ResetPhaseChangeFlag();
                CurrentPhase = GamePhases.ResolveTurn;
                thisFont.FontText = $"Your health: {zilla.Health}";
            }
        }

        public static void ResolveTurn(Rectangle mouseRect, MouseStateExtended mouseState, Character zilla)
        {
            thisFont.FontText = $"Resolving Turn";

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
