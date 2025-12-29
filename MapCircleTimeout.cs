using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace labile.Bannerlord.NoRelation
{
    public static class CircleTimer
    {
        public static readonly Dictionary<MapNotificationItemBaseVM, float> ExpirationTimes = new();
    }

    [HarmonyPatch(typeof(MapNotificationVM))]
    public static class PatchMapCirclesTimeout
    {
        [HarmonyPatch("AddMapNotification")]
        [HarmonyPostfix]
        public static void Postfix_Add(MapNotificationVM __instance, InformationData data)
        {
            var timeout = Settings.Current.MapCircleTimeout;
            if (timeout <= 0) return;

            if (__instance.NotificationItems.Count <= 0) return;
            var newItem = __instance.NotificationItems[__instance.NotificationItems.Count - 1];

            if (newItem.Data == data && !CircleTimer.ExpirationTimes.ContainsKey(newItem))
            {
                CircleTimer.ExpirationTimes[newItem] = Time.ApplicationTime + timeout;
            }
        }

        [HarmonyPatch("OnFrameTick")]
        [HarmonyPostfix]
        public static void Postfix_Tick(MapNotificationVM __instance, float dt)
        {
            var timeout = Settings.Current.MapCircleTimeout;
            if (timeout <= 0) return;


            if (__instance.NotificationItems.Count == 0)
            {
                if (CircleTimer.ExpirationTimes.Count > 0) CircleTimer.ExpirationTimes.Clear();

                return;
            }



            var now = Time.ApplicationTime;
            var toRemove = new List<MapNotificationItemBaseVM>();

            foreach (var item in __instance.NotificationItems)
            {
                if (!CircleTimer.ExpirationTimes.TryGetValue(item, out var expireTime)) continue;
                if (now >= expireTime)
                {
                    toRemove.Add(item);
                }
            }

            foreach (var item in toRemove)
            {
                try
                {
                    item.ExecuteRemove();
                }
                catch
                {
                    // ignored
                }

                CircleTimer.ExpirationTimes.Remove(item);
            }
        }
    }
}
