using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models.DatabaseModels
{
    [Table("Tasks")]
    public class Tasks
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set;}
        [ForeignKey("TaskStatus")]
        public byte TaskStatusID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual TaskStatuses TaskStatus { get; set; }

        public virtual Users User { get; set; }
    }
}