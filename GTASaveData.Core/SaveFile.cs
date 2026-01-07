using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

namespace GTASaveData
{
    /// <summary>
    /// Represents the saved state of a <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class SaveFile<P> : SaveDataObject
        where P : SerializationParams, new()
    {
        protected const int MaxFileSize = 0x200000;

        private Game m_game;
        private P m_params;


        /// <summary>
        /// The <i>Grand Theft Auto</i> game this save belongs to.
        /// </summary>
        public Game Game => m_game;

        /// <summary>
        /// The serialization parameters used to read and write this save.
        /// </summary>
        public P Params
        {
            get { return m_params; }
            set { m_params = value ?? new P(); OnPropertyChanged(); }
        }

        protected SaveFile(Game game) : this(game, new P()) { }

        protected SaveFile(Game game, P param)
        {
            m_game = game;
            m_params = param;
        }

        private static byte[] ReadBytesFromFile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(Strings.Error_PathNotFound, path));
            }

            FileInfo info = new FileInfo(path);
            if (info.Length > MaxFileSize)
            {
                throw new InvalidDataException(Strings.Error_InvalidData_FileTooLarge);
            }

            return File.ReadAllBytes(path);
        }

        private int DoLoad(string path)
        {
            OnFileLoading(path);
            byte[] buf = ReadBytesFromFile(path);
            int bytesRead = Serializer.Read(this, buf, Params);
            OnFileLoad(path);

            return bytesRead;
        }

        protected static T LoadFromFile<T>(string path, P param, out int bytesRead) where T : SaveFile<P>, new()
        {
            T save = new T()
            {
                Params = param ?? throw new ArgumentNullException(nameof(param))
            };

            bytesRead = save.DoLoad(path);
            return save;
        }

        protected static bool TryLoadFromFile<T>(string path, P param, out T saveData) where T : SaveFile<P>, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            try
            {
                saveData = LoadFromFile<T>(path, param, out _);
                return true;
            }
            catch (Exception ex)
            {
                if (!(ex is InvalidDataException &&
                      ex is SerializationException &&
                      ex is DirectoryNotFoundException))
                {
                    throw;
                }
            }

            saveData = null;
            return false;
        }

        public static bool TryGetFileType<T>(string path, out FileType t) where T : SaveFile<P>, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            try
            {
                byte[] data = ReadBytesFromFile(path);
                return new T().DetectFileType(data, out t);
            }
            catch (Exception ex)
            {
                if (!(ex is InvalidDataException &&
                      ex is SerializationException &&
                      ex is DirectoryNotFoundException))
                {
                    throw;
                }
            }

            t = FileType.Default;
            return false;
        }


        /// <summary>
        /// Saves all data to a file.
        /// </summary>
        /// <param name="path">The path to the file to write.</param>
        /// <returns>The number of bytes written.</returns>
        public int Save(string path)
        {
            OnFileSaving(path);
            int bytesWritten = Save(out byte[] data);
            File.WriteAllBytes(path, data);
            OnFileSave(path);

            return bytesWritten;
        }

        /// <summary>
        /// Saves all data to a buffer.
        /// </summary>
        /// <param name="buf">The output buffer.</param>
        /// <returns>The number of bytes written.</returns>
        public int Save(out byte[] buf)
        {
            return Serializer.Write(this, Params, out buf); // TODO: need to fire file saving event here...
        }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            int mark = buf.Position;

            OnLoading();
            Load(buf);
            OnLoad();

            int bytesRead = buf.Position - mark;
            Debug.WriteLine($"Load successful! {bytesRead} total bytes read");
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            int mark = buf.Position;

            OnSaving();
            Save(buf);
            OnSave();

            int bytesWritten = buf.Position - mark;
            Debug.WriteLine($"Save successful! {bytesWritten} total bytes written");
        }

        /// <summary>
        /// Attempts to determine the file type from the data provided. This function is not meant to be called directly.
        /// </summary>
        protected abstract bool DetectFileType(byte[] buf, out FileType t);

        /// <summary>
        /// Deserializes all data from the buffer. This function is not meant to be called directly.
        /// </summary>
        protected abstract void Load(DataBuffer buf);

        /// <summary>
        /// Serializes all data to the buffer. This function is not meant to be called directly.
        /// </summary>
        protected abstract void Save(DataBuffer buf);

        /// <summary>
        /// Called immediately before data is deserialized.
        /// </summary>
        protected virtual void OnLoading() { }

        /// <summary>
        /// Called immediately after data is deserialized.
        /// </summary>
        protected virtual void OnLoad() { }

        /// <summary>
        /// Called immediately before data is serialized.
        /// </summary>
        protected virtual void OnSaving() { }

        /// <summary>
        /// Called immediately after data is serialized.
        /// </summary>
        protected virtual void OnSave() { }

        /// <summary>
        /// Called immediately before file data is loaded into memory and deserialized.
        /// </summary>
        protected virtual void OnFileLoading(string path) { }

        /// <summary>
        /// Called immediately after file data is loaded into memory and deserialized.
        /// </summary>
        protected virtual void OnFileLoad(string path) { }

        /// <summary>
        /// Called immediately before file data is serialized and written to disk.
        /// </summary>
        protected virtual void OnFileSaving(string path) { }

        /// <summary>
        /// Called immediately after file data is serialized and written to disk.
        /// </summary>
        protected virtual void OnFileSave(string path) { }
    }
}
