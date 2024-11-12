namespace Batcher
{
    public abstract class BaseCommand
    {
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
        public abstract Dictionary<string, string> AllowedArguments { get; }

        public abstract void Describe();
        public abstract void Apply(string[] arguments);

        protected Dictionary<string, string> PairArgumentsWithValues(string[] arguments)
        {
            var argumentPairs = new Dictionary<string, string>();

            if (IsOddNumber(arguments.Length))
            {
                Errors.Add("Invalid list of arguments");
                return argumentPairs;
            }

            for (int i = 0; i < arguments.Length; i++)
            {
                var isLastArgument = i == arguments.Length - 1;

                if (!isLastArgument)
                {
                    argumentPairs.Add(arguments[i], arguments[i + 1]);
                }
                i++;
            }
            return argumentPairs;
        }

        protected void DescribeArguments()
        {
            Console.WriteLine("Allowed Arguments");

            foreach (var trimArgument in AllowedArguments)
            {
                Console.WriteLine($"{trimArgument.Key} : {trimArgument.Value}");
            }
        }

        private static bool IsOddNumber(int number)
        {
            return number % 2 != 0;
        }


    }
}
