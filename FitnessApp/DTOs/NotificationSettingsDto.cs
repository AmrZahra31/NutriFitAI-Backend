namespace FitnessApp.DTOs
{
    public class NotificationSettingsDto
    {
        public bool? Sound { get; set; }
        public string? Theme { get; set; } 
        public bool? NextWorkoutTime { get; set; }
        public string? PreferredUnits { get; set; } 
        public bool? NotificationsEnabled { get; set; }
        public bool? GeneralNotification { get; set; }
        public bool? ReminderToDrinkWater { get; set; }
        public bool? NextMealTime { get; set; }
    }

}
