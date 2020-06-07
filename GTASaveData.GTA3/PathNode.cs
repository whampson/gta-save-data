using System;
using WpfEssentials;

namespace GTASaveData.GTA3
{
    public class PathNode : ObservableObject,
        IEquatable<PathNode>, IDeepClonable<PathNode>
    {
        private bool m_disabled;
        private bool m_betweenLevels;

        public bool Disabled
        { 
            get { return m_disabled; }
            set { m_disabled = value; OnPropertyChanged(); }
        }

        public bool BetweenLevels
        { 
            get { return m_betweenLevels; }
            set { m_betweenLevels = value; OnPropertyChanged(); }
        }

        public PathNode()
        { }

        public PathNode(PathNode other)
        {
            Disabled = other.Disabled;
            BetweenLevels = other.BetweenLevels;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PathNode);
        }

        public bool Equals(PathNode other)
        {
            if (other == null)
            {
                return false;
            }

            return Disabled.Equals(other.Disabled)
                && BetweenLevels.Equals(other.BetweenLevels);
        }

        public PathNode DeepClone()
        {
            return new PathNode(this);
        }
    }
}
