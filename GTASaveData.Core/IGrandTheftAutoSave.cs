using GTASaveData.Common;
using GTASaveData.Common.Blocks;

namespace GTASaveData
{
    public interface IGrandTheftAutoSave
    {
        ISimpleVars SimpleVars { get; }

        ICarGeneratorBlock CarGenerators { get; }

        // TODO: add common structures 
    }
}
