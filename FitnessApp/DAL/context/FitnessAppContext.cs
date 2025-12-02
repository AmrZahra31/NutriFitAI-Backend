using System;
using System.Collections.Generic;
using FitnessApp.Domain.Entities;
using FitnessApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.DAL.context;

public partial class FitnessAppContext : IdentityDbContext<ApplicationUsers>
{
    public FitnessAppContext()
    {
    }

    public FitnessAppContext(DbContextOptions<FitnessAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbDietPlan> TbDietPlans { get; set; }

    public virtual DbSet<TbDietPlanMeal> TbDietPlanMeals { get; set; }

    public virtual DbSet<TbMeal> TbMeals { get; set; }

    public virtual DbSet<TbMealCategory> TbMealCategories { get; set; }

    public virtual DbSet<TbProcessTracking> TbProcessTrackings { get; set; }



    public virtual DbSet<TbUserSetting> TbUserSettings { get; set; }

    public virtual DbSet<TbWorkout> TbWorkouts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<TbDietPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__diet_pla__BE9F8F1DBB888761");

            entity.ToTable("diet_plans");

            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PlanName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("plan_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");


            entity.HasOne(d => d.TbUser)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TbDietPlanMeal>(entity =>
        {
            entity.HasKey(e => e.PlanMealId).HasName("PK__diet_pla__4A4DC9331DCD973E");

            entity.ToTable("diet_plan_meals");

            entity.Property(e => e.PlanMealId).HasColumnName("plan_meal_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.MealId).HasColumnName("meal_id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");

            // الربط مع TbMealCategory
            entity.HasOne(d => d.TbCategory)
                .WithMany(p => p.TbDietPlanMeals)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__diet_plan__categ__52593CB8");

            // الربط مع TbMeal
            entity.HasOne(d => d.TbMeal)
                .WithMany(p => p.TbDietPlanMeals)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__diet_plan__meal___5165187F");

            // الربط مع TbDietPlan
            entity.HasOne(pm => pm.TbDietPlan)
                .WithMany(p => p.TbDietPlanMeals)
                .HasForeignKey(pm => pm.PlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<TbMeal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__meals__2910B00F26EF864F");

            entity.ToTable("meals");

            entity.Property(e => e.MealId).HasColumnName("meal_id");
            entity.Property(e => e.Calories).HasColumnName("calories");
            entity.Property(e => e.Carbs)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("carbs");
            entity.Property(e => e.Fats)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("fats");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("meal_name");
            entity.Property(e => e.Protein)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("protein");
        });

        modelBuilder.Entity<TbMealCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__meal_cat__D54EE9B4732F10F3");

            entity.ToTable("meal_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("category_name");

            entity.HasMany(c => c.TbDietPlanMeals)
        .WithOne(pm => pm.TbCategory)
        .HasForeignKey(pm => pm.CategoryId)
        .OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<TbProcessTracking>(entity =>
        {
            entity.HasKey(e => e.TrackingId).HasName("PK__process___7AC3E9AEE6DD31CF");

            entity.ToTable("process_tracking");

            entity.Property(e => e.TrackingId).HasColumnName("tracking_id");
            entity.Property(e => e.CaloriesBurned).HasColumnName("calories_burned");
            entity.Property(e => e.CaloriesConsumed).HasColumnName("calories_consumed");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");
            entity.Property(e => e.WorkoutDone)
                .HasDefaultValue(false)
                .HasColumnName("workout_done");

            entity.HasOne(p => p.TbUser)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        });



        modelBuilder.Entity<TbUserSetting>(entity =>
        {
            entity.HasKey(e => e.SettingId).HasName("PK__user_set__256E1E326CB7D77F");

            entity.ToTable("user_settings");

            entity.HasIndex(e => e.UserId, "UQ__user_set__B9BE370E429DA413").IsUnique();

            entity.Property(e => e.SettingId).HasColumnName("setting_id");
            entity.Property(e => e.NotificationsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("notifications_enabled");
            entity.Property(e => e.PreferredUnits)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Metric")
                .HasColumnName("preferred_units");
            entity.Property(e => e.Theme)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Light")
                .HasColumnName("theme");
            entity.Property(e => e.UserId).HasColumnName("user_id");


            entity.HasOne(s => s.TbUser)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TbWorkout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("PK__workouts__02AB2F8E1F23437A");

            entity.ToTable("workouts");

            entity.Property(e => e.WorkoutId).HasColumnName("workout_id");
            entity.Property(e => e.CaloriesBurned).HasColumnName("calories_burned");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes");
            
            entity.Property(e => e.WorkoutName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("workout_name");
            entity.Property(e => e.WorkoutType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("workout_type");


            
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
