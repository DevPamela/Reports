using Microsoft.EntityFrameworkCore;
using Reports.Domain.Exceptions;
using Reports.Domain.Interfaces.Model;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Reports.Domain.Models
{
    public class Student : ISelectable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Birthday { get; set; }
        public string? Genre { get; set; }
        public int Absences { get; set; } = 0;
        //TODO: check nullability of foreign key
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        //TODO: check nullability of foreign key
        public int? ClassRoomId { get; set; }
        public ClassRoom? Class { get; set; }
        public List<Report> Reports { get; set; } = new();
        public List<Goal> Goals { get; set; } = new();
        public List<Assessment> Assessments { get; set; } = new();
        public bool IsSelected { get; set; } = false;

        public bool IsAGirl()
        {
            if (Genre == "Feminino")
            {
                return true;
            }
            else if (Genre == "Masculino")
            {
                return false;
            }
            else
            {
                throw new GenreNotFoundException("Invalid gender, please select F for female and M for male.");
            }
        }
    }
}