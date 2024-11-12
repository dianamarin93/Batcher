namespace Batcher
{
    public class TrimCommand : BaseCommand
    {
        private string _path;
        private int _trimStart;
        private int _trimEnd;

        private const string PathArgument = "-p";
        private const string TrimStartArgument = "--trim-start";
        private const string TrimEndArgument = "--trim-end";

        private const int InvalidTrimValue = -1;

        public override Dictionary<string, string> AllowedArguments { get;  } = new Dictionary<string, string>()
        {
            { PathArgument, "Full path of the folder to be used" },
            {TrimStartArgument, "Specify a number of chars to be trimmed from the start of the file name" },
            {TrimEndArgument, "Specify a number of chars to be trimmed from the end of the file name" }
        };

        public override void Apply(string[] arguments)
        {
            Build(arguments);
            if (!ArgumentsAreValid())
            {
                Console.WriteLine("No trimming done");
                return;
            }

            var files = Directory.GetFiles(_path);

            foreach (var file in files)
            {
                if (!File.Exists(file)) continue;

                var currentFile = file;
                var trimmedFile = GetTrimmedFileName(currentFile);

                try
                {
                    File.Move(currentFile, trimmedFile);

                }
                catch (Exception ex)
                {

                    Errors.Add($"Error trimming [{currentFile}]: {ex.Message}");
                    continue;
                }
                Console.WriteLine($"File was trimmed {_trimStart} chars at the start and {_trimEnd} chars at the end");
            }


        }

        private void Build(string[] arguments)
        {
            var argumentPairs = PairArgumentsWithValues(arguments);

            foreach (var argumentPair in argumentPairs)
            {
                switch (argumentPair.Key)
                {
                    case PathArgument:
                        _path = argumentPair.Value;
                        break;
                    case TrimStartArgument:
                        _trimStart = ParseIntArgumentValue(argumentPair.Key, argumentPair.Value);
                        break;
                    case TrimEndArgument:
                        _trimEnd = ParseIntArgumentValue(argumentPair.Key, argumentPair.Value);
                        break;
                    default:
                        Errors.Add($"Invalid argument {argumentPair.Key}");
                        break;

                }
            }
        }


        public override void Describe()
        {
            Console.WriteLine("Trim: use -trim a specified number of chars at the start and/or  end of the file name");
            DescribeArguments();
        }

    

        private int ParseIntArgumentValue(string argument, string argumentValue)
        {
            if (!int.TryParse(argumentValue, out var intValue))
            {
                Errors.Add($" Value {argumentValue} is invalid for argument : {argument}");
                return InvalidTrimValue;
            }
           
            return intValue;
        }

        private bool ArgumentsAreValid()
        {
            if (HasErrors) return false;

            if (string.IsNullOrWhiteSpace(_path))
            {
                Errors.Add($"Path is needed, use {PathArgument} argument to add it");
                return false;
            }

            if (_trimStart == 0 && _trimEnd == 0) 
            {
                return false;
            }
          
            return true;
        }

        private string GetTrimmedFileName(string filePath)
        {
            string trimmedFileName;

            try
            {
                var initialFileName = filePath
             .Split("\\")
             .LastOrDefault()
             .Split(".")
             .FirstOrDefault();

                trimmedFileName = initialFileName.Substring(_trimStart, initialFileName.Length - _trimStart);
                trimmedFileName = trimmedFileName.Substring(0, trimmedFileName.Length - _trimEnd);
                trimmedFileName = filePath.Replace(initialFileName, trimmedFileName);
            }
            catch (Exception ex)
            {

                Errors.Add($"Error trimming file {filePath}: {ex.Message}");
                return filePath;
            }
         

            return trimmedFileName;
        }
    }
}
