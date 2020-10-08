using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data{
    public interface ICommanderRepository
    {
        IEnumerable<Command> GetAllCommands();
        Command GetCommandById(int id);
    }
}