namespace GTASaveData.Common
{
    public interface ISimpleVars
    {
        string SaveName { get; set; }

        SystemTime SaveTime { get; set; }

        Vector3d CameraPosition { get; set; }

        int MillisecondsPerGameMinute { get; set; }

        uint WeatherTimer { get; set; }

        int GameHour { get; set; }

        int GameMinute { get; set; }

        uint GlobalTimer { get; set; }

        int PreviousWeather { get; set; }

        int CurrentWeather { get; set; }

        int ForcedWeather { get; set; }

        float WeatherInterpolation { get; set; }

        int WeatherTypeInList { get; set; }

        // pad mode?
        // on foot camera?
        // in car camera?
    }
}
