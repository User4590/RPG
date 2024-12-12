using System;
using System.Collections.Generic;

namespace RPGGame
{
    // Base Character class
    public abstract class Character
    {
        public string Name { get; private set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Character(string name, int health, int attack, int defense)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Defense = defense;
        }

        public virtual int AttackTarget(Character target)
        {
            int damage = Math.Max(1, Attack - target.Defense);
            target.Health = Math.Max(0, target.Health - damage);
            return damage;
        }
    }

    // Spiller class
    public class Player : Character
    {
        public List<string> Inventory { get; private set; }
        public int Level { get; private set; }
        public int Experience { get; set; }

        public Player(string name, string characterClass) : base(name, 0, 0, 0)
        {
            Inventory = new List<string>();
            Level = 1;
            Experience = 0;
                                         //Sætter Attack, Defense og HP værdier
            switch (characterClass)
            {
                case "Warrior": 
                    Health = 100;
                    Attack = 15;
                    Defense = 10;
                    break;
                case "Mage":
                    Health = 80;
                    Attack = 20;
                    Defense = 5;
                    break;
                case "Archer":
                    Health = 90;
                    Attack = 18;
                    Defense = 7;
                    break;
                default:
                    throw new ArgumentException("Invalid character class");
            }
        }

        public void LevelUp()
        {
            Level++;
            Health += 10;
            Attack += 2;
            Defense += 1;
            Console.WriteLine($"{Name} leveled up to level {Level}! Stats increased.");
        }

        public void AddItem(string item)
        {
            Inventory.Add(item);
            Console.WriteLine($"{item} added to inventory.");
        }
    }

    // Fjende class
    public class Enemy : Character
    {
        public Enemy(string name, int health, int attack, int defense) : base(name, health, attack, defense) { }
    }

    // NPC class
    public class NPC
    {
        public string Name { get; private set; }
        public string Quest { get; private set; }

        public NPC(string name, string quest)
        {
            Name = name;
            Quest = quest;
        }

        public void GiveQuest()
        {
            Console.WriteLine($"{Name}: {Quest}");
        }
    }

    // Gameplay funktioner
    public static class Game
    {
        public static void Combat(Player player, Enemy enemy)
        {
            Console.WriteLine($"A wild {enemy.Name} appears!");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine($"\n{player.Name}'s turn:");
                Console.WriteLine("1. Attack\n2. Defend\n3. Use potion");     //Viser valgmuligheder
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        int playerDamage = player.AttackTarget(enemy);
                        Console.WriteLine($"You dealt {playerDamage} damage to {enemy.Name}.");
                        break;
                    case "2":
                        Console.WriteLine("You brace yourself and reduce incoming damage.");
                        player.Defense += 5;
                        break;
                    case "3":
                        if (player.Inventory.Contains("Potion"))
                        {
                            player.Health += 20;
                            player.Inventory.Remove("Potion");
                            Console.WriteLine("You used a Potion and recovered 20 health.");
                        }
                        else
                        {
                            Console.WriteLine("You don't have any potions!");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. You miss your turn!");
                        break;
                }

                if (enemy.Health > 0)
                {
                    Console.WriteLine($"\n{enemy.Name}'s turn:");
                    int enemyDamage = enemy.AttackTarget(player);
                    Console.WriteLine($"{enemy.Name} dealt {enemyDamage} damage to you.");
                }

                player.Defense = Math.Max(player.Defense - 5, 0); // Reset defense boost
            }

            if (player.Health > 0)
            {
                Console.WriteLine($"\nYou defeated {enemy.Name}!");
                player.Experience += 50;
                if (player.Experience >= 100)
                {
                    player.Experience = 0;
                    player.LevelUp();
                }
            }
            else
            {
                Console.WriteLine($"\nYou were defeated by {enemy.Name}. Game Over.");
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the RPG Adventure!");
            Console.Write("Enter your character's name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Choose your class: Warrior, Mage, Archer");
            string characterClass = Console.ReadLine();

            try
            {
                Player player = new Player(name, characterClass);
                Console.WriteLine($"Welcome, {player.Name} the {characterClass}!");

                NPC npc = new NPC("Elder", "Defeat the goblin in the forest!");
                npc.GiveQuest();

                Enemy enemy = new Enemy("Goblin", health: 50, attack: 10, defense: 5);
                Combat(player, enemy);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}