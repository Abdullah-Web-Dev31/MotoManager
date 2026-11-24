using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoManager.DAL.DTOs.Persondto
{
    /// <summary>
    /// this temlate contain a normal info about person like id and full name and phone number
    /// </summary>
    public class PersonDTO
    {

        public int PersonID { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }

        public string  PhoneNumber { get; set; }

        public PersonDTO(int personID, string firstName, string lastName, string phoneNumber)
        {
            PersonID = personID;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }
}
