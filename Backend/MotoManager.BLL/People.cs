using MotoManager.DAL;
using MotoManager.DAL.DTOs.Persondto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoManager.BLL
{
    public class People
    {
        public int PersonID { get; set; }

        enum enMode { Update}

        enMode _Mode  = enMode.Update;
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        /// <summary>
        /// this dto contan only id and fullname and phone number
        /// </summary>
        public PersonDTO PersonDTO { get { return new PersonDTO(this.PersonID, this.FirstName, this.LastName, this.PhoneNumber); } }

        private People (PersonDTO personDTO)
        {
            PersonID = personDTO.PersonID;
            FirstName = personDTO.FirstName;
            LastName = personDTO.LastName;
            PhoneNumber = personDTO.PhoneNumber;
            _Mode = enMode.Update;
        }


         public static async  Task< People> FindPerson(int personID)
        {
            PersonDTO pdto = await PeopleData.GetPersonByID(personID);

            if (pdto != null)
            {
                return new People (pdto);
            }
            return null;
        }

        public static async Task<People>FindPerson(string PhoneNumber)
        {
            PersonDTO pdto = await PeopleData.GetPersonByPhoneNumber(PhoneNumber);

            if (pdto != null)
                return new People (pdto);
            return null;
        }


        public static async Task<List<PersonDTO>> GetAllPeople()=>   await PeopleData.GetAllPeople();

        public static async Task<bool> IsIDExists(int  personID) => await PeopleData.IsIDExists(personID);
        public static async Task<bool> IsPhoneNumberExists(string  PhoneNumber) => await PeopleData.IsPhoneNumberExists(PhoneNumber);

        private async Task<bool> _UpdatePerson()
        {
            return await PeopleData.UpdatePerson(this.PersonDTO);
        }

        public static async Task<bool>  DeletePerson(int personID)
        {
            return await PeopleData.DeletePerson(personID);
        }

        public async Task<bool> DeleteThisPerson()
        {
            bool deletedSuccessfully = await People.DeletePerson(this.PersonID);
            

            return deletedSuccessfully;

        }

        public static async Task<List<PersonDTO>> SearchPeople(string searchpeople)
        {
            return await PeopleData.SearchPeople(searchpeople);
        }

        public async Task<bool>Save()
        {
            return await _UpdatePerson();
        }
    }

}
