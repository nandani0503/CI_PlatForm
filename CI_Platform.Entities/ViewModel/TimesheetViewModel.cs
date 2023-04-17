using CI_PlatForm.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Entities.ViewModel
{
    public class TimesheetViewModel
    {
     public List<Timesheet> TimeSheets { get; set; }
        public List<Timesheet> GoalSheets { get; set; }
        public List<MissionApplication> TimeMission { get; set; }
        public List<MissionApplication> GoalMission { get; set; }
    }
}
