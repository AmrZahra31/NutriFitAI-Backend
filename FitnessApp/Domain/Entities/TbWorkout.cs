using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Domain.Entities;

public partial class TbWorkout
{
    [Key]
    public int WorkoutId { get; set; }

    public string? NextWorkout { get; set; }

    
    public string? WorkoutName { get; set; }

    
    public string? WorkoutType { get; set; } // Cardio, Strength, etc.

    
    public string DurationMinutes { get; set; } 

    
    public int? CaloriesBurned { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string URL { get; set; }

    public string Level { get; set; }
}
