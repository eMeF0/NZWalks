using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //https://localhost:7241/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: api/<StudentsController>
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentsName = new string[] { "Mateusz", "Jan", "Wiktoria", "Emilia", "Monika"};
            return Ok(studentsName);
        }
    }
}
