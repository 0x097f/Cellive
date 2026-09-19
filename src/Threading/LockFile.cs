namespace CilDotNet.Threading
{
    public sealed class LockFile : IDisposable
    {
        private readonly string filePath = string.Empty;
        private FileStream? fileStream;
        private bool locked;

        public LockFile()
        {
            Lock();
        }

        public void Lock()
        {
            if (locked) return;

            try
            {
                var dir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                fileStream = new FileStream(
                    filePath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
                locked = true;
            }
            catch
            {
                locked = false;
            }
        }

        public void Unlock()
        {
            if (!locked) return;

            try
            {
                fileStream?.Close();
                fileStream?.Dispose();
                fileStream = null;
                locked = false;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch { }
        }

        public bool FileLocked()
        {
            if (!File.Exists(filePath)) return false;

            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            }
            catch
            {
                return true;
            }
        }

        public void Dispose() => Unlock();
    }
}