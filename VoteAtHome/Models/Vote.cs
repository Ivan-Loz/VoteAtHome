using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoteAtHome.Models
{
    public class Vote
    {

        public static List<string> votes = new List<string> { };
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int VoteCount { get; set; }
        public string photo { get; set; } //путь к расположению в файлах
    }
}