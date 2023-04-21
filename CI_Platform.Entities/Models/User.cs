using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI_PlatForm.Entities.Models;

public partial class User
{
    public long UserId { get; set; }
    [Required(ErrorMessage = "First name is required!")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required!")]
    public string? LastName { get; set; }
    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password is required!")]
    public string Password { get; set; } = null!;
    [NotMapped]
    [Compare("Password")]
    [Required(ErrorMessage = "Confirm Password is required!")]
    public string ConfirmPassword { get; set; } = null!;
    [Required(ErrorMessage = "Phone number is required!")]
    public int PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    public string? WhyIVolunteer { get; set; }

    public string? EmployeeId { get; set; }

    public string? Department { get; set; }

    public long? CityId { get; set; }

    public long? CountryId { get; set; }

    public string? ProfileText { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? Title { get; set; }

    public bool? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Manager { get; set; }

    public string? Availability { get; set; }

    public virtual ICollection<ContactU> ContactUs { get; } = new List<ContactU>();

    public virtual Country? Country { get; set; }

    public virtual ICollection<FavoriteMission> FavoriteMissions { get; } = new List<FavoriteMission>();

    public virtual ICollection<MissionApplication> MissionApplications { get; } = new List<MissionApplication>();

    public virtual ICollection<MissionInvite> MissionInviteFromUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionInvite> MissionInviteToUsers { get; } = new List<MissionInvite>();

    public virtual ICollection<MissionRating> MissionRatings { get; } = new List<MissionRating>();

    public virtual ICollection<Story> Stories { get; } = new List<Story>();

    public virtual ICollection<StoryView> StoryViews { get; } = new List<StoryView>();

    public virtual ICollection<Timesheet> Timesheets { get; } = new List<Timesheet>();

    public virtual ICollection<UserSkill> UserSkills { get; } = new List<UserSkill>();
}
