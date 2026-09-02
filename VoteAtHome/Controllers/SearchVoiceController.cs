using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VoteAtHome.Models;

namespace VoteAtHome.Controllers
{
    public class SearchVoiceController : ApiController
    {
        private static List<Vote> votes = VoteDB.GetAllVotes();
        public IHttpActionResult Get(
           decimal maxVoice, decimal minVoice)
        {

            if (minVoice < 0)
            {
                return BadRequest(
                    "Минемальное количество голосов не должно быть отрицательным.");
            }

            if (maxVoice <= 0)
            {
                return BadRequest(
                    "Максимальное количество голосов не должно быть больше нуля.");
            }


            if (minVoice > maxVoice)
            {
                return BadRequest(
                    "Минемальное количество голосов не должно быть больше максимального.");
            }

            List<Vote> result =
                votes
                    .Where(
                        item =>
                            item.VoteCount >= minVoice &&
                            item.VoteCount <= maxVoice)
                    .ToList();

            return Ok(result);
        }
    }
}
