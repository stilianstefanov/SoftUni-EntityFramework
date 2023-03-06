namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            StudentsCourses = new HashSet<StudentCourse>();
        }

        public int CourseId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;

        public ICollection<Homework> Homeworks { get; set; } = null!;

        public ICollection<Resource> Resources { get; set; } = null!;
    }
}
