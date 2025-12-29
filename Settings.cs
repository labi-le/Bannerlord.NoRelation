using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;
using TaleWorlds.Library;

namespace labile.Bannerlord.NoRelation
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "labile_norelation_v1";
        public override string DisplayName => "No Relation Notifications";
        public override string FolderName => "NoRelation";
        public override string FormatType => "json2";

        [SettingPropertyBool("Disable relation changes", Order = 0, RequireRestart = false,
            HintText = "Hides relationship change notifications")]
        [SettingPropertyGroup("General")]
        public bool DisableRelationNotifications { get; set; } = true;

        [SettingPropertyBool("Disable all notifications", Order = 1, RequireRestart = false,
            HintText = "Hides all popup notifications and redirects them to game chat")]
        [SettingPropertyGroup("General")]
        public bool DisableAllNotifications { get; set; } = false;

        [SettingPropertyBool("Disable notification sounds", Order = 2, RequireRestart = false,
            HintText = "Mutes UI notification sounds")]
        [SettingPropertyGroup("General")]
        public bool DisableNotificationSounds { get; set; } = false;

        [SettingPropertyInteger("Map circle timeout", 0, 86400, "0s", Order = 5, RequireRestart = false, HintText = "Time in seconds before circle notifications auto-remove. 0 = Disabled (Manual removal).")]
        [SettingPropertyGroup("General")]
        public int MapCircleTimeout { get; set; } = 0;

        [SettingPropertyDropdown("Log message color", Order = 4, RequireRestart = false,
            HintText = "Select the color for redirected messages in the game chat")]
        [SettingPropertyGroup("General")]
        private Dropdown<string> LogColorDropdown { get; set; } = new(new[]
        {
            "White", "Gray", "Red", "Green", "Blue", "Yellow", "Cyan", "Magenta", "Gold"
        }, 1);

        public Color GetLogColor()
        {
            return LogColorDropdown.SelectedIndex switch
            {
                0 => new Color(0.9f, 0.9f, 0.9f), // White
                1 => new Color(0.7f, 0.7f, 0.7f), // Gray (Standard Log)
                2 => new Color(0.8f, 0.2f, 0.2f), // Red
                3 => new Color(0.2f, 0.8f, 0.2f), // Green
                4 => new Color(0.4f, 0.6f, 0.9f), // Blue
                5 => new Color(0.9f, 0.9f, 0.2f), // Yellow
                6 => new Color(0.2f, 0.9f, 0.9f), // Cyan
                7 => new Color(0.9f, 0.2f, 0.9f), // Magenta
                8 => new Color(0.8f, 0.7f, 0.4f), // Gold
                _ => new Color(0.7f, 0.7f, 0.7f) // Default
            };
        }

        public static Settings Current => Instance ?? new Settings();
    }
}