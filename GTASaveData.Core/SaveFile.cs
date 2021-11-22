using GTASaveData.Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace GTASaveData
{
    /// <summary>
    /// Represents the saved state of a <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class SaveFile : SaveDataObject
    {
        /// <summary>
        /// Arbitrary limit that should be larger than any GTA save file
        /// to prevent unnecessarily large reads.
        /// </summary>
        private const int FileSizeMax = 0x200000;

        private FileFormat m_fileFormat;
        private Game m_game;

        /// <summary>
        /// The internal name of the save file.
        /// </summary>
        public abstract string Title { get; set; }

        /// <summary>
        /// The time the file was last saved.
        /// </summary>
        public abstract DateTime TimeStamp { get; set; }

        /// <summary>
        /// A descriptor for the file type which controls how data is serialized.
        /// </summary>
        public FileFormat FileType
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The <i>Grand Theft Auto</i> game this save belongs to.
        /// </summary>
        public Game Game
        {
            get { return m_game; }
            set { m_game = value; OnPropertyChanged(); }
        }

        // TODO: other useful/common structres?

        protected SaveFile(Game game)
        {
            Game = game;
        }

        /// <summary>
        /// Attempts to determine the <see cref="FileFormat"/> of the data in the specified buffer.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to.</typeparam>
        /// <param name="buf">The buffer to parse.</param>
        /// <param name="fmt">The detected <see cref="FileFormat"/>, if successful.</param>
        /// <returns>True if the detection was successful, false otherwise.</returns>
        public static bool TryGetFileType<T>(byte[] buf, out FileFormat fmt) where T : SaveFile, new()
        {
            return new T().DetectFileType(buf, out fmt);
        }

        /// <summary>
        /// Attempts to determine the <see cref="FileFormat"/> of the specified save file.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to.</typeparam>
        /// <param name="path">The path to the file to parse.</param>
        /// <param name="fmt">The detected <see cref="FileFormat"/>, if successful.</param>
        /// <returns>True if the detection was successful, false otherwise.</returns>
        public static bool TryGetFileType<T>(string path, out FileFormat fmt) where T : SaveFile, new()
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                if (info.Length > FileSizeMax)
                {
                    goto Fail;
                }

                byte[] data = File.ReadAllBytes(path);
                return TryGetFileType<T>(data, out fmt);
            }

        Fail:
            fmt = FileFormat.Default;
            return false;
        }

        /// <summary>
        /// Attempts to load a <see cref="SaveFile"/> from the data in the specified buffer.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to load.</typeparam>
        /// <param name="buf">The buffer to read.</param>
        /// <param name="saveFile">The deserialized <see cref="SaveFile"/>, if successful.</param>
        /// <returns>True if the load was sucessful, false otherwise.</returns>
        public static bool TryLoad<T>(byte[] buf, out T saveFile) where T : SaveFile, new()
        {
            try
            {
                saveFile = Load<T>(buf);
                return saveFile != null;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = default;
            return false;
        }

        /// <summary>
        /// Loads a <see cref="SaveFile"/> from the specified buffer.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to load.</typeparam>
        /// <param name="buf">The buffer to read.</param>
        /// <returns>
        /// A new <see cref="SaveFile"/> object containing the deserialized save data,
        /// or null if loading was not successful.
        /// </returns>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="SerializationException"/>
        public static T Load<T>(byte[] buf) where T : SaveFile, new()
        {
            return Load<T>(buf, FileFormat.Default);
        }

        /// <summary>
        /// Loads a <see cref="SaveFile"/> from the specified buffer.
        /// </summary>
        /// /// <remarks>
        /// If the <paramref name="fmt"/> is <see cref="FileFormat.Default"/>, the
        /// file type will be detected automatically.
        /// </remarks>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to load.</typeparam>
        /// <param name="buf">The buffer to read.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is read.</param>
        /// <returns>
        /// A new <see cref="SaveFile"/> object containing the deserialized save data,
        /// or null if loading was not successful.
        /// </returns>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="SerializationException"/>
        public static T Load<T>(byte[] buf, FileFormat fmt) where T : SaveFile, new()
        {
            if (fmt == FileFormat.Default)
            {
                if (!TryGetFileType<T>(buf, out fmt))
                {
                    return null;
                }
            }

            return Serializer.Read<T>(buf, fmt);
        }

        /// <summary>
        /// Attempts to load a <see cref="SaveFile"/> from the file at the specified path.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to load.</typeparam>
        /// <param name="path">The path to the file to read.</param>
        /// <param name="saveFile">The deserialized <see cref="SaveFile"/>, if successful.</param>
        /// <returns>True if the load was sucessful, false otherwise.</returns>
        public static bool TryLoad<T>(string path, out T saveFile) where T : SaveFile, new()
        {
            try
            {
                saveFile = Load<T>(path);
                return saveFile != null;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = default;
            return false;
        }

        /// <summary>
        /// Loads a <see cref="SaveFile"/> from the specified buffer.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to load.</typeparam>
        /// <param name="path">The path to the file to read.</param>
        /// <returns>
        /// A new <see cref="SaveFile"/> object containing the deserialized save data,
        /// or null if loading was not successful.
        /// </returns>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="SerializationException"/>
        public static T Load<T>(string path) where T : SaveFile, new()
        {
            return Load<T>(path, FileFormat.Default);
        }

        /// <summary>
        /// Loads a <see cref="SaveFile"/> from the specified buffer.
        /// </summary>
        /// <remarks>
        /// If the <paramref name="fmt"/> is <see cref="FileFormat.Default"/>, the
        /// file type will be detected automatically.
        /// </remarks>
        /// <typeparam name="T">The type of <see cref="SaveFile"/> to load.</typeparam>
        /// <param name="path">The path to the file to read.</param>
        /// <param name="fmt">A <see cref="FileFormat"/> descriptor controlling how data is read.</param>
        /// <returns>
        /// A new <see cref="SaveFile"/> object containing the deserialized save data,
        /// or null if loading was not successful.
        /// </returns>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="SerializationException"/>
        public static T Load<T>(string path, FileFormat fmt) where T : SaveFile, new()
        {
            if (fmt == FileFormat.Default)
            {
                if (!TryGetFileType<T>(path, out fmt))
                {
                    return null;
                }
            }

            T obj = new T() { FileType = fmt };
            obj.LoadFromFile(path);

            return obj;
        }

        private void LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(Strings.Error_PathNotFound, path));
            }

            FileInfo info = new FileInfo(path);
            if (info.Length > FileSizeMax)
            {
                throw new InvalidDataException(Strings.Error_InvalidData_FileTooLarge);
            }

            OnFileLoading(path);
            byte[] buf = File.ReadAllBytes(path);
            _ = Serializer.Read(this, buf, FileType);
            OnFileLoad(path);
        }

        /// <summary>
        /// Saves all data to a buffer.
        /// </summary>
        /// <param name="buf">The output buffer.</param>
        /// <returns>The number of bytes written.</returns>
        public int Save(out byte[] buf)
        {
            return Serializer.Write(this, FileType, out buf);
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

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int mark = buf.Position;
            FileType = fmt;

            OnLoading();
            Load(buf);
            OnLoad();

            int bytesRead = buf.Position - mark;
            Debug.WriteLine($"Load successful! {bytesRead} total bytes read");
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            int mark = buf.Position;
            FileType = fmt;

            OnSaving();
            Save(buf);
            OnSave();

            int bytesWritten = buf.Position - mark;
            Debug.WriteLine($"Save successful! {bytesWritten} total bytes written");
        }

        /// <summary>
        /// Deserializes all data from the buffer.
        /// </summary>
        /// <param name="buf">The buffer to read from.</param>
        protected abstract void Load(DataBuffer buf);

        /// <summary>
        /// Serializes all data to the buffer.
        /// </summary>
        /// <param name="buf">The buffer to write into.</param>
        protected abstract void Save(DataBuffer buf);


        protected abstract bool DetectFileType(byte[] buf, out FileFormat fmt);

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
