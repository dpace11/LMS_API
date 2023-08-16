using LMS_API.Models.Membership;
using LMS_API.Models.Student;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ConsumeAPI.Controllers
{
    public class MembershipConsumeController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:7147";

        [HttpGet]
        public IActionResult Index()
        {
            List<Membership> data = new();

            try
            {
                using (HttpClient client = new HttpClient()) 
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicatin/json"));

                    HttpResponseMessage response = client.GetAsync("/api/Membership/GetAllMembers").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string stringData = response.Content.ReadAsStringAsync().Result;
                        data = JsonConvert.DeserializeObject<List<Membership>>(stringData);

                    }
                    else
                    {
                        TempData["error"] = $"{response.ReasonPhrase}";
                    }                  
                    
                }
            }
            catch(Exception ex)
            {
                TempData["exception"] = ex.Message;
            }
            return View(data);
        }


        [HttpGet]
        public IActionResult AddMembership()
        {
            Membership member = new();
            return View (member);
        }


        [HttpPost]
        public IActionResult AddMembership(Membership model)
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
                        if (model.MembershipId == Guid.Empty)
                        {
                            HttpResponseMessage response = client.PostAsync("api/Membership/AddMembers", contentData).Result;
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
                            HttpResponseMessage response = client.PutAsync("api/Membership/UpdateMembers", contentData).Result;
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
                    ModelState.AddModelError(string.Empty, "model state is not valid!!");
                }
            }
            catch(Exception ex)
            {
                TempData["exception"] = ex.Message; 
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult>  DeleteMembership(Guid id)
        {
            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage respone = client.DeleteAsync("/api/Membership/DeleteMember/" + id).Result;

                if (respone.IsSuccessStatusCode)
                {
                    TempData["success"] =await respone.Content.ReadAsStringAsync();
                }
                else
                {
                    TempData["error"] = $"{respone.ReasonPhrase}";
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult MembershipDetail(Guid id)
        {
            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage response = client.GetAsync("api/Membership/GetMemberById/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Membership member = JsonConvert.DeserializeObject<Membership>(stringData);
                    return View(member);
                }
                else
                {
                    TempData["error"] = $"{response.ReasonPhrase}";
                }
            }
            return View();            
        }


        [HttpGet]
        public IActionResult EditMembership()
        {
            Membership mem = new();
            return View(mem);
        }


        [HttpPost]
        public IActionResult EditMembership(Guid model)
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


                        HttpResponseMessage response = client.PutAsync($"/api/Membership/UpdateMembers/{model}", contentData).Result;
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
