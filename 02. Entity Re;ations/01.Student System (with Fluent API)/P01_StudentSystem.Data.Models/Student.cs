namespace P01_StudentSystem.Data.Models
{           
    public class Student
    {
        public Student()
        {
            StudentsCourses = new HashSet<StudentCourse>();
            Homeworks = new HashSet<Homework>();
        }

        public int StudentId { get; set; }
     
        public string Name { get; set; } = null!;
        
        public string? PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;

        public ICollection<Homework> Homeworks { get; set; } = null!;
    }
}