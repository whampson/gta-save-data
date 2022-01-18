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
        /// The internal name of the save file.
        /// </summary>
        public abstract string Title { get; set; }

        /// <summary>
        /// The time the file was last saved.
        /// </summary>
        public abstract DateTime TimeStamp { get; set; }

        /// <summary>
        /// The <i>Grand Theft Auto</i> game this save belongs to.
        /// </summary>
        public Game Game
        {
            get { return m_game; }
            set { m_game = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The serialization parameters used to read and write this save.
        /// </summary>
        public P Params
        {
            get { return m_params; }
            set { m_params = value ?? new P(); OnPropertyChanged(); }
        }

        protected SaveFile(Game game)
        {
            Game = game;
            Params = new P();
        }

        protected int LoadInternal(string path)
        {
            if (!File.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(Strings.Error_PathNotFound, path));
            }

            FileInfo info = new FileInfo(path);
            if (info.Length > MaxFileSize)
            {
                throw new InvalidDataException(Strings.Error_InvalidData_FileTooLarge);
            }

            OnFileLoading(path);
            byte[] buf = File.ReadAllBytes(path);
            int bytesRead = LoadInternal(buf);
            OnFileLoad(path);

            return bytesRead;
        }

        protected int LoadInternal(byte[] buf)
        {
            return Serializer.Read(this, buf, Params);
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
            return Serializer.Write(this, Params, out buf);
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
        /// Deserializes all data from the buffer.
        /// </summary>
        protected abstract void Load(DataBuffer buf);

        /// <summary>
        /// Serializes all data to the buffer.
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
