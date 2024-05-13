using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduWork.BusinessLogic.Entitites
{
    [Table("ProjectMembers")]
    public class ProjectMember
    {
        [Key]
        public Guid UserId { get; set; }
        [Key]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid ProjectRoleId { get; set; }


        public User User { get; set; }


        public Project Project { get; set; }

        public ProjectRole ProjectRole { get; set; }
    }
}
