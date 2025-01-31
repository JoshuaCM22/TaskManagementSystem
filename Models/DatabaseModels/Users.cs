using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models.DatabaseModels
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required] // Ensures RoleName is not null
        [Index(IsUnique = true)] // Adds a unique constraint
        [StringLength(50)] // Optional
        public string Username { get; set; }

        public string Password { get; set; }

        [ForeignKey("Roles")]
        public byte RoleID { get; set; }

        public virtual Roles Roles { get; set; }
    }
}