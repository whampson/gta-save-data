namespace GTASaveData.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GTAObject : ObservableObject
    {
        // NOTE TO INHERITORS:
        // To make an object serializable, you must do the following:
        //   1) Create a serialization function, preferably protected or private,
        //      with one of the following signatures:
        //          void Serialize(Serializer)
        //          void Serialize(Serializer, SystemType)
        //   2) Create a deserialization constructor, preferably protected or
        //      private, with one of the following signatures:
        //          Ctor(Serializer)
        //          Ctor(Serializer, SystemType)

        protected static string BuildToString(params (string, object)[] fields)
        {
            string s = "{ ";
            foreach (var f in fields)
            {
                s += string.Format("{0} = {1}, ", f.Item1, f.Item2);
            }

            return s.Substring(0, s.Length - 2) + " }";
        }
    }
}
