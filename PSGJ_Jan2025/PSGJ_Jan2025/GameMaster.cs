using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    public static class GameMaster
    {
        public static GamePhases CurrentPhase = GamePhases.Start;
        public static GamePhases PreviousPhase = GamePhases.Start;
        public static bool AbleToChangePhases = true, GenerateAbilities = true;
        public static List<NPC> enemyWave = new List<NPC>();
        public static Task task;
        internal static int WaveNumber = 1;
        public static ContentManager CustomContent;
        public static Font thisFont = new();
        public static int selectedZone, selectedMove;
        public static CustomGameUI[] moveArr;
        public static List<Ability> newAbilities = new List<Ability>();


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
                    SelectAbility(mouseState, actions, mouseRect);
                    break;
                case GamePhases.PlayerTurn:
                    PlayerTurn(actions, mouseRect, mouseState);
                    break;
                case GamePhases.SelectZone:
                    SelectZone(actions, zones, mouseRect, mouseState, zilla);
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

        public static void SelectAbility(MouseStateExtended mouseState, List<CustomGameUI> actions, Rectangle mouseRect)
        {
            //thisFont.FontText = "Select 1 of the 4 abilities to add them to your move set!      [Mouse click to proceed]";

            if(GenerateAbilities)
            {
                newAbilities.Add(new Ability());
                newAbilities.Add(new Ability());
                newAbilities.Add(new Ability());
                newAbilities.Add(new Ability());

                moveArr = actions.ToArray();
                
                GenerateAbilities = false;
            }

            thisFont.FontText = $"Select a move [Q. {newAbilities[0].AbilityName}]    [W. {newAbilities[1].AbilityName}]    [E. {newAbilities[2].AbilityName}]    [R. {newAbilities[3].AbilityName}]";

            KeyboardState keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                moveArr[0].ButtonAbility = newAbilities[0];
                actions[0].ButtonAbility = newAbilities[0];
                actions[0].ButtonAbility.BaseDamagePower = (float)(WaveNumber * .75f) * 30f;
                GenerateAbilities = true;
                newAbilities.Clear();
                CurrentPhase = GamePhases.PlayerTurn;
                task = ResetPhaseChangeFlag();
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                moveArr[1].ButtonAbility = newAbilities[1];
                actions[1].ButtonAbility = newAbilities[1];
                actions[1].ButtonAbility.BaseDamagePower = (float)(WaveNumber * .75f) * 30f;
                GenerateAbilities = true;
                newAbilities.Clear();
                CurrentPhase = GamePhases.PlayerTurn;
                task = ResetPhaseChangeFlag();
            }
            if (keyboardState.IsKeyDown(Keys.E))
            {
                moveArr[2].ButtonAbility = newAbilities[2];
                actions[2].ButtonAbility = newAbilities[2];
                actions[2].ButtonAbility.BaseDamagePower = (float)(WaveNumber * .75f) * 30f;
                GenerateAbilities = true;
                newAbilities.Clear();
                CurrentPhase = GamePhases.PlayerTurn;
                task = ResetPhaseChangeFlag();
            }
            if (keyboardState.IsKeyDown(Keys.R))
            {
                moveArr[3].ButtonAbility = newAbilities[3];
                actions[3].ButtonAbility = newAbilities[3];
                actions[3].ButtonAbility.BaseDamagePower = (float)(WaveNumber * .75f) * 30f;
                GenerateAbilities = true;
                newAbilities.Clear();
                CurrentPhase = GamePhases.PlayerTurn;
                task = ResetPhaseChangeFlag();
            }
        }

        public static void PlayerTurn(List<CustomGameUI> actions, Rectangle mouseRect, MouseStateExtended mouseState)
        {
            thisFont.FontText = "select a learned move";

            foreach (CustomGameUI action in actions)
            {

                if (mouseRect.Intersects(action.Rect) && mouseState.WasButtonPressed(MouseButton.Left))
                {

                    //moveArr = actions.ToArray();
                    selectedMove = Array.IndexOf(moveArr, action);
                    CurrentPhase = GamePhases.SelectZone;
                    thisFont.FontText = "you have selected a move";
                    task = ResetPhaseChangeFlag();
                }
            }
        }
        public static void SelectZone(List<CustomGameUI> actions, CustomGameUI[] zones, Rectangle mouseRect, MouseStateExtended mouseState, Character zilla)
        {
            thisFont.FontText = "select a zone for your attack";


            foreach (CustomGameUI zone in zones)
            {
                if (mouseRect.Intersects(zone.Rect) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    selectedZone = Array.IndexOf(zones, zone);
                    CurrentPhase = GamePhases.EnemyTurn;
                    thisFont.FontText = "attacking. . . ";
                    PlayerTurnExecution(zones, zilla, actions);
                    task = ResetPhaseChangeFlag();
                }
            }

        }

        private async static void PlayerTurnExecution(CustomGameUI[] zones, Character zilla, List<CustomGameUI> actions)
        {
            Debug.WriteLine("running simulation");
            foreach (var enemy in enemyWave.ToList())
            {
                if (enemy.Rect.Intersects(zones[selectedZone].Rect))
                {
                    Ability selectedAbility = actions[selectedMove].ButtonAbility;
                    int rangeModifier = Random.Shared.Next(-WaveNumber*5, WaveNumber*3);
                    if (enemy.IsWeakTo(selectedAbility))
                    {
                        enemy.CurrentHealth -= (int)((zilla.Attack * selectedAbility.BaseDamagePower * WaveNumber) / 75) + rangeModifier;
                        Debug.WriteLine("weak to move: " + enemy.CurrentHealth);

                    }
                    //else if (enemy.IsResistantTo(selectedAbility))
                    //{
                    //    enemy.CurrentHealth -= (int)((zilla.Attack * selectedAbility.BaseDamagePower * WaveNumber) / 175) + rangeModifier;
                    //    Debug.WriteLine("resistant to move: " + enemy.CurrentHealth);
                    //}
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
            foreach (var enemy in enemyWave.ToList())
            {
                enemy.chooseAction(zilla);
                if (enemy.Zone <= 0)
                {
                    zilla.CurrentHealth -= 5;
                    enemyWave.Remove(enemy);
                }
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
                if (zilla.CurrentHealth > 0 && enemyWave.Count > 0) 
                {
                    //
                    if(zilla.CurrentHealth < zilla.MaximumHealth)
                        zilla.CurrentHealth += (zilla.MaximumHealth / 10);
                    task = ResetPhaseChangeFlag();
                    CurrentPhase = GamePhases.PlayerTurn;
                }
                else if (zilla.CurrentHealth > 0 && enemyWave.Count <= 0) 
                {
                    //
                    zilla.MaximumHealth += (zilla.MaximumHealth / 10);
                    zilla.CurrentHealth += (zilla.MaximumHealth / 5);
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
