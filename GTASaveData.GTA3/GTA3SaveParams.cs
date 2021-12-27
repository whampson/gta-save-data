using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Serialization parameters for GTA3 save files.
    /// </summary>
    public class GTA3SaveParams : SerialiationParamsGTA3VC
    {

        private static readonly GTA3SaveParams Defaults = new GTA3SaveParams()
        {
            WorkBufferSize = 55000,
            MaxLastMissionPassedNameLength = 24,
            MaxStackDepth = 6,
            MaxNumPaddingBlocks = 4,
            NumContacts = 16,
            NumCollectives = 32,
            NumBuildingSwaps = 25,
            NumInvisibilitySettings = 20,
            NumLocalVariables = 16
        };

        // General Save File
        public int MaxNumPaddingBlocks { get; set; }

        // SimpleVars
        public int MaxLastMissionPassedNameLength { get; set; }

        // ScriptsBlock
        public int NumContacts { get; set; }
        public int NumCollectives { get; set; }
        public int NumBuildingSwaps { get; set; }
        public int NumInvisibilitySettings { get; set; }

        // RunningScript
        public int MaxStackDepth { get; set; }
        public int NumLocalVariables { get; set; }


        public GTA3SaveParams() : base()
        { }

        public GTA3SaveParams(GTA3SaveParams other) : base(other)
        {
            MaxNumPaddingBlocks = other.MaxNumPaddingBlocks;
            MaxLastMissionPassedNameLength = other.MaxLastMissionPassedNameLength;
            NumContacts = other.NumContacts;
            NumCollectives = other.NumCollectives;
            NumBuildingSwaps = other.NumBuildingSwaps;
            NumInvisibilitySettings = other.NumInvisibilitySettings;
            MaxStackDepth = other.MaxStackDepth;
            NumLocalVariables = other.NumLocalVariables;
        }

        /// <summary>
        /// Gets the default, non-file-type-specific serialization params.
        /// </summary>
        /// <returns></returns>
        public static GTA3SaveParams GetDefaults()
        {
            return new GTA3SaveParams(Defaults);
        }

        /// <summary>
        /// Gets the default <see cref="FileType"/>-specific serialization params.
        /// </summary>
        /// <remarks>
        /// Valid file types are located in <see cref="GTA3Save.FileTypes"/>.
        /// </remarks>
        /// <returns>
        /// The default serialization params for the given file type,
        /// or <c>null</c> if the file type is not valid for GTA3 saves.
        /// </returns>
        public static GTA3SaveParams GetDefaults(FileType t)
        {
            bool isValid = GTA3Save.FileTypes.GetAll().Select(x => x == t).Any();
            if (!isValid)
            {
                return null;
            }

            GTA3SaveParams p = new GTA3SaveParams(Defaults)
            {
                FileType = t
            };
            if (t.IsPS2)
            {
                p.WorkBufferSize = 50000;
                p.MaxStackDepth = 4;
            }

            return p;
        }

        protected override T GetDefaultsInternal<T>(FileType t)
        {
            return GetDefaults(t) as T;
        }
    }
}
