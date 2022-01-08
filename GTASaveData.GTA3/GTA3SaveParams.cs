using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Serialization parameters for GTA3 save files.
    /// </summary>
    public class GTA3SaveParams : SerialiationParamsGTA3VC
    {
        public const int DefaultWorkBufferSize = 55000;
        public const int DefaultWorkBufferSizePS2 = 50000;
        public const int DefaultMaxNumPaddingBlocks = 4;
        public const int DefaultLastMissionPassedNameLength = 24;
        public const int DefaultNumContacts = 16;
        public const int DefaultNumCollectives = 32;
        public const int DefaultNumBuildingSwaps = 25;
        public const int DefaultNumInvisibilitySettings = 20;
        public const int DefaultNumLocalVariables = 16;
        public const int DefaultStackDepth = 6;
        public const int DefaultStackDepthPS2 = 4;
        public const int DefaultNumGarages = 32;


        // ---------- General Save File ----------

        /// <summary>
        /// The maximum number of padding blocks that can be written to a save file.
        /// </summary>
        public int MaxNumPaddingBlocks { get; set; }


        // ---------- SimpleVariables ----------

        /// <summary>
        /// The total number of characters to write for the save title
        /// (<see cref="SimpleVariables.LastMissionPassedName"/>).
        /// </summary>
        /// <remarks>
        /// Does not apply to <i>The Definitive Edition</i>.
        /// </remarks>
        public int LastMissionPassedNameLength { get; set; }


        // ---------- ScriptBlock ----------

        /// <summary>
        /// The number of items to store in the <see cref="ScriptBlock.Contacts"/> array.
        /// </summary>
        public int NumContacts { get; set; }

        /// <summary>
        /// The number of items to store in the <see cref="ScriptBlock.Collectives"/> array.
        /// </summary>
        public int NumCollectives { get; set; }

        /// <summary>
        /// The number of items to store in the <see cref="ScriptBlock.BuildingSwaps"/> array.
        /// </summary>
        public int NumBuildingSwaps { get; set; }

        /// <summary>
        /// The number of items to store in the <see cref="ScriptBlock.InvisibilitySettings"/> array.
        /// </summary>
        public int NumInvisibilitySettings { get; set; }


        // ---------- RunningScript ----------
        
        /// <summary>
        /// The number of local variables to store per script.
        /// </summary>
        public int NumLocalVariables { get; set; }

        /// <summary>
        /// The maximum number of <c>gosub</c> return addresses that can be stored
        /// on the stack.
        /// </summary>
        public int StackDepth { get; set; }


        // ---------- GarageBlock ----------

        /// <summary>
        /// The number of items to store in the <see cref="GarageBlock.Garages"/> array.
        /// </summary>
        public int NumGarages { get; set; }


        public GTA3SaveParams() : base()
        {
            WorkBufferSize = DefaultWorkBufferSize;
            MaxNumPaddingBlocks = DefaultMaxNumPaddingBlocks;
            LastMissionPassedNameLength = DefaultLastMissionPassedNameLength;
            NumContacts = DefaultNumContacts;
            NumCollectives = DefaultNumCollectives;
            NumBuildingSwaps = DefaultNumBuildingSwaps;
            NumInvisibilitySettings = DefaultNumInvisibilitySettings;
            NumLocalVariables = DefaultNumLocalVariables;
            StackDepth = DefaultStackDepth;
            NumGarages = DefaultNumGarages;
        }

        public GTA3SaveParams(GTA3SaveParams other) : base(other)
        {
            MaxNumPaddingBlocks = other.MaxNumPaddingBlocks;
            LastMissionPassedNameLength = other.LastMissionPassedNameLength;
            NumContacts = other.NumContacts;
            NumCollectives = other.NumCollectives;
            NumBuildingSwaps = other.NumBuildingSwaps;
            NumInvisibilitySettings = other.NumInvisibilitySettings;
            StackDepth = other.StackDepth;
            NumLocalVariables = other.NumLocalVariables;
        }

        /// <summary>
        /// Gets the default serialization params for a given <see cref="FileType"/>.
        /// </summary>
        /// <remarks>
        /// Supported file types for GTA3 saves are located in <see cref="GTA3Save.FileTypes"/>.
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

            GTA3SaveParams p = new GTA3SaveParams() { FileType = t };
            if (t.IsPS2)
            {
                p.WorkBufferSize = DefaultWorkBufferSizePS2;
                p.StackDepth = DefaultStackDepthPS2;
            }

            return p;
        }

        protected override T GetDefaultsInternal<T>(FileType t)
        {
            return GetDefaults(t) as T;
        }
    }
}
