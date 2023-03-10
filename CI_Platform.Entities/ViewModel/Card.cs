using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModel
{
    public class Card
    {
        public long CityId { get; set; }

        public string? Theme { get; set; }
        public string CityName { get; set; } = null!;
       
        public long MissionId { get; set; }
     
        public string? Title { get; set; }
        public string? MediaName { get; set; }
        public string? ShortDescription { get; set; }
        public int Rating { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string? OrganizationName { get; set; }
        public bool MissionType { get; set; }
        public int GoalValue { get; set; }
        public string? GoalObjectiveText { get; set; }
        public int? TotalSeat { get; set; }
        /*public string? MediaPath { get; set; }*/
        public DateTime? Deadline { get; set; }




    }
}
