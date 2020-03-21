using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class TimeStep : NonSerializableObject,
        IEquatable<TimeStep>
    {
        private float m_timeStep;
        private float m_framesPerUpdate;
        private float m_timeScale;

        public float Step
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

            return m_timeStep.Equals(other.m_timeStep)
                && m_framesPerUpdate.Equals(other.m_framesPerUpdate)
                && m_timeScale.Equals(other.m_timeScale);
        }
    }
}
