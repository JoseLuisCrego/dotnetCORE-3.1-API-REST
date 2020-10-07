using System.Collections.Generic;
using Commander.Data;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/commands")]
    public class CommandsController : Controller
    {
        private readonly MockCommanderRepository _repo = new MockCommanderRepository();

        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands(){
            var commands = _repo.GetAppCommands();
            return Ok(commands);
        }

        //GET api/commands/{id}
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id){
            var command = _repo.GetCommandById(id);
            return Ok(command);
        }
    }
}