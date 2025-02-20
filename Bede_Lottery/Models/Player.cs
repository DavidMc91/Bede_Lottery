using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bede_Lottery.Models
{
    /// <summary>
    /// Abract class defining a player. Derived classes are currently human player or computer player
    /// </summary>
    public abstract class Player
    {      
        public int Id { get; }
        public string Name { get; set; }
        public List<Ticket> Tickets { get; set; }
        public decimal BankBalance { get; set; }

        protected Player(int id, string name, decimal bankBalance)
        {
            Id = id;
            Name = name;
            BankBalance = bankBalance;
        }

        public abstract void GetDesiredTicketCount(int minTicketCount, int maxTicketCount);
        public abstract void PurchaseTickets(string ticketName, decimal ticketCost);
    }

    public class HumanPlayer : Player
    {
        public int Age { get; set; }
        public string IsoCountryCode { get; set; }
        public string Language { get; set; }
        public bool SafeGambling_Enabled { get; set; }
        public int TicketCount_Wanted { get; set; }

        public HumanPlayer(int id, string name, decimal bankBalance, int age, string isoCountryCode, string language, bool safeGambling_Enabled) : base(id, name, bankBalance)
        {
            Age = age;
            IsoCountryCode = isoCountryCode;
            Language = language;
            SafeGambling_Enabled = safeGambling_Enabled;
        }

        public override void GetDesiredTicketCount(int minTicketCount, int maxTicketCount)
        {
            int specifiedTicketsWanted = 0;

            //Customer buying tickets
            Console.WriteLine(string.Format("How many tickets do you want to buy, {0}?", Name));
            while (true)
            {
                try
                {
                    specifiedTicketsWanted = Convert.ToInt32(Console.ReadLine());
                    if (specifiedTicketsWanted < minTicketCount || specifiedTicketsWanted > maxTicketCount)
                    {
                        Console.WriteLine(string.Format("Please enter a number within {0} and {1}.",minTicketCount,maxTicketCount));
                    }
                    else
                    {
                        TicketCount_Wanted = specifiedTicketsWanted;
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("This is not a valid number. Please try again.");
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            Console.WriteLine();
        }

        public override void PurchaseTickets(string ticketName, decimal ticketCost)
        {
            Tickets = new List<Ticket>();
            for (int i = 0; i < TicketCount_Wanted; i++)
            {
                if (BankBalance >= ticketCost)
                {
                    Ticket ticket = new Ticket(GameEngine.NextTicketID(), ticketName, ticketCost, Id);
                    Tickets.Add(ticket);
                    BankBalance = BankBalance - ticketCost;
                }
                else 
                {
                    Console.WriteLine(String.Format("You have only purchased {0} as your bank balance could not cover more.", i));    
                    break; 
                }            
            }
        }
    }

    public class CPU : Player
    {
        public int DifficultyLevel { get; set; }
        public int TicketCount_Wanted { get; set; }
        private static Random _random = new Random(); //This is used for desired ticket amount. Due to the quick succession in which the desired tickets are produced, this needs to be one instance as using a fresh instance each time will use the time of day as a seed and produce the same number each time

        public CPU(int id, string name, decimal bankBalance, int difficultyLevel) : base(id, name, bankBalance)
        {
           DifficultyLevel = difficultyLevel;

        }

        public override void GetDesiredTicketCount(int minTicketCount, int maxTicketCount)
        {
            TicketCount_Wanted = _random.Next(minTicketCount, maxTicketCount);
        }

        public override void PurchaseTickets(string ticketName, decimal ticketCost)
        {
            Tickets = new List<Ticket>();
            for (int i = 0; i < TicketCount_Wanted; i++)
            {
                if (BankBalance >= ticketCost)
                {
                    Ticket ticket = new Ticket(GameEngine.NextTicketID(), ticketName, ticketCost, Id);
                    Tickets.Add(ticket);
                    BankBalance = BankBalance - ticketCost;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
