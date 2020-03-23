using GTASaveData.Types;
using System;

namespace GTASaveData.VC
{
    public class TimeStep : GTAObject, IEquatable<TimeStep>
    {
        private float m_timeStep;
        private float m_framesPerUpdate;
        private float m_timeScale;

        public float TimeStepValue
        {
            get { return m_timeStep; }
            set { m_timeStep = value; OnPropertyChanged(); }
        }

        public float FramesPerUpdate
        {
            get { return m_framesPerUpdate; }
            set { m_framesPerUpdate = value; OnPropertyChanged(); }
        }

        public float TimeScale
        {
            get { return m_timeScale; }
            set { m_timeScale = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TimeStep);
        }

        public bool Equals(TimeStep other)
        {
            if (other == null)
            {
                return false;
            }

            return TimeStepValue.Equals(other.TimeStepValue)
                && FramesPerUpdate.Equals(other.FramesPerUpdate)
                && TimeScale.Equals(other.TimeScale);
        }
    }
}
