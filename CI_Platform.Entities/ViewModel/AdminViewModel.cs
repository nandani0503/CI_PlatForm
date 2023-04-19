﻿using CI_PlatForm.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PlatForm.Entities.ViewModel
{
    public class AdminViewModel
    {
        public List<User>? UserList { get; set; }
        public List<MissionTheme>? ThemeList { get; set; }
        public List<Mission>? MissionList { get; set; }
        public List<MissionSkill>? MissionSkillList { get; set;} = new List<MissionSkill> { };
        public List<MissionApplication>? MissionApplicationList { get; set; } = new List<MissionApplication>(); 
        public List<CmsPage> CmsList { get; set; } = new List<CmsPage>();
        public List<Story>? StoryList { get; set; } = new List<Story> { };
     
      


    }
}
