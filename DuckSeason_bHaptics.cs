using System;
using System.Collections.Generic;
using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using UnityEngine;
using System.Threading;

namespace DuckSeason_bHaptics
{
    public class DuckSeason_bHaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;

        public override void OnApplicationStart()
        {
            InitializeProTube();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        /*
         * There is not real dual wield in this game so I decided not to
         * manage dual wield
         **/
        private void InitializeProTube()
        {
            MelonLogger.Msg("Initializing ProTube gear...");
            ForceTubeVRInterface.InitAsync(true);
            Thread.Sleep(10000);
        }

        public static bool isRightHanded()
        {
            return DuckSeasonManager.instance.isRightHanded;
        }
                
        [HarmonyPatch(typeof(AudioControl), "FireAudio")]
        public class bhaptics_Shoot
        {
            [HarmonyPostfix]
            public static void Postfix(bool reverbDir)
            {
                MelonLogger.Msg(isRightHanded());
                tactsuitVr.PlaybackHaptics("RecoilVest_" +
                    (isRightHanded() ? "R" : "L"));
                tactsuitVr.PlaybackHaptics("RecoilArm_" +
                    (isRightHanded() ? "R" : "L"));
                ForceTubeVRInterface.Shoot(255, 255, 50f, ForceTubeVRChannel.all);
            }
        }
        
        [HarmonyPatch(typeof(AudioControl), "RackBackSound")]
        public class bhaptics_Pump1
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("RecoilVest_" +
                    (isRightHanded() ? "L" : "R"), 0.5f);
                tactsuitVr.PlaybackHaptics("RecoilArm_" +
                    (isRightHanded() ? "L" : "R"), 0.5f);
            }
        }

        [HarmonyPatch(typeof(AudioControl), "RackForwardSound")]
        public class bhaptics_Pump2
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.PlaybackHaptics("RecoilVest_" +
                    (isRightHanded() ? "L" : "R"), 0.5f);
                tactsuitVr.PlaybackHaptics("RecoilArm_" +
                    (isRightHanded() ? "L" : "R"), 0.5f);
            }
        }
        
        [HarmonyPatch(typeof(TubeLoadedShells), "AddShell")]
        public class bhaptics_Reload
        {
            [HarmonyPostfix]
            public static void Postfix(bool chamberLoaded)
            {
                MelonLogger.Msg("RELOAD");
                if (!chamberLoaded)
                {
                    tactsuitVr.PlaybackHaptics("RecoilArm_" +
                        (isRightHanded() ? "L" : "R"), 0.5f);
                    ForceTubeVRInterface.Rumble(60, 25f, ForceTubeVRChannel.all);
                }
            }
        }
    }
}
