using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
