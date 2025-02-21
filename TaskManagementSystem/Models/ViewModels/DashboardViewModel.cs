using System.Collections.Generic;

namespace TaskManagementSystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int AllTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks  { get; set; }
        public int CompletedTasks { get; set; }
        public int OnHoldTasks { get; set; }
        public int CanceledTasks { get; set; }
    }
}