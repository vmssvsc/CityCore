using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static CityCore.Common.Enums;

namespace CityCore.Data.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        public SettingsValues EnumValue { get; set; }

        public string SettingValue { get; set; }
    }
}
