using SchoolWebApp.Models;

namespace SchoolWebApp.ViewModels {
    public class GradesDropdownsViewModel {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public GradesDropdownsViewModel() {
            Students = new List<Student>();
            Subjects = new List<Subject>();
        }
    }
}
