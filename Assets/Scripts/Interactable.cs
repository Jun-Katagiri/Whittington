using System.Collections.Generic;
public interface Interactable
{
    public List<Command> GetCommands();
    public Command GetPrimaryCommand();

    public void ExecuteCommand(Command command);

}