using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    /*
        Con el decorator [ApiController] conseguimos automatizar cosas que son propias de un WebAPI, como
        por ejemplo ebvitar decirle que el modelo que recibimos de la llamada proviene en el body
        con el decorator [FromBody] o gestionar automáticamente la validación de los modelos que tengan
        DataAnnotations en sus atributos devolviendo un 400 con el error en el modelo de manera automática.
    */
    [ApiController]
    [Route("api/commands")]
    public class CommandsController : Controller
    {
        public CommandsController(ICommanderRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        private readonly ICommanderRepository _repository;
        public IMapper _mapper { get; }

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
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            if(commandItems != null)
            {
                //Devuelve un OK 200.
                return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
            }
            return NotFound();
        }

        //GET api/commands/{id}
        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if(commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            return NotFound(); //Devuelve un error 404.
        }

        //POST api/commands/
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        { 
            var commandModel =_mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommmand(commandModel);
            _repository.SaveChanges();
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            
            /*
                CreatedAtRoute():
                En la cabecera de la respuesta indica cómo acceder al comando creado y, a su vez, devuelve
                el objeto generado commandReadDto. Devuelve un 201.
            */
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);    
        }

        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand (int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(commandUpdateDto, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);
            //No hará nada, pero puede que en un futuro sea necesario modificarlo, así que lo llamamos igualmente (??)

            _repository.SaveChanges();
            return NoContent();//Devuelve un OK 204.
        }

        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> pathDoc)
        {
            /*
                Con PATCH y jsonpatchdocument ser establece un contrato de información con la siguiente estructura:
                [
                    {
                        "op":"replace",
                        "path":"/howTo",
                        "value":"How to create migrations !!"
                    }
                ]
            */
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            pathDoc.ApplyTo(commandToPatch, ModelState);
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}