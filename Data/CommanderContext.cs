using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data{
    public class CommanderContext : DbContext
    {
        /*
            Inicializamos el contexto del Commander con una DI del contexto de la base de datos.
            Especificaremos las opciones de conexión en options, para que ya al instanciar esta clase
            estas dependencias informativas vengan ya implícitas.
        */
        public CommanderContext (DbContextOptions<CommanderContext> options) : base(options)
        {

        }

        public DbSet<Command> Commands { get; set; }

    }
}