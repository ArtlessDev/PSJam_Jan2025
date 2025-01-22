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
        int health, attack;
        CharacterType type;
        Color textureColor;
        Rectangle rect;
        Texture2D texture;

        public Character()
        {
            health = 20;
            attack = 20;
            type = CharacterType.NPC;
        }
        public Character(CharacterType type)
        {
            health = 20;
            attack = 20;
            this.type = type;

            Rect = new(128, 128, 128, 128);
            Texture = GameMaster.CustomContent.Load<Texture2D>("zilla");
            textureColor = Color.White;
        }

        public Color TextureColor
        {
            get { return textureColor; }
            set { textureColor = value; }
        }

        public async void TakesDamage()
        {
            health--;

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
        public int Health
        {
            get { return health; }
            set { health = value; }
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
        bool isGuarding;

        public NPC() : base()
        {
            Health = 30 * GameMaster.WaveNumber;
            zone = 5;
            Rect = new Rectangle(Random.Shared.Next(200*zone, 250*zone), Random.Shared.Next(100,400), 16, 16);
            Texture = GameMaster.CustomContent.Load<Texture2D>("soldier");
            isGuarding = false;
            TextureColor = Color.White;
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

        public async void MoveAction()
        {
            isGuarding = false;

            Debug.WriteLine("moving");
            zone--;
            int xPosition = Random.Shared.Next(200 * zone, 250 * zone);
            for (int transition = Rect.X; transition > xPosition; transition--)
            {
                Rect = new Rectangle(transition, Rect.Y, 16, 16);
                await Task.Delay(25);
            }
        }
        public void AttackAction(Character zilla)
        {
            isGuarding = false;
            Debug.WriteLine("attacking");
            zilla.TakesDamage();
            
        }
        public void GuardAction()
        {
            Debug.WriteLine("guarding");
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
                    Debug.WriteLine($"zilla health: {zilla.Health}");
                    break;
                case 2:
                default:
                    GuardAction();
                    break;
            }
        }
    }
    public enum CharacterType
    {
        Player,
        NPC
    }
}
