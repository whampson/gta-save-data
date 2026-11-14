using System.ComponentModel;

﻿namespace GTASaveData.LCS
{
    /// <summary>
    /// Identifies the mission script (main.scm) that produced a save, as recorded
    /// in <see cref="ScriptData.MainScriptSize"/>. This is a *separate* axis from
    /// the <see cref="FileType"/>: the <see cref="FileType"/> describes the save-file
    /// layout (pointer width), while <see cref="ScriptVersion"/> describes the script
    /// content shipped with the game build.
    /// </summary>
    /// <remarks>
    /// The mobile platforms ship two distinct scripts that are independent of pointer
    /// width: Android (32-bit *and* 64-bit) uses the <see cref="Android"/> script, while
    /// iOS (32-bit *and* 64-bit) uses the <see cref="iOS"/> script. A 64-bit Android save
    /// therefore has a <see cref="FileType"/> of <c>Mobile64</c> but a
    /// <see cref="ScriptVersion"/> of <see cref="Android"/>.
    /// </remarks>
    public enum ScriptVersion
    {
        Unknown,
        Android,
        iOS,
        PS2,
        PSP,
    }

    /// <summary>
    /// Identifies the specific game build that produced a save, derived from the
    /// <see cref="ScriptData.MainScriptSize"/>. This is more granular than
    /// <see cref="ScriptVersion"/>: the PSP platform alone shipped several
    /// regional/revision variants of main.scm, each with a distinct size.
    /// </summary>
    public enum GameVersion
    {
        [Description("Unknown/Modified")]
        Unknown,

        [Description("Android")]
        Android,

        [Description("iOS")]
        iOS,

        [Description("PS2")]
        PS2,

        [Description("PSP v1.05 (US/EU)")]
        PSP105,

        [Description("PSP v3.00 (US/EU)")]
        PSP300,

        [Description("PSP v1.01 (Japan)")]
        PSPJapan,

        [Description("PSP (unidentified version)")]
        PSPUnidentified,
    }

    /// <summary>
    /// Helpers for deriving a <see cref="ScriptVersion"/> from the
    /// <see cref="ScriptData.MainScriptSize"/> stored in a save.
    /// </summary>
    public static class ScriptVersions
    {
        public const int MainScriptSizeAndroid = 274282;
        public const int MainScriptSizeiOS = 274312;
        public const int MainScriptSizePS2 = 272710;        // PS2 v1.01 (US/EU; SLUS-21423/SLES-54135)
        public const int MainScriptSizePSP105 = 270081;     // PSP v1.05 original (US/EU; ULUS-10041/ULES-00151; MAIN.SCM + EU saves)
        public const int MainScriptSizePSP300 = 269916;     // PSP v3.00 "Platinum" (EU; ULES-00151; MAIN.SCM + EU saves)
        public const int MainScriptSizePSPJapan = 270189;   // PSP v1.01 original (JP; ULJM-05255; JP saves)
        public const int MainScriptSizePSPUnk1 = 270049;    // observed in saves; maybe v2.00 (US/EU)
        public const int MainScriptSizePSPUnk2 = 270003;    // observed in saves; maybe v3.00 (US "Greatest Hits")

        public static ScriptVersion FromMainScriptSize(int mainScriptSize)
        {
            switch (mainScriptSize)
            {
                case MainScriptSizeAndroid: return ScriptVersion.Android;
                case MainScriptSizeiOS: return ScriptVersion.iOS;
                case MainScriptSizePS2: return ScriptVersion.PS2;
                case MainScriptSizePSP105:
                case MainScriptSizePSP300:
                case MainScriptSizePSPJapan:
                case MainScriptSizePSPUnk1:
                case MainScriptSizePSPUnk2: return ScriptVersion.PSP;
                default: return ScriptVersion.Unknown;
            }
        }

        /// <summary>
        /// Derives the specific <see cref="GameVersion"/> from the
        /// <see cref="ScriptData.MainScriptSize"/> stored in a save. Returns
        /// <see cref="GameVersion.Unknown"/> when the size does not match any
        /// known official build.
        /// </summary>
        public static GameVersion GetGameVersion(int mainScriptSize)
        {
            switch (mainScriptSize)
            {
                case MainScriptSizeAndroid: return GameVersion.Android;
                case MainScriptSizeiOS: return GameVersion.iOS;
                case MainScriptSizePS2: return GameVersion.PS2;
                case MainScriptSizePSP105: return GameVersion.PSP105;
                case MainScriptSizePSP300: return GameVersion.PSP300;
                case MainScriptSizePSPJapan: return GameVersion.PSPJapan;
                case MainScriptSizePSPUnk1:
                case MainScriptSizePSPUnk2: return GameVersion.PSPUnidentified;
                default: return GameVersion.Unknown;
            }
        }
    }
}
