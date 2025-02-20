using Bede_Lottery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bede_Lottery_UnitTests
{
    [TestClass]
    public sealed class PlayerTests
    {
        [TestMethod]
        public void CPU_GetDesiredTicketCount_Request10_10IsStoredAsChoice()
        {
            // Arrange
            CPU player = new CPU(1, "player2", 10, 1);

            // Act
            player.GetDesiredTicketCount(10, 10);

            // Assert
            Assert.IsTrue(player.TicketCount_Wanted == 10); // Desired ticket count is 10
        }

        [TestMethod]
        public void CPU_PurchaseTickets_CanBuyWithinBudget_BuysDesiredAmountOfTickets()
        {
            // Arrange
            CPU player = new CPU(1, "player2", 10, 1);
            player.GetDesiredTicketCount(9, 9);

            // Act
            player.PurchaseTickets("standard", 1);

            // Assert
            Assert.IsTrue(player.Tickets.Count() == 9); // Desired ticket count is 9
        }

        [TestMethod]
        public void CPU_PurchaseTickets_Buy11ButNotEnoughBalance_OnlyBuys10()
        {
            // Arrange
            CPU player = new CPU(1, "player2", 10, 1);
            player.GetDesiredTicketCount(11, 11);

            // Act
            player.PurchaseTickets("standard", 1);

            // Assert
            Assert.IsTrue(player.Tickets.Count() == 10); // Desired ticket count is 10
        }


        [TestMethod]
        public void HumanPlayer_CanBuyWithinBudget_BuysDesiredAmountOfTickets()
        {
            // Arrange
            HumanPlayer player = new HumanPlayer(1, "Player 1", 10, 18, "GB", "Eng", false);
            player.TicketCount_Wanted = 9;

            // Act
            player.PurchaseTickets("standard", 1);

            // Assert
            Assert.IsTrue(player.Tickets.Count() == 9); // Desired ticket count is 9
        }


        [TestMethod]
        public void HumanPlayer_PurchaseTickets_Buy11ButNotEnoughBalance_OnlyBuys10()
        {
            // Arrange
            HumanPlayer player = new HumanPlayer(1, "Player 1", 10, 18, "GB", "Eng", false);
            player.TicketCount_Wanted = 11;

            // Act
            player.PurchaseTickets("standard", 1);

            // Assert
            Assert.IsTrue(player.Tickets.Count() == 10); // Desired ticket count is 10
        }

    }
}
