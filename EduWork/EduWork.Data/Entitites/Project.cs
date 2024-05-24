using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduWork.Data.Entitites
{
    [Table("Projects")]
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid ProjectCategoryId { get; set; }

        public ProjectCategory ProjectCategory { get; set; }

        public ICollection<ProjectMember> ProjectMembers { get; set; }

        public ICollection<ProjectTime> ProjectTimes { get; set; }
    }
}
