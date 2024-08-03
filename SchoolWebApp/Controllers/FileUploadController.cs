using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;
using System.Xml;

namespace SchoolWebApp.Controllers {
    public class FileUploadController : Controller {
        private StudentService _studentService;
        public FileUploadController(StudentService studentService) {
            _studentService = studentService;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file) {
            if (file.Length > 0) {
                string filepath = Path.GetFullPath(file.Name);
                using(var stream = new FileStream(filepath, FileMode.Create)) {
                    await file.CopyToAsync(stream);
                    stream.Close();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filepath);
                    XmlElement koren = xmlDocument.DocumentElement;
                    foreach (XmlNode node in koren.SelectNodes("/Students/Student")) {
                        StudentDTO studentDTO = new StudentDTO() {
                            DateOfBirth = DateTime.Parse(node.ChildNodes[2].InnerText),
                            FirstName = node.ChildNodes[0].InnerText,
                            LastName = node.ChildNodes[1].InnerText
                        };
                        await _studentService.AddStudentAsync(studentDTO);
                    }
                }
            }

            return RedirectToAction("Index", "Students");
        }
    }
}
