using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models.DatabaseModels
{
    [Table("TaskLogs")]
    public class TaskLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        [ForeignKey("TaskPriority")]
        public byte TaskPriorityID { get; set; }

        [ForeignKey("TaskStatus")]
        public byte TaskStatusID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Action")]
        public byte ActionID { get; set; }

        public DateTime DateTimeCreated { get; set; }

        [ForeignKey("CreatedByUser")]
        public int CreatedBy { get; set; } // ✅ Rename ForeignKey mapping explicitly

        // Navigation Properties
        public virtual TaskPriorities TaskPriority { get; set; }
        public virtual TaskStatuses TaskStatus { get; set; }
        public virtual Users User { get; set; } // Refers to UserID
        public virtual Users CreatedByUser { get; set; } // ✅ New explicit reference for CreatedBy
        public virtual Actions Action { get; set; }
    }
}
