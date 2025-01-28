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
        int moveMethodId;

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
        public int MoveMethodId
        {
            get { return moveMethodId; }
            set { value = moveMethodId; }
        }

        //will need to add animations
        //also sounds
        //these can be in the json data as strings and reference the content pipeline 

        public void RunMove()
        {
            if(moveMethodId == 0)
            {
                //standard
            }
            else if(moveMethodId == 1)
            {
                //lifesteal attack
            }
            else if (moveMethodId == 2)
            {
                //self-heal
            }
            //could add a 'hyper beam' style attack where the player
            //will do massive damage but will then need to recharge afterward
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
