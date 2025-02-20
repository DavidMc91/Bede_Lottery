using Bede_Lottery.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bede_Lottery_UnitTests
{
    [TestClass]
    public sealed class GameEngineTests
    {
        [TestMethod]
        public void NextTicketID_RequestNewID_IncrementingID()
        {
            // Arrange
            int initialTicketID = GameEngine.NextTicketID();

            // Act
            int secondTicketID = GameEngine.NextTicketID();

            // Assert
            Assert.IsTrue(secondTicketID > initialTicketID); // ticket ID is incrementing
        }

        [TestMethod]
        public void CreatePlayers_10PlayersRequested_10PlayersCreated()
        {
            // Arrange
            GameEngine lotteryGame = new GameEngine(1.00m, 10.00m, 10, 10, 1, 10, 10);

            // Act
            lotteryGame.AddPlayersToGame();

            // Assert
            Assert.AreEqual(10, lotteryGame.Players.Count()); // Expected 10 players created
        }

        [TestMethod]
        public void CalculateRevenue_100ticketsSold_CorrectRevenue()
        {
            // Arrange
            GameEngine lotteryGame = new GameEngine(1.00m, 10.00m, 10, 10, 0, 10, 10);
            //Create players
            lotteryGame.AddPlayersToGame();
            //Sell tickets
            foreach (Player player in lotteryGame.Players)
            {
                player.GetDesiredTicketCount(lotteryGame.MinTicketCount, lotteryGame.MaxTicketCount);
                player.PurchaseTickets("standard", lotteryGame.TicketCost);
                lotteryGame.Tickets.AddRange(player.Tickets);
            }

            // Act
            lotteryGame.CalculateRevenue();

            // Assert
            Assert.IsTrue(lotteryGame.TotalRevenue == 100); //Should equal 100
        }

        [TestMethod]
        public void UpdateRemainingRevenue_ReduceRevenueBy10_RevenueReducedBy10()
        {
            // Arrange
            GameEngine lotteryGame = new GameEngine(1.00m, 10.00m, 10, 10, 0, 10, 10);
            //Create players
            lotteryGame.AddPlayersToGame();
            //Sell tickets
            foreach (Player player in lotteryGame.Players)
            {
                player.GetDesiredTicketCount(lotteryGame.MinTicketCount, lotteryGame.MaxTicketCount);
                player.PurchaseTickets("standard", lotteryGame.TicketCost);
                lotteryGame.Tickets.AddRange(player.Tickets);
            }
            lotteryGame.CalculateRevenue();

            // Act
            lotteryGame.UpdateRemainingRevenue(10);

            // Assert
            Assert.IsTrue(lotteryGame.RemainingRevenue == 90); //Should equal 90
        }

        [TestMethod]
        public void MarkWinningTickets_RemoveAWinningTicket_TicketIsRemoved()
        {
            // Arrange
            GameEngine lotteryGame = new GameEngine(1.00m, 10.00m, 10, 10, 0, 10, 10);
            //Create players
            lotteryGame.AddPlayersToGame();
            //Sell tickets
            foreach (Player player in lotteryGame.Players)
            {
                player.GetDesiredTicketCount(lotteryGame.MinTicketCount, lotteryGame.MaxTicketCount);
                player.PurchaseTickets("standard", lotteryGame.TicketCost);
                lotteryGame.Tickets.AddRange(player.Tickets);
            }
            //Choose our winning ticket
            List<Ticket> winningTicket = new List<Ticket>();
            winningTicket.Add(lotteryGame.Tickets.First());

            // Act
            lotteryGame.MarkWinningTickets(winningTicket);

            // Assert
            Assert.IsFalse(lotteryGame.Tickets.Contains(winningTicket[0])); //Ticket list should not contain winning ticket
        }       
    }
}
