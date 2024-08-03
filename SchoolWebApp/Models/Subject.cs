using System.Diagnostics.CodeAnalysis;

namespace SchoolWebApp.Models {
    public class Subject {
        public int Id { get; set; }
        [NotNull]
        public string Name { get; set; }
    }
}
