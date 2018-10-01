﻿using System;

namespace Brawlworld
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexicon Q = new Lexicon();

            string target = "";
            int tries = 0;
            while (true)
            {
                tries++;
                target = Q.ng.GenName();
                Console.WriteLine(target);

                Console.ReadKey();
                Console.Clear();

                if(target.ToUpper() == "")
                {
                    Console.WriteLine(tries);
                    Console.ReadKey();
                }
            }
        }
    }
    
    //GAMECONTROLL

    class Lexicon
    {
        public NameGenerator ng = new NameGenerator();

        public int GetX()
        {
            return 0;
        }

        public int GetY()
        {
            return 1;
        }
    }

    class NameGenerator
    {
        char[] vokal = new char[6] {'e', 'y', 'u', 'i', 'o', 'a' };
        char[] konsonant = new char[20] {'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
        string[] title = new string[8] {"Slayer", "Destroyer", "Defender", "Protector", "Maester", "Professor", "Escapist", "Assasin" };

        int lenght;
        int streak;
        int type = 1;

        string name;

        Random r = new Random();

        public string GenName()
        {
            name = "";
            lenght = r.Next(2, 8);

            if (r.Next(100) < 50)
            {
                type *= -1;
                streak = 0;
            }

            for (int i = 0; i < lenght; i++)
            {
                if (type > 0)
                {
                    name += vokal[r.Next(vokal.Length)].ToString();
                }
                else
                {
                    name += konsonant[r.Next(konsonant.Length)].ToString();
                }
                streak++;

                if(r.Next(101) < 25 + 25 * streak + 23*type)
                {
                    type *= -1;
                }

                if (i == 0)
                {
                    name = name.ToUpper();
                }
            }

            if (r.Next(100) > 85)
            {
                name += " " + GenName(); ;
            }

            if (r.Next(100) > 75)
            {
                name += " the " + title[r.Next(title.Length)];
            }

            if (r.Next(100) > 85)
            {
                name += " of " + GenName();
            }

            return name;
        }
    }

    class GameController
    {

    }

    //MAP
    class Map
    {
        Lexicon Q = new Lexicon();

        int[] dimensions = new int[2];

        Tile[,] map;
    }

    class Tile
    {
        public bool unWalkable;


    }
    
    
    //ENTITY
    class Entity
    {
        Lexicon Q = new Lexicon();

        int[,] pos;
    }

    class Actor : Entity
    {
        int vitality;
        int strenght;
        int intelegence;
        int agility;

        int xp;
        int lvl;

        Item[] inventory = new Item[6];
        Item[] inventoryOld;

        Item weapon;
        Item armor;
    }

    class Enemy : Actor
    {

    }

    class Hero : Actor
    {

    }

    class Player
    {
        Hero plr = new Hero();
    }

    //ITEM
    class Item
    {
        public int vitality;
        public int strenght;
        public int intelegence;
        public int agility;

        int xp;
        int lvl;
    }

    class Armor : Item
    {

    }

    class Weapon : Item
    {

    }

    class Inventory
    {
        int size;
        Item[] content;
    }
}
