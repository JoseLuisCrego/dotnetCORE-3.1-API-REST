using System.Collections.Generic;
using Commander.Data;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/commands")]
    public class CommandsController : Controller
    {
        public CommandsController(ICommanderRepository repository)
        {
            this._repository = repository;
        }

        private readonly ICommanderRepository _repository;

        //private readonly MockCommanderRepository _repository = new MockCommanderRepository();
        /*
            Si ya tenemos un repo que nos llega por DI no hace falta tener una 
            instancia siempre aquí de un MockCommanderRepository.
            En lugar de eso, tendremos una propiedad que sea de tipo ICommanderRepository (sin necesidad de instanciarla
            con una clase que la implemente ya que esto nos llegará por DI) y la instancia a la que hace referencia 
            la especificaremos en el Startup.ConfigureServices().
        */

        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands(){
            var commands = _repository.GetAllCommands();
            return Ok(commands);
        }

        //GET api/commands/{id}
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id){
            var command = _repository.GetCommandById(id);
            return Ok(command);
        }
    }
}