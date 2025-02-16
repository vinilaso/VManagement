namespace VManagement.Commons.Utility
{
    public class FileUtility
    {
        public string FilePath { get; set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;

        public FileUtility(string path) 
        {
            FilePath = path;

            using (FileStream file = File.OpenRead(path))
            using (StreamReader reader = new StreamReader(file))
            {
                Content = reader.ReadToEnd();
            }
        }

        public string FindValue(string search)
        {
            var notFoundException = new ArgumentOutOfRangeException($"There is no value defined for the key {search}.");

            string[] keyValues = Content.Split(';');
            string targetValue = keyValues.FirstOrDefault(kv => kv.StartsWith(search))
                                 ?? throw notFoundException;

            try
            {
                return targetValue.Split('=')[1];
            }
            catch (IndexOutOfRangeException)
            {
                throw notFoundException;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Finds values defined in a text file.
        ///     String model:
        ///     key=value;key=value;...
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="search">The key to search</param>
        /// <returns></returns>
        public static string FindValue(string path, string search)
        {
            var notFoundException = new ArgumentOutOfRangeException($"There is no value defined for the key {search}.");

            using FileStream file = File.OpenRead(path);
            using StreamReader reader = new StreamReader(file);

            string source = reader.ReadToEnd();

            string[] keyValues = source.Split(';');
            string targetValue = keyValues.FirstOrDefault(kv => kv.StartsWith(search))
                                 ?? throw notFoundException;

            try
            {
                return targetValue.Split('=')[1];
            }
            catch (IndexOutOfRangeException)
            {
                throw notFoundException;
            }
            catch
            {
                throw;
            }
        }
    }
}
