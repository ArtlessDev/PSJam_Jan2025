using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    public static class GameMaster
    {
        public static int CurrentPhase = 0;
        public static bool AbleToChangePhases = true;

        public static void ChangePhase()
        {
            if (CurrentPhase == 4)
            {
                CurrentPhase = 0;
            }
            else 
            {
                CurrentPhase++;
            }

            AbleToChangePhases = false;
        }

        public static void ResetPhaseChangeFlag()
        {
            AbleToChangePhases = true;
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
}
