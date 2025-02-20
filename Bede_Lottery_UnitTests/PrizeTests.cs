

using Bede_Lottery.Models;
using Microsoft.Win32;
using System.Net.Sockets;

namespace Bede_Lottery_UnitTests
{
    [TestClass]
    public sealed class PrizeTests
    {
        #region Tests for class Prize_Money_IntWinners
        [TestMethod]
        public void Prize_Money_IntWinners_WinningPlayers_ToString_NoWinners_ReturnsCorrectMessage()
        {
            // Arrange
            Prize_Money_IntWinners prize = new Prize_Money_IntWinners(1, "Grand Prize", 10, 0.5m); 
            prize.WinningTickets = new List<Ticket>(); // No winners

            // Act
            string result = prize.WinningPlayers_ToString();

            // Assert
            Assert.AreEqual("* Grand Prize: There were no winners this time!", result); // Expected message when no winners
        }

        [TestMethod]
        public void Prize_Money_IntWinners_WinningPlayers_ToString_SingleWinner_ReturnsCorrectMessage()
        {
            // Arrange
            Prize_Money_IntWinners prize = new Prize_Money_IntWinners(1, "Grand Prize", 1, 0.5m);
            prize.WinningTickets = new List<Ticket> { new Ticket(1, "Standard", 1, 123) };
            prize.WinningsPercentage = 0.5m;
            prize.NumberOfWinners = 1;
            prize.PrizeMoney = 50;
            prize.PrizeMoney_PerPlayer = 50;

            // Act
            string result = prize.WinningPlayers_ToString();

            // Assert
            Assert.AreEqual("* Grand Prize: Player 123 wins $50.00!", result);
        }

        [TestMethod]
        public void Prize_Money_IntWinners_Return_Prize_MaxAmount_CalculatesCorrectly()
        {
            // Arrange
            Prize_Money_IntWinners prize = new Prize_Money_IntWinners(1, "Grand Prize", 1, 0.5m);

            // Act
            decimal result = prize.Return_Prize_MaxAmount(100); 

            // Assert
            Assert.AreEqual(50m, result); 
        }

        [TestMethod]
        public void Prize_Money_IntWinners_NumberOfWinners_Get_1Winner_ReturnsCorrectly()
        {
            // Arrange
            Prize_Money_IntWinners prize = new Prize_Money_IntWinners(1, "Grand Prize", 1, 0.5m); 

            // Act
            int winnerCount = prize.NumberOfWinners_Get(100); 

            // Assert
            Assert.AreEqual(1, winnerCount); // Expected just 1 winner
        }

        [TestMethod]
        public void Prize_Money_IntWinners_DetermineWinners_1Winner_ReturnsCorrectly()
        {
            // Arrange
            Prize_Money_IntWinners prize = new Prize_Money_IntWinners(1, "Grand Prize", 1, 0.5m);
            List<Ticket> ticketList = new List<Ticket>()
            {
                new Ticket(1, "standard", 1, 1),
                new Ticket(2, "standard", 1, 2),
                new Ticket(3, "standard", 1, 3)
            };

            // Act
            List<Ticket> winningTickets = prize.DetermineWinners(ticketList, prize.NumberOfWinners);

            // Assert
            Assert.IsTrue(winningTickets.Count() == 1);
        }

        [TestMethod]
        public void Prize_Money_IntWinners_DetermineWinnings_1Winner_ReturnsCorrectly()
        {
            // Arrange
            Prize_Money_IntWinners prize = new Prize_Money_IntWinners(1, "Grand Prize", 1, 0.5m);

            // Act
            decimal prizeMoney = prize.DetermineWinnings(100);

            // Assert
            Assert.AreEqual(50m, prizeMoney);
        }
        #endregion

        #region Tests for class Prize_Money_PercentageWinners
        [TestMethod]
        public void Prize_Money_PercentageWinners_WinningPlayers_ToString_MultipleWinners_ReturnsCorrectMessage()
        {
            // Arrange
            Prize_Money_PercentageWinners prize = new Prize_Money_PercentageWinners(1, "Second Prize", 0.2m, 0.3m);
            prize.WinningTickets = new List<Ticket> { new Ticket(1, "Standard", 1, 456), new Ticket(1, "Standard", 1, 789) };
            prize.NumberOfWinners = 2;
            prize.PrizeMoney = 30;
            prize.PrizeMoney_PerPlayer = 15;

            // Act
            string result = prize.WinningPlayers_ToString();

            // Assert
            Assert.AreEqual("* Second Prize: Players 456, 789 win $15.00 each!", result);
        }

        [TestMethod]
        public void Prize_Money_PercentageWinners_NumberOfWinners_Get_20Percent_Returns20AsWinnerCount()
        {
            // Arrange
            Prize_Money_PercentageWinners prize = new Prize_Money_PercentageWinners(1, "Second Prize", 0.2m, 0.3m);

            // Act
            int winnerCount = prize.NumberOfWinners_Get(100);

            // Assert
            Assert.AreEqual(20, winnerCount);
        }

        [TestMethod]
        public void Prize_Money_PercentageWinners_Return_Prize_MaxAmount_CalculatesCorrectly()
        {
            // Arrange
            Prize_Money_PercentageWinners prize = new Prize_Money_PercentageWinners(1, "Second Prize", 0.2m, 0.3m);

            // Act
            decimal result = prize.Return_Prize_MaxAmount(100);

            // Assert
            Assert.AreEqual(30m, result);
        }

        [TestMethod]
        public void Prize_Money_PercentageWinners_DetermineWinners_20PercentWinners_ReturnsCorrectly()
        {
            // Arrange
            Prize_Money_PercentageWinners prize = new Prize_Money_PercentageWinners(1, "Second Prize", 0.2m, 0.3m);
            List<Ticket> ticketList = new List<Ticket>();
            for (int i = 0; i < 10; i++)
            {
                int id = i + 1;
                ticketList.Add(new Ticket(id, "standard", 1, id));
            }
            prize.NumberOfWinners = prize.NumberOfWinners_Get(ticketList.Count());

            // Act
            List<Ticket> winningTickets = prize.DetermineWinners(ticketList, prize.NumberOfWinners);

            // Assert
            Assert.IsTrue(winningTickets.Count() == 2);
        }

        [TestMethod]
        public void Prize_Money_PercentageWinners_DetermineWinnings_30PercentOfWinnings_ReturnsCorrectly()
        {
            // Arrange
            Prize_Money_PercentageWinners prize = new Prize_Money_PercentageWinners(1, "Second Prize", 0.2m, 0.3m);
            List<Ticket> ticketList = new List<Ticket>();
            for (int i = 0; i < 10; i++)
            {
                int id = i + 1;
                ticketList.Add(new Ticket(id, "standard", 1, id));
            }
            prize.NumberOfWinners = prize.NumberOfWinners_Get(ticketList.Count());

            // Act
            decimal prizeMoney = prize.DetermineWinnings(10);

            // Assert
            Assert.AreEqual(3m, prizeMoney);
        }

        #endregion
    }
}
