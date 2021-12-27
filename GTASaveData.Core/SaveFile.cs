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
        /// <summary>
        /// Arbitrary limit that should be larger than any GTA save file
        /// to prevent unnecessarily large reads.
        /// </summary>
        private const int FileSizeMax = 0x200000;

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

        /// <summary>
        /// Gets the current <see cref="FileType"/> for this save.
        /// </summary>
        /// <remarks>
        /// The file type is stored in the serialization parameters.
        /// </remarks>
        public FileType GetFileType()
        {
            return Params.FileType;
        }

        /// <summary>
        /// Attempts to determine the <see cref="FileType"/> of the specified save file.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile{P}"/> to.</typeparam>
        /// <param name="path">The path to the file to parse.</param>
        /// <param name="t">The detected <see cref="FileType"/>, if successful.</param>
        /// <returns>True if the detection was successful, false otherwise.</returns>
        public static bool TryGetFileType<T>(string path, out FileType t)
            where T : SaveFile<P>, new()
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                if (info.Length > FileSizeMax)
                {
                    goto Fail;
                }

                byte[] data = File.ReadAllBytes(path);
                return TryGetFileType<T>(data, out t);
            }

        Fail:
            t = FileType.Default;
            return false;
        }

        /// <summary>
        /// Attempts to determine the <see cref="FileType"/> of the data in the specified buffer.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="SaveFile{P}"/> to.</typeparam>
        /// <param name="buf">The buffer to parse.</param>
        /// <param name="t">The detected <see cref="FileType"/>, if successful.</param>
        /// <returns>True if the detection was successful, false otherwise.</returns>
        public static bool TryGetFileType<T>(byte[] buf, out FileType t)
            where T : SaveFile<P>, new()
        {
            return new T().DetectFileType(buf, out t);
        }

        public static bool TryLoad<T>(string path, out T saveFile)
            where T : SaveFile<P>, new()
        {
            return TryLoad(path, FileType.Default, out saveFile);
        }

        public static bool TryLoad<T>(string path, FileType t, out T saveFile)
            where T : SaveFile<P>, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            try
            {
                saveFile = Load<T>(path, t);
                return true;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = null;
            return false;
        }

        public static bool TryLoad<T>(string path, P param, out T saveFile)
            where T : SaveFile<P>, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (param == null) throw new ArgumentNullException(nameof(param));

            try
            {
                saveFile = Load<T>(path, param);
                return true;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = null;
            return false;
        }

        public static bool TryLoad<T>(byte[] buf, P param, out T saveFile)
            where T : SaveFile<P>, new()
        {
            if (buf == null) throw new ArgumentNullException(nameof(buf));
            if (param == null) throw new ArgumentNullException(nameof(param));

            try
            {
                saveFile = Load<T>(buf, param);
                return true;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = null;
            return false;
        }

        public static T Load<T>(string path)
            where T : SaveFile<P>, new()
        {
            return Load<T>(path, FileType.Default);
        }

        /// <summary>
        /// Opens a <see cref="SaveFile{P}"/> from the specified file path.
        /// </summary>
        /// <remarks>
        /// If <paramref name="t"/> is <see cref="FileType.Default"/>,
        /// <see cref="TryGetFileType{T}(string, out FileType)"/> will be used
        /// to determine the file type. If unsuccessful, <c>null</c> will be returned.
        /// </remarks>
        /// <typeparam name="T">
        /// The type of <see cref="SaveFile{P}"/> to load.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="SaveFile{P}"/> instance containing deserialized data from
        /// the specified file, if successful. <c>null</c> if the file type could not
        /// be determined or if no <see cref="SerializationParams"/> could be found for
        /// the given file type.
        /// </returns>
        /// <exception cref="SerializationException"/>
        /// <exception cref="InvalidDataException"/>
        public static T Load<T>(string path, FileType t)
            where T : SaveFile<P>, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (t == FileType.Default)
            {
                if (!TryGetFileType<T>(path, out t))
                {
                    return null;
                }
            }

            var s = new T();
            var p = SerializationParams.GetDefaults<P>(t);
            if (p == null)
            {
                return null;
            }

            s.Params = p;
            s.LoadFromFile(path);

            return s;
        }

        public static T Load<T>(string path, P param)
            where T : SaveFile<P>, new()
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (param == null) throw new ArgumentNullException(nameof(param));

            T s = new T { Params = param };
            s.LoadFromFile(path);

            return s;
        }

        public static T Load<T>(byte[] buf, P param)
            where T : SaveFile<P>, new()
        {
            if (buf == null) throw new ArgumentNullException(nameof(buf));
            if (param == null) throw new ArgumentNullException(nameof(param));

            T s = Serializer.Read<T>(buf, param);
            s.Params = param;

            return s;
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
            _ = Serializer.Read(this, buf, Params);
            OnFileLoad(path);
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
        /// Detects the <see cref="FileType"/> of the buffer and returns a value
        /// indicating whether detection was successful.
        /// </summary>
        protected abstract bool DetectFileType(byte[] buf, out FileType fmt);

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
