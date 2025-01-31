using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models.ViewModels
{
    public class TaskViewModel
    {
        public int TaskId { get; set; }
        [Required(ErrorMessage ="Please Enter Task Title")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Please Enter Task Description")]
        public string Description { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "Status")]
        public byte TaskStatusID { get; set; }
        public string TaskStatusName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
    }
}