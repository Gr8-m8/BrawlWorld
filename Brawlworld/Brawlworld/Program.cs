using System;
using System.Collections.Generic;

namespace Brawlworld
{
    class Program
    {
        static int Main()
        {
            Lexicon Q = new Lexicon();
            GameController gctrl = new GameController(Q);

            gctrl.InitPlr();
            MapManager mm = new MapManager(Q);
            mm.map.MapGen(Convert.ToInt32((mm.map.width * mm.map.height) * 0.2), new int[0]);
            gctrl.players[0].plr.pos = new int[2] { (mm.map.width/2), (mm.map.height/2) };

            while (true)
            {
                mm.RendMap(gctrl.players[0].plr.pos);
                Console.WriteLine(gctrl.players[0].plr.pos[Q.GetX()] + " " + gctrl.players[0].plr.pos[Q.GetY()]);

                switch (Q.InputKey())
                {
                    default:
                        Console.WriteLine("No Input");
                        Q.InputKey();
                        break;

                    case "W":
                    case "I":
                        mm.MoveEntity(gctrl.players[0].plr, new int[2] { 0, -1 });
                        break;

                    case "J":
                    case "A":
                        mm.MoveEntity(gctrl.players[0].plr, new int[2] { -1, 0 });
                        break;

                    case "K":
                    case "S":
                        mm.MoveEntity(gctrl.players[0].plr, new int[2] { 0, 1 });
                        break;

                    case "L":
                    case "D":
                        mm.MoveEntity(gctrl.players[0].plr, new int[2] { 1, 0 });
                        break;

                }
                
                
            }

            Console.Title = "BrawlWorld";
            //Console.WriteLine("Welcome to BrawlWorld!");
            Console.Write("Welcome ");      //Console.Beep(220, 180); Console.Beep(220, 280);
            Console.Write("to ");           //Console.Beep(329, 600);
            Console.Write("BrawlWorld! ");  //Console.Beep(391, 150); Console.Beep(329, 700);
            Console.WriteLine();
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
            gctrl.players[0].plr.ActorSetup(1,false);
            //*/gctrl.players[0].plr.ActorSetup();
            Console.WriteLine("Your Character:\n" + gctrl.players[0].plr.WriteActor());

            while (gctrl.GameIsRunning)
            {
                Q.InputKey();

                Actor opponent = new Actor(Q);
                opponent.ActorSetup(100/*new Random().Next(Math.Max(1, gctrl.players[0].plr.lvl - 3), gctrl.players[0].plr.lvl + 3)*/);
                Console.WriteLine(opponent.WriteActor());
            }

            return 0;
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
            //Console.Beep(200, 200);
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
            //Console.Beep(200, 200);
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
                    //Console.Beep(200, 200);
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

        public void TransferItem(Inventory[] inv)
        {

            for (int i = 0; i < inv.Length; i++)
            {
                Console.WriteLine("Inventory " + (i + 1) + ":");
                inv[i].WriteContent();
            }

            Console.WriteLine("Inventory Number");
            Inventory invSrc = inv[Clamp(0, inv.Length, InputNumberInt() - 1)];

            Console.WriteLine("Item Number");
            int itemNum = Math.Max(0, Math.Min(invSrc.contentList.Length, InputNumberInt() - 1));

            Console.WriteLine("Item quantity");
            int itemQuantity = InputNumberInt();

            Item itemTransferSrc = new Item(invSrc.contentList[itemNum].strenght, invSrc.contentList[itemNum].vitality, invSrc.contentList[itemNum].intelegence, invSrc.contentList[itemNum].agility, invSrc.contentList[itemNum].value, invSrc.contentList[itemNum].name, itemQuantity);
            Item itemTransferTo = new Item(invSrc.contentList[itemNum].strenght, invSrc.contentList[itemNum].vitality, invSrc.contentList[itemNum].intelegence, invSrc.contentList[itemNum].agility, invSrc.contentList[itemNum].value, invSrc.contentList[itemNum].name, itemQuantity - invSrc.RemoveItem(itemTransferSrc).quantity);
            
            if (inv.Length == 2)
            {
                for (int i = 0; i < inv.Length; i++)
                {
                    if (inv[i] != invSrc)
                    {
                        inv[i].AddItem(itemTransferTo);
                    }
                }
            }
            else
            {
                Console.WriteLine("Inventory target Number");
                inv[Clamp(0, inv.Length, InputNumberInt() - 1)].AddItem(itemTransferTo);
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

        public Item itemZero = new Item(0, 0, 0, 0, 0, "0", 0);

        public int Clamp(int min, int max, int num)
        {
            return Math.Max(min, Math.Min(max, num));
        }
    }


    class NameGenerator
    {
        char[] vokal = new char[6] { 'e', 'y', 'u', 'i', 'o', 'a' };
        char[] konsonant = new char[20] { 'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
        string[] title = new string[9] { "Slayer", "Destroyer", "Defender", "Protector", "Maester", "Professor", "Escapist", "Assasin", "Lord" };

        
        Random r = new Random();
        bool singleName;

        public NameGenerator(bool singleNameSet = false, int debugSeed = -1)
        {
            
            if(debugSeed > 0)
            {
                r = new Random(debugSeed);
                singleName = singleNameSet;
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

            if (r.Next(100) > 85 && !singleName)
            {
                name += " " + GenName(); ;
            }

            if (r.Next(100) > 75 && !singleName)
            {
                name += " the " + title[r.Next(title.Length)];
            }

            if (r.Next(100) > 85 && !singleName)
            {
                name += " of " + GenName();
            }

            return name;
        }
    }

    class ActorRole
    {
        string role;
        int[] sts;

        string[] roles = new string[5] {"Jack", "Warrior","Tank","Mage","Scout"};

        public string RoleNameGet()
        {
            return role;
        }

        public int[] RoleStatsGet()
        {
            return sts;
        }

        public ActorRole(string roleSet = null)
        {
            if (roleSet != null)
            {
                role = roleSet;
            } else
            {
                role = roles[new Random().Next(0, roles.Length)];
            }

            switch (role)
            {
                case "None":
                case "Jack":
                default:
                    role = "Trades Jack";
                    sts = new int[4] { 1, 1, 1, 1 };
                    break;

                case "Warrior":
                    sts = new int[4] { 3, 1, 1, 1 };
                    break;

                case "Tank":
                    sts = new int[4] { 1, 3, 1, 1 };
                    break;

                case "Mage":
                    sts = new int[4] { 1, 1, 3, 1 };
                    break;

                case "Scout":
                    sts = new int[4] { 1, 1, 1, 3 };
                    break;
            }
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
                        int atkC = team_player[team, player].strenght;
                        int defC = team_player[team, player].vitality;
                        int splC = team_player[team, player].intelligence;
                        int aglC = team_player[team, player].agility;

                        while (aglC > 0) {
                            if (team_player[team, player].isAI)
                            {
                                Random r = new Random();


                                int rn = r.Next(atkC + defC + splC);

                                if (rn >= atkC + defC)
                                {

                                }
                                else if (rn >= atkC)
                                {

                                }
                                else
                                {

                                }

                            }
                            else
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
                                            Q.ErrorInput("", new string[0] { });
                                            break;
                                    }
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

        public MapManager(Lexicon getQ)
        {
            Q = getQ;
            map = new Map(40,40);
        }

        public Map map;

        void SetCurrentMap(Map mapSet)
        {
            map = mapSet;
        }

        public void MoveEntity(Entity ent, int[] walkLng)
        {
            if (map.GetTile(ent.pos[Q.GetX()] + walkLng[Q.GetX()], ent.pos[Q.GetY()] + walkLng[Q.GetY()]).walkable)
            {
                ent.pos[Q.GetX()] += walkLng[Q.GetX()];
                ent.pos[Q.GetY()] += walkLng[Q.GetY()];
            } else
            {
                Console.WriteLine("Tile out of Bounds");
            }
        }

        public void RendMap(int[] plrPos)
        {
            int viewDistance = 5;
            Console.Clear();
            Console.WriteLine();
            for (int y = plrPos[1] - viewDistance; y < plrPos[1] + (viewDistance +1); y++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("                    ");
                for (int x = plrPos[0] - viewDistance; x < plrPos[0] + (viewDistance + 1); x++)
                {

                    if (y == plrPos[1] && x == plrPos[0])
                    {
                        map.GetTile(x, y).Rend("@");
                    }
                    else
                    {
                        map.GetTile(x, y).Rend();
                    }
                }
                Console.WriteLine();
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

    }

    class Map
    {
        public int width, height;
        Tile[,] map;

        public Map(int widthSet, int heightSet)
        {
            width = widthSet;
            height = heightSet;
            map = new Tile[widthSet, heightSet];
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                for (int x = 0; x < map.GetUpperBound(0); x++)
                {
                    map[x, y] = new Tile(new int[2] { x,y }, false, "UNSET");

                    /*
                    if (x == 0 || x == map.GetUpperBound(0)-1 || y == 0 || y == map.GetUpperBound(1)-1)
                    {
                        map[x, y] = new Tile();
                    }
                    */
                }
            }
        }

        public void MapGen(int landMass, int[] structures)
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                map[x, 0] = new Tile(new int[2] { x, 0 });
                map[x, map.GetUpperBound(1)] = new Tile(new int[2] { x, map.GetUpperBound(1) });
            }

            for (int y = 0; y < map.GetUpperBound(0); y++)
            {
                map[0, y] = new Tile(new int[2] { 0, y });
                map[map.GetUpperBound(0), y] = new Tile(new int[2] { map.GetUpperBound(0), y });
            }

            int[] genPos = new int[2] { (width / 2), (height / 2) };
            Tile[] genTiles = new Tile[landMass];
            
            Random r = new Random();

            while (landMass > 0)
            {   

                if (GetTile(genPos[0], genPos[1]).icon == "UNSET")
                {
                    
                    map[genPos[0], genPos[1]] = new Tile(new int[2] { genPos[0], genPos[1] }, true, " ", ConsoleColor.DarkGreen);
                    genTiles[genTiles.Length - landMass] = map[genPos[0], genPos[1]];

                    landMass--;
                }

                int dim = r.Next(2);
                genPos[dim] = genTiles[r.Next((genTiles.Length - 1) - landMass)].pos[dim] + r.Next(-1, 2);

            }

            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                for (int x = 0; x < map.GetUpperBound(0); x++)
                {
                    if (GetTile(x, y).icon == "UNSET")
                    {       
                        map[x, y] = new Tile(new int[2] { x, y });
                    }
                }
            }
        }


        public Tile GetTileAll(int x, int y)
        {
            return map[Math.Max(0, Math.Min(map.GetUpperBound(0) -1, x)), Math.Max(0, Math.Min(map.GetUpperBound(1) -1, y))];
        }

        public Tile GetTile(int x, int y)
        {
            return map[Math.Max(1, Math.Min(map.GetUpperBound(0) - 2, x)), Math.Max(1, Math.Min(map.GetUpperBound(1) - 2, y))];
        }
    }

    class Tile
    {
        public string icon = "~";
        ConsoleColor clr = ConsoleColor.Blue;
        public int[] pos = new int[2];

        public bool walkable = false;

        public Tile(int[] posSet, bool walkableSet = false, string iconSet = "~", ConsoleColor clrSet = ConsoleColor.Blue)
        {
            Array.Copy(posSet, pos, 2);
            walkable = walkableSet;
            icon = iconSet;
            clr = clrSet;
        }

        public void Rend(string iconRend = "")
        {
            if (iconRend == "")
            {
                iconRend = icon;
            }
            Console.BackgroundColor = clr;
            if (walkable)
            {
                Console.Write("|_" + iconRend + "_");
            } else
            {
                Console.Write("  " + iconRend + " ");
            }
        }

    }

    //ENTITY
    class Entity
    {
        public Lexicon Q;

        public Entity(Lexicon getQ)
        {
            Q = getQ;
        }

        public int[] pos = new int[2] {0,0};

        public void MapPosSet(int x, int y)
        {
            pos[0] = x;
            pos[1] = y;
        }

        public string WriteName()
        {
            return name;
        }

        public string name;
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

        public int strenght = 1;
        public int vitality = 1;
        public int intelligence = 1;
        public int agility = 1;
        public int[] sts = new int[4] { 1, 1, 1, 1 };
        public ActorRole role = new ActorRole("None");

        int xp;
        public int lvl = 1;

        Inventory inventory = new Inventory();
        Weapon weapon;
        Armor armor;

        public void Lvl(int amount)
        {
            xp += amount;
            Console.WriteLine("+" + amount + "xp");

            if (xp >= 100 * lvl)
            {
                xp -= 100 * lvl;
                lvl++;

                Console.WriteLine("LVL UP! [lvl: " + lvl + "]");

                if (isAI)
                {
                    StatsGen(2);
                }
                else
                {
                    StatsSet(2);
                }
            }
        }

        public void ActorSetup(int lvlSet = 1, bool isAISet = true)
        {
            isAI = isAISet;
            if (isAI)
            {
                NameSet(new NameGenerator().GenName());
                lvl = lvlSet;
                role = new ActorRole();
                StsSet(role.RoleStatsGet());
                StatsGen(5 + lvl * 2);
            } else
            {
                Console.WriteLine("Set Name");
                NameSet(Q.InputText());

                lvl = lvlSet;
                StatsSet(5 + lvl * 2);
            }
        }

        public void StsSet(int[] stsSet)
        {
            sts[0] = stsSet[0];
            sts[1] = stsSet[1];
            sts[2] = stsSet[2];
            sts[3] = stsSet[3];
        }

        public void StatsSet(int skillPoints)
        {
            int skillAmountSet = 0;

            while (skillPoints > 0)
            {
                Console.WriteLine("Distribute Skillpoints: [S]renght, [V]itality, [I]ntelegence, [A]gility");
                Console.WriteLine("Skillpoints: " + skillPoints);
                Console.WriteLine(WriteStats());
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

        public string WriteLvl()
        {
            return "[lvl: " + lvl + "]";
        }

        public string WriteStats()
        {
            return "[Strength: " + strenght + " | Vitality: " + vitality + " | Intelegence: " + intelligence + " | Agility: " + agility + "]";
        }

        public string WriteActor()
        {
            return WriteName() + " " + WriteLvl() + " (" + role.RoleNameGet() + ") \n" + WriteStats();
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
        public string name;
        public int quantity;
        public int value;

        public int strenght;
        public int vitality;
        public int intelegence;
        public int agility;

        public Item(int s, int v, int i, int a, int val, string nameSet, int q = 1)
        {
            name = nameSet;
            value = val;
            quantity = q;

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
        public Armor(int s, int v, int i, int a, int val, string nameSet, int q = 1) : base(s, v, i, a, val, nameSet, q = 1)
        {
            name = nameSet;
            value = val;
            quantity = q;

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
        public Weapon(int s, int v, int i, int a, int val, string nameSet, int q = 1) : base(s, v, i, a, val, nameSet, q = 1)
        {
            name = nameSet;
            value = val;
            quantity = q;

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
        public Rune(int s, int v, int i, int a, int val, string nameSet, int q = 1) : base(s, v, i, a, val, nameSet, q = 1)
        {
            name = nameSet;
            value = val;
            quantity = q;

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
        int slots = 6;
        int money = 0;
        public Dictionary<string, Item> content = new Dictionary<string, Item>();
        public Item[] contentList;

        public Item AddItem(Item itemAdd)
        {
            if (itemAdd != null)
            {
                if (content.Count <= slots)
                {
                    if (content.ContainsKey(itemAdd.name))
                    {
                        content[itemAdd.name].quantity += itemAdd.quantity;
                    }
                    else
                    {
                        if (itemAdd.quantity > 0)
                        {
                            content.Add(itemAdd.name, itemAdd);
                        }
                    }
                    ContentToList();
                    return ItemZero();
                }
                else
                {
                    Console.WriteLine("Inventory full");
                    ContentToList();
                    return itemAdd;
                }
            }

            ContentToList();
            return ItemZero();
        }

        /*
        public Item RemoveItem(string itemGetName, int amount)
        {
            if (content.ContainsKey(itemGetName))
            {
                if (content[itemGetName].quantity - amount > 0)
                {
                    content[itemGetName].quantity -= amount;
                }
                else
                {
                    Item ret = content[itemGetName];
                    content.Remove(itemGetName);
                    return ret;
                }
            }
            return null;
        }
        //*/

        //*
        public Item RemoveItem(Item itemRem)
        {
            if (content.ContainsKey(itemRem.name))
            {
                if (content[itemRem.name].quantity - itemRem.quantity > 0)
                {    
                    content[itemRem.name].quantity -= itemRem.quantity;

                    ContentToList();
                    return ItemZero();
                }
                else
                {
                    itemRem.quantity -= content[itemRem.name].quantity;
                    content.Remove(itemRem.name);
                    ContentToList();
                    return itemRem;
                }
            }

            ContentToList();
            return itemRem;
        }
        //*/

        public int MoneyTransfer(int amount)
        {
            if (money + amount >= 0)
            {
                money += amount;
            } else {
                money = 0;
                return money + amount;
            }

            return 0;
        }

        void ContentToList()
        {
            contentList = new Item[content.Count];
            int i = 0;
            foreach (KeyValuePair<string, Item> de in content)
            {
                contentList[i] = de.Value;
                i++;
            }
        }

        public void WriteContent()
        {
            Console.WriteLine("Money: " + money + "$");
            int j = 0;
            foreach (KeyValuePair<string, Item> de in content)
            {
                j++;
                Item i = de.Value;
                Console.WriteLine(j + ": " + i.name + " [" + i.quantity + "] [" + i.value + "$] " + i.strenght + " " + i.vitality + " " + i.intelegence + " " + i.vitality);
            }
            Console.WriteLine();
        }

        Item ItemZero()
        {
            return new Item(0, 0, 0, 0, 0, "0", 0);
        }
    }
}
