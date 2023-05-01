using CI_PlatForm.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Entities.ViewModel
{
    public class MissionAdminViewModel
    {
        public long mission_id { get; set; }

        [Required(ErrorMessage = "Theme can't be null")]
        public long ThemeId { get; set; }

        public long? skillid { get; set; }

        [Required(ErrorMessage = "City can't be null")]
        public long CityId { get; set; }

        public string? skillname { get; set; }
        public string ThemeName { get; set; }

        public string SkillName { get; set; }

        [Required(ErrorMessage = "Description can't be null")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Country can't be null")]
        public long CountryId { get; set; }

        [Required(ErrorMessage = "Title can't be null")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Select mission type")]
        public bool missiontype { get; set; }

        [Required(ErrorMessage = "goalobject can't be null")]
        public string? goalobject { get; set; }

        [Required(ErrorMessage = "Total Seats can't be null")]
        public long? TotalSeats { get; set; }

        [Required(ErrorMessage = "Available seats can't be null")]
        public int? AvbSeat { get; set; }

        public long? Achieved { get; set; }

        [Required(ErrorMessage = "Deadline is required.")]
        public DateTime? Deadline { get; set; }

        [Required(ErrorMessage = "Organization Name can't be null")]
        public string? OrganizationName { get; set; }

        [Required(ErrorMessage = "Organization Details can't be null")]
        public string? OrganizationDetail { get; set; }

        [Required(ErrorMessage = "Select Availability.")]
        public string? Availability { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime? EndDate { get; set; }

        public List<MissionAdminViewModel>? Missions { get; set; }
        public List<Country>? countries { get; set; }

        public List<City>? cities { get; set; }

        public List<Skill>? Skills { get; set; }
        public List<MissionTheme>? theme { get; set; }

        public string? selected_skill { get; set; }
        public List<MissionSkill>? missionSkills { get; set; }
        public MissionAdminViewModel? mission { get; set; }

        public List<IFormFile>? missionMediums { get; set; }

        public List<IFormFile>? MissionDocuments { get; set; }
    }
}
