using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using VoteAtHome.Models;


namespace VoteAtHome.Controllers
{
    public class VoteController : ApiController
    {
        private static List<Vote> votes = VoteDB.GetAllVotes();
        public IHttpActionResult Get()
        {
            return Ok(votes);
        }
        public IHttpActionResult Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    "Идентификатор должен быть больше нуля.");
            }

            Vote vote =
                votes.FirstOrDefault(
                    item => item.Id == id);

            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }
        public IHttpActionResult Post([FromBody] Vote vote)
        {
            if (vote == null)
            {
                return BadRequest("Данные не переданы.");
            }
            if (string.IsNullOrWhiteSpace(vote.FullName))
            {
                return BadRequest("Имя не должно быть пустым.");
            }
            if (vote.Age <= 0)
            {
                return BadRequest("Возраст не может быть равен 0 или меньше.");
            }
            if (string.IsNullOrWhiteSpace(vote.Description))
            {
                return BadRequest("Описание не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(vote.Category))
            {
                return BadRequest("Категория не может быть пустой.");
            }

            
            vote.photo = vote.photo ?? "pho.jpg";

            
            int generatedId = VoteDB.FillingVote(
                vote.FullName,
                vote.Age,
                vote.Description,
                vote.Category,
                vote.VoteCount,
                vote.photo
            );

            
            vote.Id = generatedId;

            
            if (votes != null)
            {
                votes.Add(vote);
            }

            
            return CreatedAtRoute(
                "DefaultApi",
                new { controller = "vote", id = vote.Id },
                vote
            );
        }


        public IHttpActionResult Put(int id, [FromBody] Vote vote)
        {
            
            if (id <= 0)
            {
                return BadRequest("Идентификатор должен быть больше нуля.");
            }

            if (vote == null)
            {
                return BadRequest("Данные не переданы.");
            }

            
            if (string.IsNullOrWhiteSpace(vote.FullName))
            {
                return BadRequest("Имя не может быть пустым.");
            }

            if (vote.Age <= 0)
            {
                return BadRequest("Возраст не может быть меньше или равен 0.");
            }

            if (string.IsNullOrWhiteSpace(vote.Description))
            {
                return BadRequest("Описание не может быть пустым.");
            }

            if (string.IsNullOrWhiteSpace(vote.Category))
            {
                return BadRequest("Категория не может быть пустой.");
            }

            
            Vote existingVote = VoteDB.GetVoteById(id);

            if (existingVote == null)
            {
                return NotFound(); 
            }

            
            existingVote.FullName = vote.FullName;
            existingVote.Age = vote.Age;
            existingVote.Description = vote.Description;
            existingVote.Category = vote.Category;

            
            VoteDB.UpdateVote(existingVote);

            
            if (votes != null)
            {
                Vote listVote = votes.FirstOrDefault(item => item.Id == id);
                if (listVote != null)
                {
                   
                    listVote.FullName = existingVote.FullName;
                    listVote.Age = existingVote.Age;
                    listVote.Description = existingVote.Description;
                    listVote.Category = existingVote.Category;
                }
                else
                {
                    
                    votes.Add(existingVote);
                }
            }

            
            return Ok(existingVote);
        }
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Идентификатор должен быть больше нуля.");
            }

            Vote vote = VoteDB.GetVoteById(id);

            if (vote == null)
            {
                return NotFound();
            }

            VoteDB.DeleteVote(id);
            votes.RemoveAll(item => item.Id == id);
            return Ok(vote);
        }
        [HttpPost] 
        public IHttpActionResult VoteUp(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Идентификатор должен быть больше нуля.");
            }

            bool isUpdatedInDb = VoteDB.IncrementVoteCount(id);

            if (!isUpdatedInDb)
            {
                return NotFound();
            }

            if (votes != null)
            {
                Vote listVote = votes.FirstOrDefault(item => item.Id == id);
                if (listVote != null)
                {
                    listVote.VoteCount++;
                }
            }

            return Ok();
        }



    }
}