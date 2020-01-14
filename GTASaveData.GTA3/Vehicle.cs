using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace GTASaveData.GTA3
{
    public abstract class Vehicle : SaveDataObject
    {
        protected ObservableCollection<byte> m_unknownArray0;
        protected Vector3d m_position;
        protected ObservableCollection<byte> m_unknownArray1;

        [JsonIgnore]
        public ObservableCollection<byte> UnknownArray0
        {
            get { return m_unknownArray0; }
            set { m_unknownArray0 = value; OnPropertyChanged(); }
        }

        public Vector3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public ObservableCollection<byte> UnknownArray1
        {
            get { return m_unknownArray1; }
            set { m_unknownArray1 = value; OnPropertyChanged(); }
        }

        public Vehicle()
        {
            m_unknownArray0 = new ObservableCollection<byte>();
            m_unknownArray1 = new ObservableCollection<byte>();
            m_position = new Vector3d();
        }
    }
}
