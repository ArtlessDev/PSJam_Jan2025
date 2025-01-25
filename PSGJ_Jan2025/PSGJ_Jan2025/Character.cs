using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    public class Character
    {
        int maximumHealth, currentHealth, attack;
        CharacterType type;
        Color textureColor;
        Rectangle rect;
        Texture2D texture;
        Rectangle[] zones = {
            new Rectangle(0, 192, 192, 288),
            new Rectangle(Random.Shared.Next(192*1, (192*1)+192), Random.Shared.Next(32,320), 192, 288),
            new Rectangle(Random.Shared.Next(192*2, (192*2)+192), Random.Shared.Next(32,320), 192, 288),
            new Rectangle(Random.Shared.Next(192*3, (192*3)+192), Random.Shared.Next(32,320), 192, 288),
            new Rectangle(Random.Shared.Next(192*4, (192*4)+192), Random.Shared.Next(32,320), 192, 288),
            new Rectangle(Random.Shared.Next(192*5, (192*5)+192), Random.Shared.Next(32,320), 192, 288),
        };
        public Character()
        {
            currentHealth = 50;
            maximumHealth = 50;
            attack = 20;
            type = CharacterType.NPC;
        }
        public Character(CharacterType type)
        {
            currentHealth = 50;
            maximumHealth = 50;
            attack = 20;
            this.type = type;

            Rect = new(32, 128, 128, 128);
            Texture = GameMaster.CustomContent.Load<Texture2D>("zilla");
            textureColor = Color.White;
        }

        public Color TextureColor
        {
            get { return textureColor; }
            set { textureColor = value; }
        }

        List<Ability> movesList = new List<Ability>();

        public List<Ability> MovesList
        {
            get { return movesList; }
            set { movesList = value; }
        }

        public async void TakesDamage()
        {
            currentHealth--;

            this.TextureColor = Color.Red;
            await Task.Delay(150);
            this.TextureColor = Color.White;
            await Task.Delay(150);
            this.TextureColor = Color.Red;
            await Task.Delay(150);
            this.TextureColor = Color.White;
            await Task.Delay(150);
        }

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }
        public int MaximumHealth
        {
            get { return maximumHealth; }
            set { maximumHealth = value; }
        }
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        public CharacterType Type
        {
            get { return type; }
            set { type = value; }
        }
    }

    public class NPC : Character
    {
        int zone;
        bool isGuarding, canMoveZones;
        Element mobElement;

        public NPC() : base()
        {
            canMoveZones = true;
            CurrentHealth = 20 * GameMaster.WaveNumber;
            zone = 5;
            Rect = new Rectangle(Random.Shared.Next(192*zone, (192*zone)+192), Random.Shared.Next(32,320), 16, 16);
            Texture = GameMaster.CustomContent.Load<Texture2D>("soldier");
            isGuarding = false;
            TextureColor = Color.White;
            int tempElement = Random.Shared.Next(0, 5);
            mobElement = (Element)tempElement;
            Debug.WriteLine(mobElement);
        }

        public Element MobElement
        {
            get{ return mobElement; }
            set { mobElement = value; }
        }
        public int Zone
        {
            get { return zone; }
            set { value = zone; }
        }
        public bool IsGuarding
        {
            get { return isGuarding; }
            set { value = isGuarding; }
        }

        public bool CanMoveZones
        {
            get { return isGuarding; }
            set { value = isGuarding; }
        }

        public async void MoveAction()
        {

            //Debug.WriteLine("moving");
            if(canMoveZones)
            {
                zone--;
                isGuarding = false;
                //this flag can be used to implement stronger enemies very easily
                //canMoveZones = false;
                //Debug.WriteLine("moving to zone number " + zone);

                int xPosition = Random.Shared.Next(192 * zone, (192 * zone) + 192);
                for (int transition = Rect.X; transition > xPosition; transition--)
                {
                    Rect = new Rectangle(transition, Rect.Y, 16, 16);
                    await Task.Delay(25);
                }
                //Debug.WriteLine(GameMaster.enemyWave.IndexOf(this) + ": " + xPosition);
            }
        }
        public void AttackAction(Character zilla)
        {
            isGuarding = false;
            //Debug.WriteLine("attacking");
            zilla.TakesDamage();
            
        }
        public void GuardAction()
        {
            //Debug.WriteLine("guarding");
            isGuarding = true;
        }

        public void chooseAction(Character zilla)
        {
            int actionChoice = Random.Shared.Next(0,2);

            switch (actionChoice)
            {
                case 0:
                    MoveAction();
                    break;
                case 1:
                    AttackAction(zilla);
                    break;
                case 2:
                default:
                    GuardAction();
                    break;
            }
        }

        public bool IsWeakTo(Ability selectedAbility)
        {
            //THERES GOTTA BE A BETTER WAY TO CHECK THIS BUT I CANT BE BOTHERED RIGHT NOW TO FIGURE IT OUT SO ENJOY THE SPAGHETTI
            
            if (this.MobElement == Element.Universal && selectedAbility.MoveElement == Element.Physical)
            {
                return true;
            }
            if (this.MobElement == Element.Ice && (selectedAbility.MoveElement == Element.Fire || selectedAbility.MoveElement == Element.Universal))
            {
                return true;
            }
            if (this.MobElement == Element.Fire && (selectedAbility.MoveElement == Element.Electric || selectedAbility.MoveElement == Element.Universal))
            {
                return true;
            }
            if (this.MobElement == Element.Electric && (selectedAbility.MoveElement == Element.Ice || selectedAbility.MoveElement == Element.Universal))
            {
                return true;
            }
            if (this.MobElement == Element.Physical && (selectedAbility.MoveElement == Element.Ice || selectedAbility.MoveElement == Element.Electric || selectedAbility.MoveElement == Element.Fire))
            {
                return true;
            }
            return false;
        }

        public bool IsResistantTo(Ability selectedAbility)
        {
            //TYPES CANNOT BE STRONG AGAINST THEMSELVES EXCEDPT UNIVERSAL
            if (this.MobElement == Element.Universal && (selectedAbility.MoveElement == Element.Fire || selectedAbility.MoveElement == Element.Ice || selectedAbility.MoveElement == Element.Electric))
            {
                return true;
            }
            if (this.MobElement == Element.Physical && (selectedAbility.MoveElement == Element.Physical || selectedAbility.MoveElement == Element.Universal))
            {
                return true;
            }
            if (this.MobElement == Element.Ice && (selectedAbility.MoveElement == Element.Ice || selectedAbility.MoveElement == Element.Electric))
            {
                return true;
            }
            if (this.MobElement == Element.Fire && (selectedAbility.MoveElement == Element.Fire || selectedAbility.MoveElement == Element.Ice))
            {
                return true;
            }
            if (this.MobElement == Element.Electric && (selectedAbility.MoveElement == Element.Electric || selectedAbility.MoveElement == Element.Fire))
            {
                return true;
            }
            return false;
        }
    }
    public enum CharacterType
    {
        Player,
        NPC
    }
}
