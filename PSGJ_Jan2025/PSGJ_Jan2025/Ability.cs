using Microsoft.Xna.Framework;
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
        float baseDamagePower;
        Element moveType;
        int moveMethodId;
        Color elementColor;

        public Ability()
        {
            int tempType = Random.Shared.Next(1,5);
            moveType = (Element)tempType;
            baseDamagePower = 40;
            //int basePowerModifier = GameMaster.WaveNumber; //might need this later for determining what kind of power strength i want

            abilityName = moveType.ToString();

            moveMethodId = 0;

            switch (moveType) 
            { 
                case Element.Fire: elementColor = Color.Red; break;
                case Element.Ice: elementColor = Color.Blue; break;
                case Element.Electric: elementColor = Color.Yellow; break;
                case Element.Physical: elementColor = Color.Orange; break;
                case Element.Universal: elementColor = Color.Silver; break;
            }
        }
        public Ability(CustomGameUI button)
        {
            abilityName = button.MoveName;
            baseDamagePower = 40;
            moveType = Element.Physical;
        }

        public Ability(string name)
        {
            abilityName = name;
            elementColor = Color.White;
        }

        public float BaseDamagePower
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
        public Color ElementColor
        {
            get { return elementColor; }
            set { elementColor = value; }
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
