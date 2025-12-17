using System;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
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
                InformationManager.DisplayMessage(new InformationMessage("NoRelation Error: " + ex.Message, Colors.Red));
            }
        }
    }

    [HarmonyPatch(typeof(GameNotificationVM), "AddGameNotification")]
    public static class Patch_GameNotificationVM
    {
        public static bool Prefix(string notificationText, int extraTimeInMs, BasicCharacterObject announcerCharacter, Equipment equipment, string soundId)
        {
            if (!string.IsNullOrEmpty(soundId) && soundId.Contains("relation"))
            {
                InformationManager.DisplayMessage(new InformationMessage(notificationText, new Color(0.5f, 0.6f, 0.6f)));
                return false; 
            }

            return true;
        }
    }
}