using EFCoreDatabaseFirstSample.Models;
using EFCoreDatabaseFirstSample.Models.DTO;
using EFCoreDatabaseFirstSample.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDatabaseFirstSample.Controllers
{
    [Route("api/publishers")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IDataRepository<Publisher, PublisherDto> _dataRepository;

        public PublishersController(IDataRepository<Publisher, PublisherDto> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var publisher = _dataRepository.Get(id);
            if (publisher == null)
            {
                return NotFound("The Publisher record couldn't be found.");
            }

            _dataRepository.Delete(publisher);
            return NoContent();
        }
    }
}