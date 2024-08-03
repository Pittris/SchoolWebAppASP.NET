using System.Diagnostics.CodeAnalysis;

namespace SchoolWebApp.Models {
    public class Student {
        public int Id { get; set; }
        [NotNull]
        public string? FirstName { get; set; }
        [NotNull]
        public string? LastName { get; set;}
        public DateTime DateOfBirth { get; set; }
    }
}
