using Reports.Domain.Entities;
using Reports.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Reports.Application.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly Birthdate { get; set; }
        public Gender Gender { get; set; }
        public int Absences { get; set; }
        public int TeacherId { get; set; }
        public int ClassRoomId { get; set; }
    }
}
