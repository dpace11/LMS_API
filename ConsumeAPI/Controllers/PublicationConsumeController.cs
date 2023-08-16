using LMS_API.Models.Publication;
using LMS_API.Models.Student;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ConsumeAPI.Controllers
{
    public class PublicationConsumeController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:7147";

        [HttpGet]
        public IActionResult Index()
        {
            List<Publication> data=new List<Publication>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/Publication/GetAllPublications").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string stringData = response.Content.ReadAsStringAsync().Result;
                        data = JsonConvert.DeserializeObject<List<Publication>>(stringData);
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
        public IActionResult AddPublication()
        {
            Publication pub = new Publication();
            return View(pub);
        }


        [HttpPost]
        public IActionResult AddPublication(Publication model )
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
                            HttpResponseMessage response = client.PostAsync("/api/Publication/AddPublication", contentData).Result;

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
                            HttpResponseMessage response = client.PutAsync("/api/Publication/UpdatePublication", contentData).Result;
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

        
        public IActionResult DeletePublication(Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage response = client.DeleteAsync("/api/Publication/DeletePublication/" + id).Result;

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
        public IActionResult PublicationDetail(Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage response = client.GetAsync("/api/Publication/GetPublicationById/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Publication pub = JsonConvert.DeserializeObject<Publication>(stringData);
                    return View(pub);
                }
                else
                {
                    TempData["error"] = $"{response.ReasonPhrase}";
                }
            }

            return View();
        }


        [HttpGet]
        public IActionResult EditPublication() 
        {
            Publication pub = new();
            return View(pub);
        }


        [HttpPost]
        public IActionResult EditPublication(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiBaseUrl);
                        var data = JsonConvert.SerializeObject(id);
                        var contentData = new StringContent(data, Encoding.UTF8, "application/json");


                        HttpResponseMessage response = client.PutAsync($"/api/Publication/UpdatePublication/{id}", contentData).Result;
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
                    return View(id);
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
