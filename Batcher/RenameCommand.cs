namespace Batcher
{
    public class RenameCommand : BaseCommand
    {
        private string _path;
        private string _renameFrom;
        private string _renameTo;

        private const string PathArgument = "-p";
        private const string FromArgument = "-f";
        private const string ToArgument = "-t";

        public override Dictionary<string, string> AllowedArguments { get; } = new Dictionary<string, string>()
        {
            { PathArgument, "Full path of the folder to be used" },
            { FromArgument, "Text to rename from" },
            { ToArgument, "Text to rename to" }

        };


        public override void Apply(string[] arguments)
        {

            Build(arguments);
            if(!ArgumentsAreValid()) return;

            var files = Directory.GetFiles(_path);


            foreach (var file in files) 
            {
                if(!File.Exists(file)) continue;

                var currentFileName = file;
                var newFileName = currentFileName.Replace(_renameFrom, _renameTo, StringComparison.OrdinalIgnoreCase);

                if(string.Equals(currentFileName, newFileName, StringComparison.Ordinal)) continue;

                try
                {
                    File.Move(currentFileName, newFileName);
                } 
                catch(Exception ex) 
                {
                    Errors.Add($"Error renaming [{currentFileName}] to [{newFileName}]: {ex.Message}");
                    continue;
                }
                Console.WriteLine($"Renamed file [{currentFileName}] to [{newFileName}]");
            }
        }

        public override void Describe()
        {
            Console.WriteLine("Rename: use -rename to rename all files inside a specified folder");
           DescribeArguments();
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
                    case FromArgument:
                        _renameFrom = argumentPair.Value;
                        break;
                    case ToArgument:
                        _renameTo = argumentPair.Value;
                        break;
                    default:
                        Errors.Add($"Invalid argument {argumentPair.Key}");
                        break;
                
                }
            }
        }


        private bool ArgumentsAreValid()
        {
            if(HasErrors) return false;

            if (string.IsNullOrWhiteSpace(_path)) 
            {
                Errors.Add($"Path is needed, use {PathArgument} argument to add it");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_renameFrom)) 
            {
                Errors.Add($"Text to rename is needed, use {FromArgument} argument to add it");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_renameTo)) 
            {
                Errors.Add($"Text to rename is needed, use {ToArgument} argument to add it");
                return false;
            }

            return true;
        }
    }
}
