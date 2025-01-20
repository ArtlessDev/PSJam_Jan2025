using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSGJ_Jan2025
{
    internal class Character
    {
        int health, attack;
        CharacterType type;
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

    internal class NPC : Character
    {
        public NPC() : base()
        {

        }

        public void Move()
        {

        }
        public void Attack()
        {

        }
        public void Guard()
        {

        }

        public void chooseAction()
        {

        }
    }
    public enum CharacterType
    {
        Player,
        NPC
    }
}
