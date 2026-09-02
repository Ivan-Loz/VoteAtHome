using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VoteAtHome.Models;

namespace VoteAtHome.Controllers
{
    public class SearcNameController : ApiController
    {
        private static List<Vote> votes = VoteDB.GetAllVotes();

        public IHttpActionResult Get(
                string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(
                    "Строка поиска не должна быть пустой.");
            }

            List<Vote> result =
                votes
                    .Where(
                        item =>
                            item.FullName.IndexOf(
                                name,
                                StringComparison
                                    .OrdinalIgnoreCase)
                            >= 0)
                    .ToList();

            return Ok(result); 
        }
    }
}
