using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    public static class GameMaster
    {
        public static GamePhases CurrentPhase = GamePhases.EnemySpawn;
        public static bool AbleToChangePhases = true;
        static List<NPC> enemyWave = new List<NPC>();

        public static async void ChangePhase(List<CustomGameUI> actions, Rectangle mouseRect, MouseState mouseState)
        {
            if (GameMaster.AbleToChangePhases == true)
            {
                switch (GameMaster.CurrentPhase)
                {
                    case GamePhases.EnemySpawn:
                        Debug.WriteLine(GamePhases.EnemySpawn);
                        await GameMaster.SpawnEnemies();
                        break;
                    case GamePhases.SelectAbility:
                        Debug.WriteLine(GamePhases.SelectAbility);
                        await GameMaster.SelectAbility();
                        break;
                    case GamePhases.PlayerTurn:
                        Debug.WriteLine(GamePhases.PlayerTurn);
                        await GameMaster.PlayerTurn(actions, mouseRect, mouseState);
                        break;
                    case GamePhases.EnemyTurn:
                        Debug.WriteLine(GamePhases.EnemyTurn);
                        await GameMaster.EnemyTurn(mouseRect, mouseState);
                        break;
                    case GamePhases.ResolveTurn:
                        Debug.WriteLine(GamePhases.ResolveTurn);
                        await GameMaster.ResolveTurn(mouseRect, mouseState);
                        break;
                        //}
                        //GameMaster.CurrentPhase++;
                }
            }
        }

        public static void ResetPhaseChangeFlag()
        {
            AbleToChangePhases = true;
        }

        public static async Task<Task> SpawnEnemies()
        {
            return Task.Factory.StartNew(() =>
            {

                //this section spawns enemies for the wave
                int numOfEnemies = Random.Shared.Next(20, 30);
                for (int i = 0; i < numOfEnemies; i++)
                {
                    enemyWave.Add(new NPC());
                }
                Debug.WriteLine($"spawned {numOfEnemies} enemies");

                // this section will allow the player to choose 2 new abilities

                GameMaster.CurrentPhase = GamePhases.SelectAbility;
                ResetPhaseChangeFlag();

                Thread.Sleep(2000);
            });
        }
        public static async Task<Task> SelectAbility()
        {
            return Task.Factory.StartNew(() =>
            {
                Debug.WriteLine("In phase player turn");


                ResetPhaseChangeFlag();

                GameMaster.CurrentPhase = GamePhases.PlayerTurn;
                Thread.Sleep(2000);
            });
        }
        
        public static async Task<Task> PlayerTurn(List<CustomGameUI> actions, Rectangle mouseRect, MouseState mouseState)
        {
            return Task.Factory.StartNew(() =>
            {
                AbleToChangePhases = false;

                Debug.WriteLine("In phase player turn");

                //wait for player input to progress
                foreach (CustomGameUI action in actions)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed) //&& mouseRect.Intersects(this.Rect))
                    {
                        GameMaster.CurrentPhase = GamePhases.EnemyTurn;
                        ResetPhaseChangeFlag();
                    }
                }

                //await move selection


                Thread.Sleep(2000);
            });
        }
        public static async Task<Task> EnemyTurn(Rectangle mouseRect, MouseState mouseState)
        {
            return Task.Factory.StartNew(() =>
            {
                AbleToChangePhases = false;
                Debug.WriteLine("In phase enemy turn");

                if (mouseState.LeftButton == ButtonState.Pressed) //&& mouseRect.Intersects(this.Rect))
                {
                        ResetPhaseChangeFlag();
                    GameMaster.CurrentPhase = GamePhases.ResolveTurn;
                }
                Thread.Sleep(2000);
            });
        }
        public static async Task<Task> ResolveTurn(Rectangle mouseRect, MouseState mouseState)
        {
            return Task.Factory.StartNew(() =>
            {
                AbleToChangePhases = false;

                Debug.WriteLine("In phase resolve turn");

                ///if player health is 0
                ///     game over
                ///if player health is above 0 and wave done
                ///     go to spawn enemies state
                ///if player health is above 0 and wave not done
                ///     go to player turn
                if (mouseState.LeftButton == ButtonState.Pressed) //&& mouseRect.Intersects(this.Rect))
                {
                        ResetPhaseChangeFlag();
                    GameMaster.CurrentPhase = GamePhases.PlayerTurn;
                }


                Thread.Sleep(2000);
            });
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
