using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data{
    public interface ICommanderRepository
    {
        bool SaveChanges();

        IEnumerable<Command> GetAllCommands();
        Command GetCommandById(int id);
        void CreateCommmand(Command command);
    }
}