using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public static int selectedZone, selectedMove;
        public static CustomGameUI[] moveArr;

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
                    SelectZone(zones, mouseRect, mouseState, zilla);
                    break;
                case GamePhases.EnemyTurn:
                    EnemyTurn(mouseRect, mouseState, zilla);
                    break;
                case GamePhases.ResolveTurn:
                    ResolveTurn(mouseRect, mouseState, zilla);
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
                    ability.MoveName = "pound";
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

                    moveArr = actions.ToArray();
                    selectedMove = Array.IndexOf(moveArr, action);
                    CurrentPhase = GamePhases.SelectZone;
                    thisFont.FontText = "you have selected a move";
                    task = ResetPhaseChangeFlag();
                }
            }
        }
        public static void SelectZone(CustomGameUI[] zones, Rectangle mouseRect, MouseStateExtended mouseState, Character zilla)
        {
            thisFont.FontText = "select a zone for your attack";


            foreach (CustomGameUI zone in zones)
            {
                if (mouseRect.Intersects(zone.Rect) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    selectedZone = Array.IndexOf(zones, zone);
                    CurrentPhase = GamePhases.EnemyTurn;
                    thisFont.FontText = "attacking. . . ";
                    PlayerTurnExecution(zones, zilla);
                    task = ResetPhaseChangeFlag();
                }
            }

        }

        private async static void PlayerTurnExecution(CustomGameUI[] zones, Character zilla)
        {
            Debug.WriteLine("running simulation");
            foreach (var enemy in enemyWave.ToList())
            {
                if (enemy.Rect.Intersects(zones[selectedZone].Rect))
                {
                    Ability selectedAbility = new Ability(moveArr[selectedMove]);
                    int rangeModifier = Random.Shared.Next(-WaveNumber*5, WaveNumber*3);
                    if (enemy.IsWeakTo(selectedAbility))
                    {
                        enemy.CurrentHealth -= (int)((zilla.Attack * selectedAbility.BaseDamagePower * WaveNumber) / 100) + rangeModifier;
                        Debug.WriteLine("weak to move: " + enemy.CurrentHealth);

                    }
                    else if (enemy.IsResistantTo(selectedAbility))
                    {
                        enemy.CurrentHealth -= (int)((zilla.Attack * selectedAbility.BaseDamagePower * WaveNumber) / 175) + rangeModifier;
                        Debug.WriteLine("resistant to move: " + enemy.CurrentHealth);

                    }
                    else 
                    {
                        enemy.CurrentHealth -= (int)((zilla.Attack * selectedAbility.BaseDamagePower * WaveNumber) / 150) + rangeModifier;
                        Debug.WriteLine("not weak to move: " + enemy.CurrentHealth);
                    }
                    await Task.Delay(100);
                    enemy.CanMoveZones = true;
                }

                if (enemy.CurrentHealth <= 0)
                {
                    enemy.TextureColor = Color.Red;
                    enemyWave.Remove(enemy);
                }
            }
            await Task.Delay(1000);
        }

        public static async void EnemyTurn(Rectangle mouseRect, MouseStateExtended mouseState, Character zilla)
        {

            thisFont.FontText = "In phase enemy turn";
            foreach (var enemy in enemyWave)
            {
                enemy.chooseAction(zilla);
            }
            task = ResetPhaseChangeFlag();
            CurrentPhase = GamePhases.ResolveTurn;
            thisFont.FontText = $"Your health: {zilla.CurrentHealth}...     [Mouse click to proceed]";

            await Task.Delay(1000);

        }

        public static void ResolveTurn(Rectangle mouseRect, MouseStateExtended mouseState, Character zilla)
        {

            if(mouseState.WasButtonPressed(MouseButton.Left))
            {
                if (zilla.CurrentHealth > 0 && enemyWave.Count > 0) //&& mouseRect.Intersects(this.Rect))
                {
                    //
                    if(zilla.CurrentHealth < zilla.MaximumHealth)
                        zilla.CurrentHealth += (zilla.MaximumHealth / 10);
                    task = ResetPhaseChangeFlag();
                    CurrentPhase = GamePhases.PlayerTurn;
                }
                else if (zilla.CurrentHealth > 0 && enemyWave.Count <= 0) //&& mouseRect.Intersects(this.Rect))
                {
                    //
                    zilla.MaximumHealth += (zilla.MaximumHealth / 10);
                    zilla.CurrentHealth += (zilla.MaximumHealth / 8);
                    task = ResetPhaseChangeFlag();
                    CurrentPhase = GamePhases.EnemySpawn;
                }
                else if (zilla.CurrentHealth <= 0)
                {
                    //game over
                    zilla.CurrentHealth += 20;
                    CurrentPhase = GamePhases.GameOver;
                    thisFont.FontText = $"game over . . . you made it to wave {WaveNumber}";
                }
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
