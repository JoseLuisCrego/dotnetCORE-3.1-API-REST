using System;
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

        public bool SaveChanges()
        {
            /*
                Cuando llamamos a métodos que hacen insert, delete, update en BD, este método es el que dirá si ha ido bien o no 
                el proceso. Es parecido al ExecuteNonQuery() de toda la vida, pero para decirle a EF que proceda con los cambios en BD.
            */
            return (_context.SaveChanges() >= 0);
        }

        public void CreateCommmand(Command command)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            _context.Add(command);
        }

        public void UpdateCommand(Command command)
        {
            /*
                No es necesario implementar nada de base ya que en el momento en que obtenemos en la lógica del aplicativo 
                el objeto que queremos actualizar, será suficiente con modificarlo y aplicar SaveChanges().
            */
        }
    }
}