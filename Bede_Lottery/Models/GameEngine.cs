using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bede_Lottery.Models
{
    public class GameEngine
    {
        public decimal TicketCost { get; set; }
        public decimal PlayerStartingBalance { get; set; }
        public int MinTicketCount { get; set; }
        public int MaxTicketCount { get; set; }
        public int HumanPlayers { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public List<Player> Players { get { return _players; } }
        public List<Ticket> Tickets { get { return _tickets; } }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get { return _totalRevenue; } }
        public decimal RemainingRevenue { get { return _remainingRevenue; } }

        private static int _ticketId = 1;
        private static Random _random = new Random();
        private List<Player> _players = new List<Player>();
        private List<Ticket> _tickets = new List<Ticket>();
        private decimal _totalRevenue = 0;
        private decimal _remainingRevenue = 0;

        public GameEngine(decimal ticketCost, decimal playerStartingBalance, int minTicketCount, int maxTicketCount, int humanPlayers, int minPlayers, int maxPlayers)
        {
            this.TicketCost = ticketCost;
            this.PlayerStartingBalance = playerStartingBalance;
            this.MinTicketCount = minTicketCount;
            this.MaxTicketCount = maxTicketCount;
            this.HumanPlayers = humanPlayers;
            this.MinPlayers = minPlayers;
            this.MaxPlayers = maxPlayers;
        }

        public static int NextTicketID() { return _ticketId++; }
        
        /// <summary>
        /// Create all of the human players and CPU players. Human player count and over all max players count will follow the properties set in the constructor
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void AddPlayersToGame()
        {
            if (HumanPlayers > MaxPlayers) { throw new Exception("There are too many human players to fit in this game."); }

            int targetPlayerCount = _random.Next(MinPlayers, MaxPlayers);

            //Loop and create all the players, including both human and CPU players           
            for (int i = 0; i < targetPlayerCount; i++)
            {
                int playerID = i + 1;
                string playerName = string.Format("Player {0}", i + 1);
                Player player;

                if (i < HumanPlayers)
                {
                    player = new HumanPlayer(playerID, playerName, PlayerStartingBalance, 18, "GB", "Eng", false);
                }
                else
                {
                    player = new CPU(playerID, playerName, PlayerStartingBalance, 1);
                }

                _players.Add(player);
            }
        }

        /// <summary>
        /// Display to the screen the introduction welcome text
        /// </summary>
        /// <param name="players"></param>
        public void ShowWelcomeText(List<Player> players)
        {
            //Welcome info
            StringBuilder playerNames = new StringBuilder();
            foreach (Player p in players)
            {
                playerNames.Append(p.Name + ", ");
            }
            Console.WriteLine(string.Format("Welcome to the Bede Lottery, {0}!", playerNames.ToString().TrimEnd(' ').TrimEnd(',')));
            Console.WriteLine();

            Console.WriteLine(string.Format("* Your digital balance: ${0}", PlayerStartingBalance.ToString()));
            Console.WriteLine(string.Format("* Ticket Price: ${0} each", TicketCost));
            Console.WriteLine();
        }

        /// <summary>
        /// Display to the screen a list of all of the players and the number of tickets each have them bought
        /// </summary>
        public void ShowAllPlayerAndTickets()
        {
            foreach (Player player in _players)
            {
                Console.WriteLine(string.Format("{0} has purchased {1} tickets.", player.Name, player.Tickets.Count().ToString()));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Using the number of tickets sold, calculate the revenue for this game
        /// </summary>
        public void CalculateRevenue()
        {
            _totalRevenue = _tickets.Count * TicketCost;
            _remainingRevenue = _totalRevenue;
        }

        public void MarkWinningTickets(List<Ticket> winningTickets)
        {
            foreach(Ticket ticket in winningTickets)
            {
                _tickets.Remove(ticket);
            }
        }

        public void UpdateRemainingRevenue(decimal deduction)
        {
            _remainingRevenue = _remainingRevenue - deduction;
        }
    }
}
