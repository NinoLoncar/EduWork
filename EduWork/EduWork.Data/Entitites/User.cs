using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduWork.Domain.Entitites


{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        public Role Role { get; set; }

        public ICollection<WorkTime> WorkTimes { get; set; }

        public ICollection<ProjectTime> ProjectTimes { get; set; }
    }
}
