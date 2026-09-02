using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VoteAtHome.Models;

namespace VoteAtHome.Controllers
{
    public class VoteCategoryController : ApiController
    {
        private static List<Vote> votes = VoteDB.GetAllVotes();

        public IHttpActionResult Get(
                string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest(
                    "Строка поиска не должна быть пустой.");
            }

            List<Vote> result =
                votes
                    .Where(
                        item =>
                            item.Category.IndexOf(
                                category,
                                StringComparison
                                    .OrdinalIgnoreCase)
                            >= 0)
                    .ToList();

            return Ok(result);
        }
    }
}
