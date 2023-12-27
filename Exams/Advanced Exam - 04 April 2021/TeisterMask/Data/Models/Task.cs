﻿namespace TeisterMask.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TeisterMask.Data.Models.Enums;

    public class Task
    {
        public Task()
        {
            EmployeesTasks = new HashSet<EmployeeTask>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.TaskNameMaxLength)]
        public string Name { get; set; } = null!;

        public DateTime OpenDate { get; set; }

        public DateTime DueDate { get; set; }

        public ExecutionType ExecutionType { get; set; }

        public LabelType LabelType { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public virtual ICollection<EmployeeTask> EmployeesTasks { get; set; } = null!;
    }
}
