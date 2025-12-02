using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FitnessApp.Models;

namespace FitnessApp.Domain.Entities;

public partial class TbProcessTracking
{
    [Key]
    public int TrackingId { get; set; }

    [Required]
    public string UserId { get; set; } // FK إلى AspNetUsers

    [ForeignKey("UserId")]
    public virtual ApplicationUsers TbUser { get; set; } 

    
    public DateTime Date { get; set; }

    public decimal Weight { get; set; } 

    
    public int CaloriesConsumed { get; set; } 

    
    public int CaloriesBurned { get; set; } 

    public bool WorkoutDone { get; set; } = false; 
}
