using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FitnessApp.Models;

namespace FitnessApp.Domain.Entities;

public partial class TbUserSetting
{
    [Key]
    public int SettingId { get; set; }

    [Required]
    public string UserId { get; set; }  // FK to AspNetUsers

    [ForeignKey("UserId")]
    public virtual ApplicationUsers TbUser { get; set; }  

    public string PreferredUnits { get; set; } = "kg";
    public bool NotificationsEnabled { get; set; } = true;
    public string Theme { get; set; } = "light";
    public bool GeneralNotification {  get; set; } = true;
    public bool Sound { get; set; } = true;
    public bool NextWorkoutTime { get; set; }=false;
    public bool ReminderToDrnkWater { get; set; } = false;
    public bool NextMealTime { get; set; } = false;
    public string Language { get; set; } = "english";
}
