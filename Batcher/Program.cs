using Batcher;

const string Help = "-h";

const string Rename = "-rename";
const string Trim = "-trim";

var arguments = args;
var renameCommand = new RenameCommand();
var trimCommand = new TrimCommand();

var commands = new List<BaseCommand>()
{
   renameCommand,
   trimCommand
};




if (arguments.Length == 0)
{
    return;
}

if (args.Contains(Help))
{
    DisplayAvailableCommands();
}
else
{
    

    switch (arguments[0])
    {
        case Rename:
            Execute(renameCommand);
            break;
        case Trim:
            Execute(trimCommand);
            break;
        default:
            Console.WriteLine("Invalid command");
            break;
    }
}

void Execute(BaseCommand command)
{
    var commandArguments = GetCommandArguments();
    renameCommand.Apply(commandArguments);
    if (renameCommand.HasErrors)
    {
        DisplayErrors(command);
    }
}

void DisplayAvailableCommands()
{
    Console.WriteLine("Batcher - command line interface for handling file groups");
    Console.WriteLine("Batcher supports following commands: ");
    Console.WriteLine("-------------------------------------");

    foreach (var command in commands)
    {
        command.Describe();
    }
}

void DisplayErrors(BaseCommand command)
{
    foreach(var error in command.Errors)
    {
        Console.WriteLine(error);
    }

}

string[] GetCommandArguments()
{
    return arguments.Skip(1).ToArray();
}