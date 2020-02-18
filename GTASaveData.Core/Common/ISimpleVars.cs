namespace GTASaveData.Common
{
    public interface ISimpleVars
    {
        string LastMissionPassedName { get; set; }

        SystemTime SaveTime { get; set; }

        Vector3d CameraPosition { get; set; }

        int MillisecondsPerGameMinute { get; set; }

        uint LastClockTick { get; set; }

        int GameClockHours { get; set; }

        int GameClockMinutes { get; set; }

        uint TimeInMilliseconds { get; set; }

        int OldWeatherType { get; set; }

        int NewWeatherType { get; set; }

        int ForcedWeatherType { get; set; }

        float InterpolationValue { get; set; }

        int WeatherTypeInList { get; set; }
    }
}
