using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace labile.Bannerlord.NoRelation
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                new Harmony("labile.Bannerlord.NoRelation").PatchAll();
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage("NoRelation Error: " + ex.Message, Colors.Red)
                );
            }
        }
    }

    [HarmonyPatch(typeof(GameNotificationVM), "AddGameNotification")]
    public static class RelationNotification
    {
        public static bool Prefix(string notificationText, string soundId)
        {
            if (!Settings.Current.DisableRelationNotifications) return true;

            if (!soundId.Contains("relation")) return true;

            InformationManager.DisplayMessage(new InformationMessage(notificationText, Settings.Current.GetLogColor()));

            return false;
        }
    }

    [HarmonyPatch(typeof(MBInformationManager), "AddQuickInformation")]
    public static class Notifications
    {
        public static bool Prefix(TextObject message, string soundEventPath)
        {
            if (!Settings.Current.DisableAllNotifications) return true;
            InformationManager.DisplayMessage(
                new InformationMessage(message.ToString(), Settings.Current.GetLogColor()));
            return false;
        }
    }

    [HarmonyPatch(typeof(SoundEvent), "PlaySound2D", typeof(string))]
    public static class NotificationsSound
    {
        public static bool Prefix(string soundName)
        {
            if (!Settings.Current.DisableNotificationSounds) return true;

            return !soundName.Contains("event:/ui/notification");
        }
    }


    // [HarmonyPatch(typeof(MapNotificationVM), "AddMapNotification")]
    // public static class MapCircles
    // {
    //     public static bool Prefix(InformationData data)
    //     {
    //         {
    //             var title = data.TitleText?.ToString() ?? "";
    //             var desc = data.DescriptionText?.ToString() ?? "";
    //
    //             var logMessage = string.IsNullOrEmpty(desc) ? title : $"{title}: {desc}";
    //
    //             if (!string.IsNullOrWhiteSpace(logMessage))
    //             {
    //                 InformationManager.DisplayMessage(
    //                     new InformationMessage(logMessage, Settings.Current.GetLogColor()));
    //             }
    //         }
    //
    //         return false;
    //     }
    // }
}