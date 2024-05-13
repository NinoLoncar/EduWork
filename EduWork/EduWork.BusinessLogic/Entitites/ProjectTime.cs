using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduWork.BusinessLogic.Entitites
{

    [Table("ProjectTimes")]
    [Index(nameof(UserId))]
    [Index(nameof(ProjectId))]
    public class ProjectTime
    {
        [Key]
        public Guid UserId { get; set; }

        [Key]
        public Guid ProjectId { get; set; }

        public string Comment { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        public User User { get; set; }
        
        public Project Project { get; set; }
    }
}
