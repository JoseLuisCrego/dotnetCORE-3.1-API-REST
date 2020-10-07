using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data{
    public interface ICommanderRepository
    {
        IEnumerable<Command> GetAppCommands();
        Command GetCommandById(int id);
    }
}