using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotoManager.BLL;
using MotoManager.Common;
using MotoManager.DAL.DTOs.Persondto;

namespace MotoManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {

        [HttpGet("GetPersonByID/{personID}",Name ="GetPersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonDTO>>GetPersonByID(int personID)
        {
            var person = await People.FindPerson(personID);

            return person != null ? Ok(person.PersonDTO) : NotFound($"Not Found any person with{personID}");
        }

        [HttpGet("GetPersonByPhoneNumber/{PhoneNumber}", Name = "GetPersonByPhoneNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonDTO>> GetPersonByPhoneNumber(string PhoneNumber)
        {
            var person = await People.FindPerson(PhoneNumber);

            return person != null ? Ok(person.PersonDTO) : NotFound($"Not Found any person with{PhoneNumber}");
        }

        [HttpGet("GetAllPeople",Name ="GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PersonDTO>>>GetAllPeople()
        {
            List<PersonDTO> people =  await People.GetAllPeople();

            if(people != null)
                return Ok(people);
            return NotFound("Not Emplemented or messing");
        }

        [HttpGet("SearchPeople/{searchpeople}",Name ="SearchPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> SearchPeople(string searchpeople)
        {
            if (string.IsNullOrWhiteSpace(searchpeople))
                return BadRequest("the inpit should be wrote ..");
            List<PersonDTO> people = await People.SearchPeople(searchpeople);

            if(people != null)
                return Ok(people);
            return NotFound($"Not found any data with this search input =>{searchpeople} ");
        }

        [HttpDelete("DeletePerson/{PersonID}", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>>DeletePerson(int PersonID)
        {
            if (PersonID <= 0) return BadRequest("The num should be greater than 0 ");
            
            var Person = await People.FindPerson(PersonID);
            if (Person == null)
                return NotFound($"Not Found any person with {PersonID}");
            if(await Person.DeleteThisPerson())
            {
                Person = null;
                return Ok(true);
            }
            return StatusCode(500, "Server error");

         }

        [HttpGet("IsIDExists/{PersonID}",Name ="IsIDExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>>IsIDExists(int PersonID)
        {
            if (PersonID <= 0)
                return BadRequest("input should be greater than 0");
            if (await People.IsIDExists(PersonID))
                return Ok(true);
            return NotFound($"This person id is not a valid {PersonID}");
        }

        [HttpGet("IsPhoneNumberExists/{PhoneNumber}", Name = "IsPhoneNumberExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> IsPhoneNumberExists(string PhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return BadRequest("input should  not not be empty ..");
            if (await People.IsPhoneNumberExists(PhoneNumber))
                return Ok(true);
            return NotFound($"This person id is not a valid {PhoneNumber}");
        }

        [HttpPut("UpdatePerson", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDTO>> UpdatePerson(PersonDTO personDTO)
        {
            if (personDTO == null || personDTO.PersonID <= 0)
                return BadRequest("The person body or person id should be fill");

            var people = await People.FindPerson(personDTO.PersonID);

            if (people == null)
                return NotFound($"Not found person with id {personDTO.PersonID}");

            if (string.IsNullOrWhiteSpace(personDTO.FirstName))
                return BadRequest("The first name input should be wrote");

            if (string.IsNullOrWhiteSpace(personDTO.LastName))
                return BadRequest("The last name input should be wrote");

            if (string.IsNullOrWhiteSpace(personDTO.PhoneNumber) || !clsValidatoin.ValidatePhoneNumber(personDTO.PhoneNumber))
                return BadRequest("invalid typeing Phjone number.. Please fill data in a correct way");

            if (people.PhoneNumber != personDTO.PhoneNumber)
            {
                if(await People.IsPhoneNumberExists(personDTO.PhoneNumber))
                {
                    return BadRequest("This number is already Exists");
                }

              
            }

            people.FirstName = personDTO.FirstName;
            people.LastName = personDTO.LastName;
            people.PhoneNumber = personDTO.PhoneNumber;

            if (await people.Save())
            {
                return Ok(people.PersonDTO);
            }

            return StatusCode(500, "Server Error");
        }
               

    }
}
