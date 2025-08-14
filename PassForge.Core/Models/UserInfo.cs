using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassForge.Core.Models
{
    public class UserInfo
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nickname { get; set; }

        /// <summary>Date of birth optional; if we don't use age.</summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>If you don't have a date of birth, you can give your age in years/months/days..</summary>
        public int? AgeYears { get; set; }
        public int? AgeMonths { get; set; }
        public int? AgeDays { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        /// <summary>Additional words such as another nickname, city name, favorite team, etc ...</summary>
        public List<string> ExtraWords { get; set; } = new();
    }
}
