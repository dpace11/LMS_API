﻿using LMS_API.Models.Student;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ConsumeAPI.Controllers
{
    public class StudentConusmeController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:7147";

        [HttpGet]
        public IActionResult Index()
        {
            List<Students> data = new List<Students>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("/api/Student/GetAllStudent").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string stringData = response.Content.ReadAsStringAsync().Result;
                        data = JsonConvert.DeserializeObject<List<Students>>(stringData);
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
        public IActionResult AddStudent()
        {
            Students student = new();
            return View(student);
        }

        [HttpPost]
        public IActionResult AddStudent(Students model)
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
                        if (model.StudentID == Guid.Empty)
                        {
                            HttpResponseMessage response = client.PostAsync("/api/Student/AddStudent", contentData).Result;

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
                            HttpResponseMessage response = client.PutAsync("/api/Student/UpdateStudent", contentData).Result;
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


        public IActionResult DeleteStudent(Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage response = client.DeleteAsync("/api/Student/DeleteStudent/" + id).Result;

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
        public IActionResult EditStudent()
        {
             Students student = new();
            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent(Guid model)
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

                      
                            HttpResponseMessage response = client.PutAsync($"/api/Student/UpdateStudent/{model}", contentData).Result;
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


        [HttpGet]
        public IActionResult StudentDetails(Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                HttpResponseMessage response = client.GetAsync("/api/Student/GetStudentById/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Students student = JsonConvert.DeserializeObject<Students>(stringData);
                    return View(student);
                }
                else
                {
                    TempData["error"] = $"{response.ReasonPhrase}";
                }
            }

            return View();

        }
    }
}
