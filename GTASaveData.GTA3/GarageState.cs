using System.ComponentModel;

namespace GTASaveData.GTA3
{
    public enum GarageState
    {
        [Description("Closed")]
        Closed,

        [Description("Opened")]
        Opened,

        [Description("Closing")]
        Closing,

        [Description("Opening")]
        Opening,

        [Description("Opened With Serviced Car")]
        OpenedWithServicedCar,

        [Description("Closed With Dropped Off Car")]
        ClosedWithDroppedOffCar
    }
}
