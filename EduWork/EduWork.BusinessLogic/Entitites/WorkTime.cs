

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduWork.BusinessLogic.Entitites
{
    [Table("WorkTimes")]
    [Index(nameof(Date))]
    [Index(nameof(UserId))]
    public class WorkTime
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set;}

        public User User { get; set; }
    }
}
