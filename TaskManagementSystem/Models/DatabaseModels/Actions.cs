using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models.DatabaseModels
{
    [Table("Actions")]
    public class Actions
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public byte ID { get; set; }
        public string Description { get; set; }
    }
}