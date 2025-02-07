using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models.DatabaseModels
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public byte RoleID { get; set; }

        [Required] // Ensures RoleName is not null
        [Index(IsUnique = true)] // Adds a unique constraint
        [StringLength(50)] // Optional
        public string RoleName { get; set; }
    }
}