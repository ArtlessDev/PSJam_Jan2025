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
        public static GamePhases CurrentPhase = 0;
        public static bool AbleToChangePhases = true;
        static List<NPC> enemyWave = new List<NPC>();

        public static async void ChangePhase()
        {
            if (CurrentPhase == GamePhases.Resolution)
            {
                CurrentPhase = 0;
            }
            else 
            {
                switch (CurrentPhase)
                {
                    case GamePhases.EnemySpawn:
                        Debug.WriteLine(GamePhases.EnemySpawn);
                        await SpawnEnemies();
                        break;
                    case GamePhases.PlayerTurn:
                        Debug.WriteLine(GamePhases.PlayerTurn);
                        await PlayerTurn();
                        break;
                    case GamePhases.EnemyTurn:
                        Debug.WriteLine(GamePhases.EnemyTurn);
                        await EnemyTurn();
                        break;
                    case GamePhases.Resolution:
                        Debug.WriteLine(GamePhases.Resolution);
                        await ResolveTurn();
                        break;
                }

                CurrentPhase++;
            }

            AbleToChangePhases = false;
        }

        public static void ResetPhaseChangeFlag()
        {
            AbleToChangePhases = true;
        }

        public static async Task<Task> SpawnEnemies()
        {
            return Task.Factory.StartNew(() =>
            {
                int numOfEnemies = Random.Shared.Next(20, 30);

                for (int i = 0; i < numOfEnemies; i++)
                {
                    enemyWave.Add(new NPC());
                }



                Debug.WriteLine($"spawned {numOfEnemies} enemies");
                Thread.Sleep(2000);
            });
        }
        public static async Task<Task> PlayerTurn()
        {
            return Task.Factory.StartNew(() =>
            {
                Debug.WriteLine("In phase player turn");
                Thread.Sleep(2000);
            });
        }
        public static async Task<Task> EnemyTurn()
        {
            return Task.Factory.StartNew(() =>
            {
                Debug.WriteLine("In phase enemy turn");
                Thread.Sleep(2000);
            });
        }
        public static async Task<Task> ResolveTurn()
        {
            return Task.Factory.StartNew(() =>
            {
                Debug.WriteLine("In phase resolve turn");
                Thread.Sleep(2000);
            });
        }


    }
    public enum GamePhases
    {
        Start,
        EnemySpawn,
        PlayerTurn,
        EnemyTurn,
        Resolution,
    }
}
