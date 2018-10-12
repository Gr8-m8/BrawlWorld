using System;

namespace Brawlworld
{
    class Program
    {
        static void Main()
        {
            Lexicon Q = new Lexicon();
            GameController gctrl = new GameController(Q);

            Console.Title = "BrawlWorld";
            Console.WriteLine("Welcome to BrawlWorld!");
            Console.WriteLine();
            /*
            Console.WriteLine("'!' will activete Options Menu");
            Console.WriteLine("'?' will activete Help Menu");
            Console.WriteLine();
            //*/
            Console.WriteLine("Press any key to start.");
            Console.WriteLine();

            Q.InputKey();
            gctrl.InitPlr();

            /*
            gctrl.players[0].plr.StatsSet(0, true);
            //*/gctrl.players[0].plr.StatsGen(0, true);
            Console.WriteLine("Your Character:\n" + gctrl.players[0].plr.Name() + " " + gctrl.players[0].plr.WriteLvl() + "\n" + gctrl.players[0].plr.WriteStats());

            while (gctrl.GameIsRunning)
            {
                Q.InputKey();
                Console.WriteLine();

                Actor opponent = new Actor(Q);
                opponent.lvl = new Random().Next(Math.Max(1, gctrl.players[0].plr.lvl - 3), gctrl.players[0].plr.lvl + 3);
                opponent.StatsGen(0, true);
                Console.WriteLine(opponent.Name() + " " + opponent.WriteLvl() + "\n" + opponent.WriteStats());

                //gctrl.players[0].plr.Lvl(10 + (2 * opponent.lvl -  2 * gctrl.players[0].plr.lvl));
            }
        }
    }

    //GAMECONTROLL

    class Lexicon
    {
        public string InputText(bool helpActive = true)
        {
            string text = Console.ReadLine();

            if (helpActive)
            {
                if (Menu(text))
                {
                    text = Console.ReadLine();
                }
            }

            return text;
        }

        public string InputKey(bool helpActive = true)
        {
            string key = Console.ReadKey().KeyChar.ToString().ToUpper();
            Console.WriteLine();

            if (helpActive)
            {
                if (Menu(key))
                {
                    key = Console.ReadKey().KeyChar.ToString().ToUpper();
                } 
            }

            return key;
        }

        public int InputNumberInt(bool helpActive = true)
        {
            string stringNum = Console.ReadLine();
            if (helpActive)
            {
                if (Menu(stringNum))
                {
                    stringNum = Console.ReadLine();
                }
            }

            int num = 0;
            bool confirmNum = true;
            while (confirmNum)
            {
                if (int.TryParse(stringNum, out int result))
                {
                    confirmNum = false;
                    num = result;
                }
                else
                {
                    ErrorInput(stringNum, new string[1] { "Number" });
                    stringNum = Console.ReadLine();
                }
            }
            return num;
        }

        public bool Menu(string input)
        {
            bool menuActive = true;
            switch (input)
            {
                case "!":
                    Console.WriteLine("    Options Menu:");
                    while (menuActive)
                    {
                        Console.WriteLine("    E[x]it Menu");
                        Console.WriteLine("    [E]xit Game");
                        switch (InputKey(false))
                        {
                            case "X":
                                menuActive = false;
                                break;

                            case "E":
                                Console.WriteLine("Confirm Exit Game: [Y]es, [N]o");
                                switch (InputKey(false))
                                {
                                    case "Y":
                                        System.Environment.Exit(0);
                                        break;

                                    case "N":
                                        break;

                                }
                                break;

                            default:
                                ErrorInput("", new string[0]);
                                break;
                        }
                    }
                    break;

                case "?":
                    Console.WriteLine("    Help Menu:");
                    while (menuActive)
                    {
                        Console.WriteLine("    E[x]it Menu");
                        switch (InputKey(false))
                        {
                            case "X":
                                menuActive = false;
                                break;

                            default:
                                ErrorInput("", new string[0]);
                                break;
                        }
                    }
                    break;

                default:
                    return false;
            }
            return true;
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

        public NameGenerator(int debugSeed = -1)
        {
            if(debugSeed > 0)
            {
                r = new Random(debugSeed);
            }
        }

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

                if (r.Next(101) < 25 + 25 * streak + 5 * type)
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
        public Lexicon Q;

        public GameController(Lexicon getQ)
        {
            Q = getQ;
        }

        public bool GameIsRunning = true;

        int numOfPlr = 1;
        public Player[] players;

        public void InitPlr(int numOfPlrSet = 1)
        {
            numOfPlr = numOfPlrSet;
            players = new Player[numOfPlr];
            for (int i = 0; i < numOfPlr; i++)
            {
                players[i] = new Player(Q);
            }
        }

        void Battle(Actor[,] team_player)
        {
            bool battle = true;
            while (battle)
            {
                for (int player = 0; player < team_player.GetUpperBound(1); player++)
                {
                    for (int team = 0; team < team_player.GetUpperBound(0); team++)
                    {
                        Console.WriteLine("Team " + team + " Player " + player + " Turn");
                        if (team_player[team, player].isAI)
                        {
                            Random r = new Random();


                            
                        } else
                        {
                            bool action = true;
                            while (action)
                            {
                                switch (Q.InputKey())
                                {
                                    case "A":
                                        action = false;
                                        break;

                                    case "D":
                                        action = false;
                                        break;

                                    case "M":
                                        action = false;
                                        break;

                                    case "I":
                                        action = false;
                                        break;

                                    default:
                                        Q.ErrorInput("", new string[0]{});
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //MAP
    class MapManager
    {
        Lexicon Q;

        MapManager(Lexicon getQ)
        {
            Q = getQ;
        }

        Tile[,] map;

        void GenMap(int xSize = 10, int ySize = 10)
        {
            map = new Tile[xSize, ySize];
        }

    }

    class Tile
    {
        public bool unWalkable;

        Entity occupant;

    }

    //ENTITY
    class Entity
    {
        public Lexicon Q;

        public Entity(Lexicon getQ)
        {
            Q = getQ;
        }

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
        public Actor(Lexicon getQ) : base(getQ)
        {
            Q = getQ;
        }

        public bool isAI = true;

        int strenght = 1;
        int vitality = 1;
        int intelligence = 1;
        int agility = 1;
        public int[] sts = new int[4] { 1, 1, 1, 1 };

        int xp;
        public int lvl = 1;

        Inventory inventory = new Inventory();
        Weapon weapon;
        Armor armor;

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
                NameSet(Q.InputText());
                skillPoints = 5 + lvl * 2;
            }
            int skillAmountSet = 0;

            while (skillPoints > 0)
            {
                Console.WriteLine("Distribute Skillpoints: [S]renght, [V]itality, [I]ntelegence, [A]gility");
                Console.WriteLine("Skillpoints: " + skillPoints);
                Console.WriteLine(WriteStats());
                //string skillSet = Console.ReadLine().ToUpper().Substring(0, 1);
                string skillSet = Q.InputKey();

                switch (skillSet)
                {
                    case "S":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.InputNumberInt();
                        strenght += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    case "V":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.InputNumberInt();
                        vitality += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    case "I":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.InputNumberInt();
                        intelligence += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    case "A":
                        Console.WriteLine("Amount of points");
                        skillAmountSet = Q.InputNumberInt();
                        agility += Math.Min(skillAmountSet, skillPoints);
                        skillPoints -= Math.Min(skillAmountSet, skillPoints);
                        break;

                    default:
                        Q.ErrorInput(skillSet, new string[0] /*new string[4]{ "S", "V", "I", "A" }*/);
                        break;
                }
            }

        }

        public void StatsGen(int skillPoints, bool startUp = false)
        {
            if (startUp)
            {
                NameSet(new NameGenerator().GenName());
                skillPoints = 5 + lvl * 2;
            }

            Random r = new Random();

            StsSet(1, 1, 1, 1);

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

    class Warrior : Actor
    {
        public Warrior(Lexicon getQ) : base(getQ)
        {
            Q = getQ;
            StsSet(6, 1, 1, 1);
        }
    }

    class Tank : Actor
    {
        public Tank(Lexicon getQ) : base(getQ)
        {
            Q = getQ;
            StsSet(1, 6, 1, 1);
        }
    }

    class Wizard : Actor
    {
        public Wizard(Lexicon getQ) : base(getQ)
        {
            Q = getQ;
            StsSet(1, 1, 6, 1);
        }
    }

    class Scout : Actor
    {
        public Scout(Lexicon getQ) : base(getQ)
        {
            Q = getQ;
            StsSet(1, 1, 1, 6);
        }
    }

    class Golem : Actor
    {
        public Golem(Lexicon getQ) : base(getQ)
        {
            Q = getQ;
            StsSet(10, 10, 1, 1);
        }
    }

    class Player
    {
        public Actor plr;
        public Player(Lexicon getQ)
        {
            plr = new Actor(getQ);
            plr.isAI = false;
        }
         
    }

    //ITEM
    class Item
    {
        public int strenght;
        public int vitality;
        public int intelegence;
        public int agility;

        public Item(int s, int v, int i, int a)
        {
            strenght = s;
            vitality = v;
            intelegence = i;
            agility = a;
        }

        void Use(Actor user)
        {

        }
    }

    class Armor : Item
    {
        public Armor(int s, int v, int i, int a, int lvlset = 1) : base(s,  v,  i,  a)
        {
            strenght = s;
            vitality = v;
            intelegence = i;
            agility = a;
        }

        void Use(Actor user)
        {

        }
    }

    class Weapon : Item
    {
        public Weapon(int s, int v, int i, int a, int lvlset = 1) : base(s, v, i, a)
        {
            strenght = s;
            vitality = v;
            intelegence = i;
            agility = a;
        }

        void Use(Actor user)
        {

        }
    }

    class Rune : Item
    {
        public Rune(int s, int v, int i, int a, int lvlset = 1) : base(s, v, i, a)
        {
            strenght = s;
            vitality = v;
            intelegence = i;
            agility = a;
        }

        void Use(Actor user)
        {

        }
    }

    class Inventory
    {
        Item[] content;
        Item[] contentOld;

        public Inventory(int inventorySize = 6)
        {
            content = new Item[inventorySize];
        }
    }
}
