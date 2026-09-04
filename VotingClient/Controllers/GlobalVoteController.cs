using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using VotingClient.Models;
using System.Text;

namespace VotingClient.Controllers
{
    public class GlobalVoteController : Controller
    {
        private const string ApiAddress =
            "http://localhost:58230";
        public async Task<ActionResult> Vote()
        {
            List<Vote> votes =
                await GetVote(
                    ApiAddress + "/api/vote");

            return View(votes);
        }
        private async Task<List<Vote>>
           GetVote(string url)
        {
            using (HttpClient client =
                new HttpClient())
            {
                HttpResponseMessage response =
                    await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Vote>();
                }

                string json =
                    await response.Content
                        .ReadAsStringAsync();

                List<Vote> votes =
                    JsonConvert
                        .DeserializeObject<List<Vote>>(
                            json);

                return votes;
            }
        }
        public async Task<ActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Vote");
            }

            string url =
                ApiAddress +
                "/api/searcname?name=" +
                System.Uri.EscapeDataString(name);

            List<Vote> votes =
                await GetVote(url);

            return View("Vote", votes);
        }

        public async Task<ActionResult> SearchVoice(int maxVoice)
        {
            if (maxVoice <= 0)
            {
                return RedirectToAction("Vote");
            }

            string url =
                ApiAddress +
                "/api/searchvoice?minVoice=1&maxVoice=" +
                maxVoice.ToString(
                    System.Globalization
                        .CultureInfo.InvariantCulture);

            List<Vote> votes =
                await GetVote(url);
            return View("Index", votes);
        }
        public async Task<ActionResult> Category(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return RedirectToAction("Vote");
            }

            string url =
                ApiAddress +
                "/api/votecategory?category=" +
                System.Uri.EscapeDataString(category);

            List<Vote> votes =
                await GetVote(url);

            return View("Vote", votes);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(
    Vote votes)
        {
            string json =
                JsonConvert.SerializeObject(votes);

            HttpContent content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            using (HttpClient client =
                new HttpClient())
            {
                HttpResponseMessage response =
                    await client.PostAsync(
                        ApiAddress + "/api/vote",
                        content);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message =
                        "Ошибка API. Код: " +
                        (int)response.StatusCode;

                    return View(votes);
                }
            }

            return RedirectToAction("Vote");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            using (HttpClient client =
                new HttpClient())
            {
                HttpResponseMessage response =
                    await client.GetAsync(
                        ApiAddress +
                        "/api/vote/" +
                        id);

                if (response.StatusCode ==
                    System.Net.HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(
                        "Vote");
                }

                string json =
                    await response.Content
                        .ReadAsStringAsync();

                Vote vote =
                    JsonConvert
                        .DeserializeObject<Vote>(
                            json);

                return View(vote);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(
Vote vote)
        {
            string json =
                JsonConvert.SerializeObject(
                    vote);

            HttpContent content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            using (HttpClient client =
                new HttpClient())
            {
                HttpResponseMessage response =
                    await client.PutAsync(
                        ApiAddress +
                        "/api/vote/" +
                        vote.Id,
                        content);

                if (!response.IsSuccessStatusCode)
                {
                    string error =
                        await response.Content
                            .ReadAsStringAsync();

                    ViewBag.Message =
                        "Ошибка API. Код: " +
                        (int)response.StatusCode +
                        ". Ответ сервера: " +
                        error;

                    return View(vote);
                }
            }

            return RedirectToAction(
                "Vote");
        }
        [HttpGet]
        public async Task<ActionResult> Delete(
    int id)
        {
            using (HttpClient client =
                new HttpClient())
            {
                HttpResponseMessage response =
                    await client.GetAsync(
                        ApiAddress +
                        "/api/vote/" +
                        id);

                if (response.StatusCode ==
                    System.Net.HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(
                        "Index");
                }

                string json =
                    await response.Content
                        .ReadAsStringAsync();

                Vote vote =
                    JsonConvert
                        .DeserializeObject<Vote>(
                            json);

                return View(vote);
            }
        }
        [HttpPost]
        public async Task<ActionResult>DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction(
                    "Vote");
            }

            using (HttpClient client =
            new HttpClient())
            {
                HttpResponseMessage response =
                    await client.DeleteAsync(
                        ApiAddress +
                        "/api/vote/" +
                        id);
                if (response.StatusCode ==System.Net.HttpStatusCode.NotFound)
                {
                    TempData["Message"] =
                        "Кандидат уже отсутствует.";

                    return RedirectToAction(
                        "Vote");
                }

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Message =
                        "Ошибка API. Код: " +
                        (int)response.StatusCode;

                    return RedirectToAction(
                        "Vote");
                }
            }
            TempData["Message"] ="Кандидат успешно удалён.";

            return RedirectToAction("Vote");
        }
        [HttpPost]
        public async Task<ActionResult> VoteUp(int id)
        {
            if (id <= 0) return RedirectToAction("Vote");

            using (HttpClient client = new HttpClient())
            {
                string url = ApiAddress + "/api/vote/voteup/" + id;

                HttpResponseMessage response = await client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Спасибо! Ваш голос успешно учтен.";
                }
                else
                {
                    TempData["Message"] = "Что-то пошло не так!";
                }
            }
            return RedirectToAction("Vote");
        }


    }
}