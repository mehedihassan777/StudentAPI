using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentAPIDbContext dbContext;
        
        public StudentController(StudentAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            return Ok(await dbContext.Students.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetStudent([FromRoute] Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentRequest addStudentRequest)
        {
            var student = new Student()
            {
                Id = Guid.NewGuid(),
                Address = addStudentRequest.Address,
                Email = addStudentRequest.Email,
                Phone = addStudentRequest.Phone,
                FullName = addStudentRequest.FullName
            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();

            return Ok(student);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] Guid id, UpdateStudentRequest updateStudentRequest)
        {
            var student = await dbContext.Students.FindAsync(id);

            if(student != null)
            {
                student.Email = updateStudentRequest.Email;
                student.Phone = updateStudentRequest.Phone;
                student.FullName = updateStudentRequest.FullName;
                student.Address = updateStudentRequest.Address;

                await dbContext.SaveChangesAsync();
                return Ok(student);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if(student != null)
            {
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();
                return Ok(student);
            }
            return NotFound();
        }
    }
}
