using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models.ViewModels
{
    public class TaskViewModel
    {
        [Display(Name = "Task ID")]
        public int TaskId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage ="Description is required")]
        public string Description { get; set; }


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Due Data is required")]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        [Display(Name = "Priority")]
        public byte TaskPriorityID { get; set; }

        public string TaskPriorityName { get; set; }

        [Display(Name = "Status")]
        public byte TaskStatusID { get; set; }

        public string TaskStatusName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public int UserID { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}