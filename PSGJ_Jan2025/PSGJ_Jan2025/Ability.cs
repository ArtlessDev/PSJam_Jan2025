using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    internal class Ability
    {
        string abilityName;
        int baseDamagePower;
        MoveType moveType;
        

        public Ability() 
        {
            abilityName = "pound";
            baseDamagePower = 10;
            moveType = MoveType.Physical;
        }

        public int BaseDamagePower
        {
            get { return baseDamagePower; }
            set { baseDamagePower = value; }
        }
        public string AbilityName
        {
            get { return abilityName; }
            set { abilityName = value; }
        }
    }

    enum MoveType
    {
        None,
        Physical,
        Universal,
        Ice,
        Fire,
        Electric,
    }
}
