using Reports.Domain.Entities;
using Reports.Domain.Enums;
using Reports.Domain.Exceptions;
using Reports.Domain.Interfaces.Model;
using System.ComponentModel.DataAnnotations;

namespace Reports.Domain.Models
{
    public class Student : ISelectable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateOnly Birthdate { get; set; }
        public Gender Gender { get; set; }
        public int Absences { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int ClassRoomId { get; set; }
        public ClassRoom? Class { get; set; }
        public List<Report> Reports { get; set; } = [];
        public List<Goal> Goals { get; set; } = [];
        public List<Assessment> Assessments { get; set; } = [];
        public bool IsSelected { get; set; }

        public bool IsAGirl()
        {
            return Gender switch
            {
                Gender.Female => true,
                Gender.Male => false,
                _ => throw new GenderNotFoundException("Invalid gender, please select F for female and M for male.")
            };
        }
    }
}