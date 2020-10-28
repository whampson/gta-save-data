using System.Numerics;

namespace GTASaveData.Interfaces
{
    public interface ISimpleVariables
    {
        Vector3 CameraPosition { get; set; }
        int MillisecondsPerGameMinute { get; set; }
        uint LastClockTick { get; set; }
        int GameClockHours { get; set; }
        int GameClockMinutes { get; set; }
        uint TimeInMilliseconds { get; set; }
        uint FrameCounter { get; set; }
        int OldWeatherType { get; set; }
        int NewWeatherType { get; set; }
        int ForcedWeatherType { get; set; }
        int WeatherTypeInList { get; set; }
    }
}
