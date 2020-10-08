using System.Collections.Generic;
using System.Linq;
using Commander.Models;

namespace Commander.Data
{
    public class SqlCommanderRepository : ICommanderRepository
    {
        public CommanderContext _context { get; }

        public SqlCommanderRepository(CommanderContext context)
        {
            _context = context;
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var commands = _context.Commands.ToList();
            return commands;
        }

        public Command GetCommandById(int id)
        {
            var command = _context.Commands.FirstOrDefault<Command>(command => command.Id == id); 
            return command;
        }
    }
}