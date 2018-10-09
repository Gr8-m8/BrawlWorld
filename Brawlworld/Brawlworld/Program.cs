using System;

namespace Brawlworld
{
    class Program
    {
        static void Main()
        {
            Lexicon Q = new Lexicon();
            GameController gctrl = new GameController();
            Console.Title = "BrawlWorld";

            Console.WriteLine();
            Console.WriteLine("Welcome to BrawlWorld!");
            Console.WriteLine();
            Console.CursorSize = 10;
            Console.WriteLine("Press any key to start.");
            Console.WriteLine();

            Console.ReadKey();


            /*
            gctrl.players[0].plr.StatsSet(0, true);
            //*/
            gctrl.players[0].plr.StatsGen(0, true);
            Console.WriteLine("Your Character:\n" + gctrl.players[0].plr.Name() + " " + gctrl.players[0].plr.WriteLvl() + "\n" + gctrl.players[0].plr.WriteStats());

            while (gctrl.GameIsRunning)
            {
                Console.ReadKey();
                Console.WriteLine();

                Golem opponent = new Golem();
                opponent.lvl = 100; //new Random().Next(Math.Max(1, gctrl.players[0].plr.lvl - 3), gctrl.players[0].plr.lvl + 3);
                opponent.StatsGen(0, true);
                opponent.NameSet(Q.ng.GenName());
                Console.WriteLine(opponent.Name() + " " + opponent.WriteLvl() + "\n" + opponent.WriteStats());

                //gctrl.players[0].plr.Lvl(10 + (2 * opponent.lvl -  2 * gctrl.players[0].plr.lvl));
            }
        }
    }

    //GAMECONTROLL

    class Lexicon
    {
        public Lexicon()
        {
            ResizeWindow();
        }

        public string Read()
        {
            string input = Console.ReadLine();

            switch (input)
            {
                case "?":
                    break;

                case "!":
                    break;
            }

            return input;
        }

        public string ReadKey()
        {
            return "";
        }

        public int StringInt(string input, int defaultNum = 0)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            } else
            {
                Console.WriteLine("Input was not a number. Number set to " + defaultNum);
                return defaultNum;
            }
        }

        public void ErrorInput(string inputError, string[] inputValid)
        {
            Console.Write("Input " + inputError + " was invalid. Enter valid input: ");
            for (int i = 0; i < inputValid.Length; i++)
            {
                Console.Write("[" + inputValid[i] + "]");
                if (i != inputValid.Length)
                {
                    Console.Write("; ");
                } else
                {
                    Console.Write(".\n");
                }
            }
        }

        public void ResizeWindow()
        {

            //Console.SetWindowSize((Console.LargestWindowWidth - 2*Console.WindowLeft), (Console.LargestWindowHeight - 2*Console.WindowTop));

            //Console.WindowHeight = Console.LargestWindowHeight;
            //Console.WindowWidth = Console.LargestWindowWidth;
            Console.SetWindowPosition(0, 0);

            //Console.WriteLine(Console.SetWindowPosition.ToString());
        }

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
        char[] vokal = new char[6] { 'e', 'y', 'u', 'i', 'o', 'a' };
        char[] konsonant = new char[20] { 'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
        string[] title = new string[9] { "Slayer", "Destroyer", "Defender", "Protector", "Maester", "Professor", "Escapist", "Assasin", "Lord" };

        Random r = new Random();

        public string GenName()
        {
            string name = "";
            int lenght = r.Next(2, 8);
            int streak = 0;
            int type = 1;

            if (r.Next(100) < 50)
            {
                type *= -1;
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

                if (r.Next(101) < 25 + 25 * streak + 5*type)
                {
                    type *= -1;
                    streak = 0;
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
        public bool GameIsRunning = true;

        public Player[] players = new Player[1] { new Player() };

        void battle()
        {

        }
    }

    //MAP
    class Map
    {
        Lexicon Q = new Lexicon();

        Tile[,] map;

        public Map(int widthSet = 5, int heightSet = 5)
        {
            map = new Tile[widthSet, heightSet];
        }
    }

    class MapCell : Map
    {
        Map[,] map;
    }

    class Tile
    {
        public bool unWalkable;

    }

    //ENTITY
    class Entity
    {
        public Lexicon Q = new Lexicon();

        public string name;
        public int[,] pos;

        public string Name()
        {
            return name;
        }

        public void NameSet(string nameSet)
        {
            name = nameSet;
        }
    }

    class Actor : Entity
    {
        int vitality = 1;
        int strenght = 1;
        int intelligence = 1;
        int agility = 1;
        public int[] sts = new int[4] { 1, 1, 1, 1 };


        int xp;
        public int lvl = 1;

        Item[] inventory = new Item[6];
        Item[] inventoryOld;

        Item weapon;
        Item armor;

        public string WriteLvl()
        {
            return "[lvl: " + lvl + "]";
        }

        public string WriteStats()
        {
            return "[Strength: " + strenght + " | Vitality: " + vitality + " | Intelegence: " + intelligence + " | Agility: " + agility + "]";
        }

        public void Lvl(int amount, bool lvlupSkillSet = true)
        {
            xp += amount;
            Console.WriteLine("+" + amount + "xp");

            if (xp >= 100 * lvl)
            {
                xp -= 100 * lvl;
                lvl++;

                Console.WriteLine("LVL UP! [lvl: " + lvl + "]");

                if (lvlupSkillSet)
                {
                    StatsSet(2);
                }
                else
                {
                    StatsGen(2);
                }
            }
        }

        public void StsSet(int strenghtChance, int vitalityChance, int intelligenceChance, int agilityChance)
        {
            sts[0] = strenghtChance;
            sts[1] = vitalityChance;
            sts[2] = intelligenceChance;
            sts[3] = agilityChance;
        }

        public void StatsSet(int skillPoints, bool startUp = false)
        {

            if (startUp)
            {
                Console.WriteLine("Set Name");
                NameSet(Console.ReadLine());
                skillPoints = 5 + lvl * 2;
            }
            int skillAmountSet = 0;

            while (skillPoints > 0)
            {
                Console.WriteLine("Distribute Skillpoints: [S]renght, [V]itality, [I]ntelegence, [A]gility");
                Console.WriteLine("Skillpoints: " + skillPoints);
                Console.WriteLine(WriteStats());
                //string skillSet = Console.ReadLine().ToUpper().Substring(0, 1);
                string skillSet = Console.ReadKey().KeyChar.ToString().ToUpper();

                switch (skillSet)
                {
                    case "S":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.StringInt(Console.ReadKey().KeyChar.ToString().ToUpper(), 1);
                        strenght += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    case "V":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.StringInt(Console.ReadKey().KeyChar.ToString(), 1);
                        strenght += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    case "I":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.StringInt(Console.ReadKey().KeyChar.ToString(), 1);
                        strenght += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    case "A":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.StringInt(Console.ReadKey().KeyChar.ToString(), 1);
                        strenght += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    default:
                        Q.ErrorInput(skillSet, new string[4] { "S", "V", "I", "A" });
                        break;
                }
            }

        }

        public void StatsGen(int skillPoints, bool startUp = false)
        {
            if (startUp)
            {
                NameSet(Q.ng.GenName());
                skillPoints = 5 + lvl * 2;
            }

            Random r = new Random();

            for (int i = 0; i < skillPoints; i++)
            {
                int rn = r.Next(sts[0] + sts[1] + sts[2] + sts[3]);

                if (rn >= sts[0] + sts[1] + sts[2])
                {
                    agility++;
                }
                else if (rn >= sts[0] + sts[1])
                {
                    intelligence++;
                }
                else if (rn >= sts[0])
                {
                    vitality++;
                }
                else
                {
                    strenght++;
                }
            }
        }
    }

    interface IWarrior
    {
        
    }

    class Warrior : Actor
    {
        public Warrior()
        {
            StsSet(6, 1, 1, 1);
        }
    }

    class Tank : Actor
    {
        public Tank()
        {
            StsSet(1, 6, 1, 1);
        }
    }

    class Wizard : Actor
    {
        public Wizard()
        {
            StsSet(1, 1, 6, 1);
        }
    }

    class Scout : Actor
    {
        public Scout()
        {
            StsSet(1, 1, 1, 6);
        }
    }

    class Golem : Actor
    {
        public Golem()
        {
            StsSet(10, 10, 1, 1);
        }
    }

    class Hero : Actor
    {

    }

    class Player
    {
        public Hero plr = new Hero();
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

    class Rune : Item
    {

    }

    class Inventory
    {
        int size;
        Item[] content;
    }
}
