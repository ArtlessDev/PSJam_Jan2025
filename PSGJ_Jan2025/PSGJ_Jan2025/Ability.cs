using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    public class Ability
    {
        string abilityName;
        int baseDamagePower;
        Element moveType;


        public Ability()
        {
            abilityName = "pound";
            baseDamagePower = 10;
            moveType = Element.Physical;
        }
        public Ability(CustomGameUI button)
        {
            abilityName = button.MoveName;
            baseDamagePower = 40;
            moveType = Element.Physical;
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
        public Element MoveElement
        {
            get { return moveType; }
            set { value = moveType; }
            
        }
    }

    public enum Element
    {
        None,
        Physical,
        Universal,
        Ice,
        Fire,
        Electric,
    }
}
