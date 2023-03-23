using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Entities.ViewModel
{
    public class StoryViewModel
    {
        public long StoryId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }

        public string? storyImage { get; set; }

        public string Theme { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Avatar { get; set; }

        public string? UserName { get; set; }
        public long CountryId { get; set; }

        public long CityId { get; set; }
        public long SkillId { get; set; }

        public long ThemeId { get; set; }

        public string Type { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
