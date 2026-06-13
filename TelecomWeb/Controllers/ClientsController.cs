using Microsoft.AspNetCore.Mvc;
using Telecom.Data;
using Telecom.Models;

namespace TelecomWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientDatabase _database;

        public ClientsController(IClientDatabase database)
        {
            _database = database;
        }

        [HttpGet("all")]
        public IActionResult GetAllClients()
        {
            var clients = _database.GetAllClients();
            var response = clients.Select(c => CreateResource(c));
            return Ok(response);
        }

        [HttpGet("{phone}", Name = "GetClient")]
        public IActionResult GetByPhone(long phone)
        {
            var clients = _database.GetAllClients();
            var client = clients.FirstOrDefault(c => c.PhoneNumber == phone);
            
            if (client == null) return NotFound("Client not found");

            return Ok(CreateResource(client));
        }

        [HttpPost("add")]
        public IActionResult AddClient([FromBody] Client client)
        {
            _database.AddClient(client);
            var resource = CreateResource(client);
            return CreatedAtRoute("GetClient", new { phone = client.PhoneNumber }, resource);
        }

        [HttpDelete("{phone}", Name = "RemoveClient")]
        public IActionResult RemoveClient(long phone)
        {
            _database.RemoveClient(phone);
            return NoContent();
        }

        [HttpPatch("{phone}/deposit")]
        public IActionResult Deposit(long phone, [FromQuery] double amount)
        {
            var clients = _database.GetAllClients();
            var client = clients.FirstOrDefault(c => c.PhoneNumber == phone);

            if (client == null) return NotFound("Client not found");

            client.Deposit(amount);
            return Ok(CreateResource(client));
        }

        private object CreateResource(Client client)
        {
            var links = new List<Link>
            {
                new Link("self", Url.Link("GetClient", new { phone = client.PhoneNumber }) ?? "", "GET"),
                new Link("delete", Url.Link("RemoveClient", new { phone = client.PhoneNumber }) ?? "", "DELETE"),
                new Link("deposit", $"/api/Clients/{client.PhoneNumber}/deposit", "PATCH")
            };

            return new { data = client, links };
        }
    }
}