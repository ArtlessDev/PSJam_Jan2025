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
            currentHealth = 30;
            maximumHealth = 30;
            attack = 20;
            type = CharacterType.NPC;
        }
        public Character(CharacterType type)
        {
            currentHealth = 30;
            maximumHealth = 30;
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
        bool isStrongMob;

        public NPC() : base()
        {
            canMoveZones = true;
            isGuarding = false;
            int tempmobStrength = Random.Shared.Next(0, 7);
            zone = 5;
            int tempElement = Random.Shared.Next(0, 5);
            mobElement = (Element)tempElement;

            Debug.WriteLine(tempmobStrength);

            if (tempmobStrength == 7)
            {
                isStrongMob = true;
                CurrentHealth = 40 * GameMaster.WaveNumber;
                Rect = new Rectangle(Random.Shared.Next(192 * zone, (192 * zone) + 192), Random.Shared.Next(32, 320), 32, 32);
                Texture = GameMaster.CustomContent.Load<Texture2D>("soldier");
                Debug.WriteLine("BIG");
            }
            else
            {
                Rect = new Rectangle(Random.Shared.Next(192 * zone, (192 * zone) + 192), Random.Shared.Next(32, 320), 16, 16);
                CurrentHealth = 20 * GameMaster.WaveNumber;
                Texture = GameMaster.CustomContent.Load<Texture2D>("soldier");
                isStrongMob = false;

            }


            switch (mobElement)
            {
                case Element.Fire: TextureColor = Color.Red; break;
                case Element.Ice: TextureColor = Color.Blue; break;
                case Element.Electric: TextureColor = Color.Yellow; break;
                case Element.Physical: TextureColor = Color.Orange; break;
                case Element.Universal: TextureColor = Color.Silver; break;
                default: TextureColor = Color.White; break;
            }
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
            if (isStrongMob)
            {
                zone--;
                isGuarding = false;
                //this flag can be used to implement stronger enemies very easily
                //canMoveZones = false;
                //Debug.WriteLine("moving to zone number " + zone);

                int xPosition = Random.Shared.Next(192 * zone, (192 * zone) + 192);
                for (int transition = Rect.X; transition > xPosition; transition--)
                {
                    Rect = new Rectangle(transition, Rect.Y, 32, 32);
                    await Task.Delay(10);
                }
                //Debug.WriteLine(GameMaster.enemyWave.IndexOf(this) + ": " + xPosition);
            }
            else
            {
                if (canMoveZones)
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
                        await Task.Delay(10);
                    }
                    //Debug.WriteLine(GameMaster.enemyWave.IndexOf(this) + ": " + xPosition);
                }
            }
        }
        public void AttackAction(Character zilla)
        {
            isGuarding = false;
            //Debug.WriteLine("attacking");
            zilla.TakesDamage();
            if (isStrongMob)
            {
                zilla.TakesDamage();
                zilla.TakesDamage();
            }

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
            
            if (this.MobElement == Element.Universal)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Physical:
                        return true;
                    default:
                        return false;
                }
            }
            if (this.MobElement == Element.Ice)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Ice:
                    case Element.Electric:
                    case Element.Physical:
                        return false;
                    default:
                        return true;
                }
            }
            if (this.MobElement == Element.Fire)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Fire:
                    case Element.Electric:
                    case Element.Physical:
                        return false;
                    default:
                        return true;
                }
            }
            if (this.MobElement == Element.Electric)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Fire:
                    case Element.Ice:
                    case Element.Physical:
                        return false;
                    default:
                        return true;
                }
            }
            if (this.MobElement == Element.Physical)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Fire:
                    case Element.Ice:
                    case Element.Physical:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        public bool IsResistantTo(Ability selectedAbility)
        {

            if (this.MobElement == Element.Universal)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Physical:
                        return false;
                    default:
                        return true;
                }
            }
            if (this.MobElement == Element.Ice)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Ice:
                    case Element.Electric:
                    case Element.Physical:
                        return true;
                    default:
                        return false;
                }
            }
            if (this.MobElement == Element.Fire)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Fire:
                    case Element.Electric:
                    case Element.Physical:
                        return true;
                    default:
                        return false;
                }
            }
            if (this.MobElement == Element.Electric)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Fire:
                    case Element.Ice:
                    case Element.Physical:
                        return true;
                    default:
                        return false;
                }
            }
            if (this.MobElement == Element.Physical)
            {
                switch (selectedAbility.MoveElement)
                {
                    case Element.Fire:
                    case Element.Ice:
                    case Element.Physical:
                        return false;
                    default:
                        return true;
                }
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
