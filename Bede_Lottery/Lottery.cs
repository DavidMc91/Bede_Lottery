using Bede_Lottery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bede_Lottery
{
    internal class Lottery
    {

        static void Main(string[] args)
        {
            //Configuration variables
            decimal ticketCost = 1.00m;
            decimal playerStartingBalance = 10.00m;
            int minTicketsCount = 1;
            int maxTicketsCount = 10;
            int humanPlayerCount = 1;
            int minPlayerCount = 10;
            int maxPlayerCount = 15;
            List<Prize> prizeTiers = new List<Prize>
            {
                new Prize_Money_IntWinners(1, "Grand Prize", 1, 0.50m),
                new Prize_Money_PercentageWinners(2, "Second Tier", 0.10m, 0.30m),
                new Prize_Money_PercentageWinners(3, "Third Tier", 0.20m, 0.10m)
            };


            //Create an instance of the GameEngine and pass in the config values for things like: number of players, ticket cost, etc
            GameEngine lotteryGame = new GameEngine(ticketCost, playerStartingBalance, minTicketsCount, maxTicketsCount, humanPlayerCount, minPlayerCount, maxPlayerCount);
                  
            //Create all the players using values already set within the game engine
            lotteryGame.AddPlayersToGame();

            //Show a welcome message that addresses each human player
            lotteryGame.ShowWelcomeText(lotteryGame.Players.Where(p => p.GetType() == typeof(HumanPlayer)).ToList());

            //Sell tickets
            foreach (Player player in lotteryGame.Players)
            {
                player.GetDesiredTicketCount(lotteryGame.MinTicketCount, lotteryGame.MaxTicketCount);
                player.PurchaseTickets("standard", lotteryGame.TicketCost);
                lotteryGame.Tickets.AddRange(player.Tickets);
            }
            //Note how many tickets were sold in total as the ticket list will be modified later, once winners are drawn
            lotteryGame.TotalTicketsSold = lotteryGame.Tickets.Count();

            //Show how many CPU players there are
            Console.WriteLine(string.Format("{0} other CPU players also have purchased tickets.", lotteryGame.Players.Where(p => p.GetType() == typeof(CPU)).Count().ToString()));
            Console.WriteLine();

            //Show all players and the number of tickets each purchased:
            lotteryGame.ShowAllPlayerAndTickets();

            //Using the number of tickets sold, calculate the revenue for this game
            lotteryGame.CalculateRevenue();
           
            //Loop each prize. Determine the winners and allocate the winnings
            Console.WriteLine("Ticket Draw Results:");
            Console.WriteLine("");
            foreach (Prize prize in prizeTiers)
            {
                //Draw the winners of this tier
                prize.WinningTickets = prize.DetermineWinners(lotteryGame.Tickets, prize.NumberOfWinners_Get(lotteryGame.TotalTicketsSold));

                //Remove the winning tickets from the pot so a single ticket can't win twice
                lotteryGame.MarkWinningTickets(prize.WinningTickets);

                //Allocate the reward for this prize tier and update the remaining revenue
                lotteryGame.UpdateRemainingRevenue(prize.DetermineWinnings(lotteryGame.TotalRevenue));

                //Display the winners for this prize tier
                Console.WriteLine(prize.WinningPlayers_ToString());
            }
            Console.WriteLine();

            Console.WriteLine("Congratulations to the winners!");
            Console.WriteLine();

            Console.WriteLine(string.Format("House Revenue: ${0}", lotteryGame.RemainingRevenue));
            Console.ReadKey();
        }
    }
}
