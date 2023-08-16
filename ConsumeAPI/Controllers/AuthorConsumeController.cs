using LMS_API.Models.Author;
using LMS_API.Models.Student;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ConsumeAPI.Controllers
{
    public class AuthorConsumeController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:7147";
        public IActionResult Index()
        {
            List<Author> data = new List<Author>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("/api/Author/GetAllAuthors").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string stringData = response.Content.ReadAsStringAsync().Result;
                        data = JsonConvert.DeserializeObject<List<Author>>(stringData);
                    }
                    else
                    {
                        TempData["error"] = $"{response.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["exception"] = ex.Message;
            }
            return View(data);
        }


        [HttpGet]
        public IActionResult AddAuthor()
        {
            Author author = new Author();
            return View(author);
        }


        [HttpPost]
        public IActionResult AddAuthor(Author model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiBaseUrl);
                        var data = JsonConvert.SerializeObject(model);
                        var contentData = new StringContent(data, Encoding.UTF8, "application/json");

                        if (model.ID == Guid.Empty)
                        {
                            HttpResponseMessage response = client.PostAsync("api/Author/AddAuthor", contentData).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                TempData["success"] = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                TempData["error"] = $"{response.ReasonPhrase}";
                            }
                        }
                        else
                        {
                            HttpResponseMessage response = client.PutAsync("/api/Author/UpdateAuthor", contentData).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["success"] = response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                TempData["error"] = $"{response.ReasonPhrase}";
                            }
                        }

                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ModelState is not vali!!");
                    return View(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }


        public IActionResult DeleteAuthor(Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage response = client.DeleteAsync($"api/Author/DeleteAuthor/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    TempData["error"] = $"{response.ReasonPhrase}";
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AuthorDetail(Guid id)
        { 
            using (HttpClient client = new HttpClient()) 
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage response = client.GetAsync("api/Author/GetAuthorById/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string strindData = response.Content.ReadAsStringAsync().Result;
                    Author author = JsonConvert.DeserializeObject<Author>(strindData);
                    return View(author);
                }
                else
                {
                    TempData["error"] = $"{response.ReasonPhrase}";
                }                
            }
            return View();
        }


        [HttpGet]
        public IActionResult EditAuthor()
        {
            Author auth = new();
            return View(auth);
        }

        [HttpPost]
        public IActionResult EditAuthor(Author model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiBaseUrl);
                        var data = JsonConvert.SerializeObject(model);
                        var contentData = new StringContent(data, Encoding.UTF8, "application/json");


                        HttpResponseMessage response = client.PutAsync("/api/Author/UpdateAuthor", contentData).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["success"] = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            TempData["error"] = $"{response.ReasonPhrase}";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ModelState is not vali!!");
                    return View(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        
    }
}
