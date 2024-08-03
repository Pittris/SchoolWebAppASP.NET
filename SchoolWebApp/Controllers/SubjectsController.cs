using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers {
    [Authorize]
    public class SubjectsController : Controller {
        private SubjectService _subjectService;

        public SubjectsController(SubjectService subjectService) {
            _subjectService = subjectService;
        }
        public IActionResult Index() {
            IEnumerable<SubjectDTO> allSubjects = _subjectService.GetSubjects();
            return View(allSubjects);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(SubjectDTO subjectDto) {
            if(ModelState.IsValid) { 
                await _subjectService.AddSubjectAsync(subjectDto);
                return RedirectToAction("Index");
            }
            else {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAsync(int id) {
            var subjectToEdit = await _subjectService.GetByIdAsync(id);
            if (subjectToEdit == null) {
                return View("NotFound");
            }
            return View(subjectToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Update(SubjectDTO subjectDto, int id) {
            await _subjectService.UpdateAsync(id, subjectDto);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id) {
            var subjectToDelete = await _subjectService.GetByIdAsync(id);
            if (subjectToDelete == null) {
                return View("NotFound");
            }
            await _subjectService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
