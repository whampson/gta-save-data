using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class Timer : GTAObject,
        IEquatable<Timer>
    {
        private uint m_timeInMilliseconds;
        private float m_timeScale;
        private float m_timeStep;
        private float m_timeStepNonClipped;
        private int m_frameCounter;

        public uint TimeInMilliseconds
        {
            get { return m_timeInMilliseconds; }
            set { m_timeInMilliseconds = value; OnPropertyChanged(); }
        }

        public float TimeScale
        {
            get { return m_timeScale; }
            set { m_timeScale = value; OnPropertyChanged(); }
        }

        public float TimeStep
        {
            get { return m_timeStep; }
            set { m_timeStep = value; OnPropertyChanged(); }
        }

        public float TimeStepNonClipped
        {
            get { return m_timeStepNonClipped; }
            set { m_timeStepNonClipped = value; OnPropertyChanged(); }
        }

        public int FrameCounter
        {
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Timer);
        }

        public bool Equals(Timer other)
        {
            if (other == null)
            {
                return false;
            }

            return m_timeInMilliseconds.Equals(other.m_timeInMilliseconds)
                && m_timeScale.Equals(other.m_timeScale)
                && m_timeStep.Equals(other.m_timeStep)
                && m_timeStepNonClipped.Equals(other.m_timeStepNonClipped)
                && m_frameCounter.Equals(other.m_frameCounter);
        }
    }
}
