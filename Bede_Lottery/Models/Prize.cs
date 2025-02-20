using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bede_Lottery.Models
{
    /// <summary>
    /// Abstract class defining a prize. Derived classes are currently money but could be a different reward in future (e.g. a speedboat :D )
    /// </summary>
    public abstract class Prize
    {       
        public int Id { get; set; }
        public String Name { get; set; }

        public List<Ticket> WinningTickets { get; set; }

        protected Prize(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract int NumberOfWinners_Get(int ticketsSold);
        public abstract List<Ticket> DetermineWinners(List<Ticket> tickets, int numberOfWinners);
        public abstract decimal DetermineWinnings(decimal totalRevenue);
        public abstract string WinningPlayers_ToString();
    }

    /// <summary>
    /// The reward for this prize is money and the number of winners is specified as an integer
    /// </summary>
    public class Prize_Money_IntWinners : Prize
    {      
        public int NumberOfWinners { get; set; }
        public decimal WinningsPercentage { get; set; }
        public decimal PrizeMoney { get; set; }
        public decimal PrizeMoney_PerPlayer { get; set; }
        private static Random _random = new Random();

        public Prize_Money_IntWinners(int id, string name, int numberOfWinners, decimal winningsPercentage) : base(id, name)
        {
            NumberOfWinners = numberOfWinners;
            WinningsPercentage = winningsPercentage;
        }

        public decimal Return_Prize_MaxAmount(decimal revenue)
        {
            decimal winnings = revenue * WinningsPercentage;
            return Math.Round(winnings, 2);
        }

        public override int NumberOfWinners_Get(int ticketsSold)
        {
            return NumberOfWinners;
        }

        public override List<Ticket> DetermineWinners(List<Ticket> tickets, int numberOfWinners)
        {
            List<Ticket> winningTickets = new List<Ticket>();

            //Choose x number of tickets at random, these are the winning tickets 
            for (int i = 0; i < numberOfWinners; i++)
            {
                //Randomly choose a ticket index in the list of tickets we have
                int ticketIndex = _random.Next(0, tickets.Count - 1);
                //Add this randomly chosen to ticket to the winning tickets list
                winningTickets.Add(tickets[ticketIndex]);
                //Remove this winning ticket from the ticket pool so we don't choose it again (winning tickets can't again in a different tier)
                tickets.RemoveAt(ticketIndex);
            }

            return winningTickets;
        }


        public override decimal DetermineWinnings(decimal totalRevenue)
        {
            decimal tier_MaxPrizeMoney = Return_Prize_MaxAmount(totalRevenue); //Get the max amount we can give away for this tier...
            decimal prizePP_BeforeRounding = tier_MaxPrizeMoney / NumberOfWinners; //...divide it by number of players...
            decimal actual_PrizePP = Math.Floor(prizePP_BeforeRounding * 100) / 100; //..round down on the third decimal place (if we allow it to round up, it can result in the prize per player exceeding the max prize amount)...
            decimal actual_PrizeMoney = actual_PrizePP * NumberOfWinners; //...use the rounded amount per person to work out the actual total amount we are giving away for this tier

            //This should be handled more gracefully but for this demo code, just throw an error
            if (actual_PrizeMoney > tier_MaxPrizeMoney)
            {
                throw new Exception("Error calculating Prize Money. We are giving more than we have for this prize tier.");
            }

            PrizeMoney = actual_PrizeMoney;
            PrizeMoney_PerPlayer = actual_PrizePP;
            
            return actual_PrizeMoney;
        }

        public override string WinningPlayers_ToString()
        {
            string sWinningPlayers = string.Empty;
            StringBuilder sbWinningPlayers = new StringBuilder();
            foreach(Ticket ticket in WinningTickets)
            {
                sbWinningPlayers.Append(ticket.PlayerID.ToString() + ", ");
            }
            sWinningPlayers = sbWinningPlayers.ToString().TrimEnd(' ').TrimEnd(',');

            if (WinningTickets.Count > 1)
            {
                return string.Format("* {0}: Players {1} win {2} each!", Name, sWinningPlayers, PrizeMoney_PerPlayer.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
            else if( WinningTickets.Count == 1) 
            {
                return string.Format("* {0}: Player {1} wins {2}!", Name, sWinningPlayers, PrizeMoney_PerPlayer.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
            else
            {
                return string.Format("* {0}: There were no winners this time!", Name);
            }
        }
    }

    /// <summary>
    /// The reward for this prize is money and the number of winners is a percentage of total tickets sold
    /// </summary>
    public class Prize_Money_PercentageWinners : Prize
    {
        public int NumberOfWinners { get; set; }
        public decimal PercentageOfWinners { get; set; }
        public decimal WinningsPercentage { get; set; }
        public decimal PrizeMoney { get; set; }
        public decimal PrizeMoney_PerPlayer { get; set; }
        private static Random _random = new Random();


        public Prize_Money_PercentageWinners(int id, string name, decimal percentOfWinners, decimal winningsPercentage) : base(id, name)
        {
            PercentageOfWinners = percentOfWinners;
            WinningsPercentage = winningsPercentage;
        }

        public decimal Return_Prize_MaxAmount(decimal revenue)
        {
            decimal winnings = revenue * WinningsPercentage;
            return Math.Round(winnings, 2);
        }

        public override int NumberOfWinners_Get(int ticketsSold)
        {
            NumberOfWinners = Convert.ToInt32(Math.Round(ticketsSold * PercentageOfWinners, MidpointRounding.AwayFromZero));
            return NumberOfWinners;
        }

        public override List<Ticket> DetermineWinners(List<Ticket> tickets, int numberOfWinners)
        {
            List<Ticket> winningTickets = new List<Ticket>();

            //Choose x number of tickets at random, these are the winning tickets 
            for (int i = 0; i < numberOfWinners; i++)
            {
                //Randomly choose a ticket index in the list of tickets we have
                int ticketIndex = _random.Next(0, tickets.Count - 1);
                //Add this randomly chosen to ticket to the winning tickets list
                winningTickets.Add(tickets[ticketIndex]);
                //Remove this winning ticket from the ticket pool so we don't choose it again (winning tickets can't again in a different tier)
                tickets.RemoveAt(ticketIndex);
            }

            return winningTickets;
        }

        public override decimal DetermineWinnings(decimal totalRevenue)
        {
            decimal tier_MaxPrizeMoney = Return_Prize_MaxAmount(totalRevenue); //Get the max amount we can give away for this tier...
            decimal prizePP_BeforeRounding = tier_MaxPrizeMoney / NumberOfWinners; //...divide it by number of players...
            decimal actual_PrizePP = Math.Floor(prizePP_BeforeRounding * 100) / 100; //..round down on the third decimal place (if we allow it to round up, it can result in the prize per player exceeding the max prize amount)...
            decimal actual_PrizeMoney = actual_PrizePP * NumberOfWinners; //...use the rounded amount per person to work out the actual total amount we are giving away for this tier

            //This should be handled more gracefully but for this demo code, just throw an error
            if (actual_PrizeMoney > tier_MaxPrizeMoney)
            {
                throw new Exception("Error calculating Prize Money. We are giving more than we have for this prize tier.");
            }

            PrizeMoney = actual_PrizeMoney;
            PrizeMoney_PerPlayer = actual_PrizePP;

            return actual_PrizeMoney;
        }

        public override string WinningPlayers_ToString()
        {
            string sWinningPlayers = string.Empty;
            StringBuilder sbWinningPlayers = new StringBuilder();
            foreach (Ticket ticket in WinningTickets)
            {
                sbWinningPlayers.Append(ticket.PlayerID.ToString() + ", ");
            }
            sWinningPlayers = sbWinningPlayers.ToString().TrimEnd(' ').TrimEnd(',');

            if (WinningTickets.Count > 1)
            {
                return string.Format("* {0}: Players {1} win {2} each!", Name, sWinningPlayers, PrizeMoney_PerPlayer.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
            else if (WinningTickets.Count == 1)
            {
                return string.Format("* {0}: Player {1} wins {2}!", Name, sWinningPlayers, PrizeMoney_PerPlayer.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
            else
            {
                return string.Format("* {0}: There were no winners this time!", Name);
            }
        }
    }
}
