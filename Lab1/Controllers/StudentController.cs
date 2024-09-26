using Lab1.Models;
using Lab1.Models.Repositories;
using Lab1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab1.Controllers
{
    public class StudentController : Controller
    {
        private readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly  IStudentRepository _studentRepository;
        public StudentController(IBufferedFileUploadService bufferedFileUploadService, IStudentRepository studentRepository)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
            _studentRepository = studentRepository;
        }    

        public IActionResult Index()
        {
            return View(_studentRepository.GetStudents());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "IT", Value = "1"},
                new SelectListItem() { Text = "BE", Value = "2"},
                new SelectListItem() { Text = "CE", Value = "3"},
                new SelectListItem() { Text = "EE", Value = "4"},
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var result = await _bufferedFileUploadService.UploadFile(file);
                if (!result)
                {
                    ModelState.AddModelError("File", "File upload failed.");
                    return View(student);
                }
                student.ImagePath = "/img/" + file.FileName; // Lưu trữ đường dẫn ảnh
            }

            student.Id = _studentRepository.GetStudents().Last<Student>().Id + 1;
            _studentRepository.AddStudent(student);
            return View("Index", _studentRepository.GetStudents());
        }
    }
}