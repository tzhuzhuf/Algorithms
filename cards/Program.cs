using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public override string ToString()
        {
            return $"{Rank} {Suit}";
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public void ShowHand()
        {
            Console.WriteLine($"{Name}'s hand: {string.Join(", ", Hand)}");
        }

        public void RemoveCard(Card card)
        {
            Hand.Remove(card);
        }
    }

    static void Main(string[] args)
    {
        List<Card> deck = CreateDeck();
        ShuffleDeck(deck);

        Console.WriteLine("Введите количество игроков:");
        int playerCount = int.Parse(Console.ReadLine());
        List<Player> players = new List<Player>();

        for (int i = 1; i <= playerCount; i++)
        {
            Console.WriteLine($"Введите имя игрока {i}:");
            string playerName = Console.ReadLine();
            players.Add(new Player(playerName));
        }

        Console.WriteLine("Выберите козырь (Черви, Бубны, Трефы, Пики):");
        string trumpSuit = Console.ReadLine();

        DealCards(deck, players);

        PlayGame(players, trumpSuit, deck);
    }

    static List<Card> CreateDeck()
    {
        string[] suits = { "Черви", "Бубны", "Трефы", "Пики" };
        string[] ranks = { "6", "7", "8", "9", "10", "Валет", "Дама", "Король", "Туз" };
        List<Card> deck = new List<Card>();

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                deck.Add(new Card(suit, rank));
            }
        }

        return deck;
    }

    static void ShuffleDeck(List<Card> deck)
    {
        Random rand = new Random();
        for (int i = 0; i < deck.Count; i++)
        {
            int j = rand.Next(i, deck.Count);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    static void DealCards(List<Card> deck, List<Player> players)
    {
        for (int i = 0; i < 6; i++)
        {
            foreach (var player in players)
            {
                if (deck.Count > 0)
                {
                    player.Hand.Add(deck[^1]);
                    deck.RemoveAt(deck.Count - 1);
                }
            }
        }
        Console.WriteLine("Карты розданы!");
    }

    static void PlayGame(List<Player> players, string trumpSuit, List<Card> deck)
    {
        int currentPlayerIndex = 0;
        bool isGameOver = false;

        while (!isGameOver)
        {
            Player attacker = players[currentPlayerIndex];
            Player defender = players[(currentPlayerIndex + 1) % players.Count];

            Console.WriteLine($"\nХод игрока: {attacker.Name}");
            attacker.ShowHand();
            defender.ShowHand();

            Console.WriteLine("Выберите карту для атаки (введите номер):");
            int attackIndex = GetCardIndex(attacker);
            Card attackCard = attacker.Hand[attackIndex];
            attacker.RemoveCard(attackCard);
            Console.WriteLine($"{attacker.Name} атакует картой {attackCard}");

            Console.WriteLine("Выберите карту для защиты (введите номер):");
            int defenseIndex = GetCardIndex(defender);
            Card defenseCard = defender.Hand[defenseIndex];

            if (CanDefend(attackCard, defenseCard, trumpSuit))
            {
                Console.WriteLine($"{defender.Name} защищается картой {defenseCard}");
                defender.RemoveCard(defenseCard);
            }
            else
            {
                Console.WriteLine($"{defender.Name} не может защититься!");
                Console.WriteLine($"{attacker.Name} выиграл!");
                isGameOver = true;
                break;
            }

            DrawCards(attacker, deck);
            DrawCards(defender, deck);

            if (attacker.Hand.Count == 0)
            {
                Console.WriteLine($"{attacker.Name} выиграл!");
                isGameOver = true;
            }

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }

        Console.WriteLine("\nИгра завершена.");
    }

    static bool CanDefend(Card attackCard, Card defenseCard, string trumpSuit)
    {
        if (defenseCard.Suit == trumpSuit && attackCard.Suit != trumpSuit)
        {
            return true;
        }

        if (defenseCard.Suit == attackCard.Suit)
        {
            return GetRankValue(defenseCard.Rank) > GetRankValue(attackCard.Rank);
        }

        if (attackCard.Suit == trumpSuit)
        {
            return defenseCard.Suit == trumpSuit && GetRankValue(defenseCard.Rank) > GetRankValue(attackCard.Rank);
        }

        return false;
    }

    static int GetRankValue(string rank)
    {
        return rank switch
        {
            "6" => 0,
            "7" => 1,
            "8" => 2,
            "9" => 3,
            "10" => 4,
            "Валет" => 5,
            "Дама" => 6,
            "Король" => 7,
            "Туз" => 8,
            _ => -1,
        };
    }

    static void DrawCards(Player player, List<Card> deck, int maxCards = 6)
    {
        while (player.Hand.Count < maxCards && deck.Count > 0)
        {
            player.Hand.Add(deck[^1]);
            deck.RemoveAt(deck.Count - 1);
        }
    }

    static int GetCardIndex(Player player)
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= player.Hand.Count)
            {
                return index - 1;
            }
            Console.WriteLine("Неверный ввод. Попробуйте еще раз:");
        }
    }
}
