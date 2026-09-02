using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingClient.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int VoteCount { get; set; }
        public string photo { get; set; } //путь к расположению в файлах
    }
}