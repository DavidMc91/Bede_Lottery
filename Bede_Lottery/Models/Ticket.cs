using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bede_Lottery.Models
{
    public class Ticket
    {
        public int Id { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PlayerID { get; }

        public Ticket(int id, string name, decimal price, int playerID)
        {
            Id = id;
            Name = name;
            Price = price;
            PlayerID = playerID;
        }
    }
}
