using Microsoft.Data.SqlClient;
using MotoManager.DAL.DTOs.Persondto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoManager.DAL
{
    public static class PeopleData
    {

        public static async Task <bool> IsIDExists(int personID)
        {
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("Person_SP_IsIDExists", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@PersonID", personID);
                       await con.OpenAsync();
                        SqlParameter outputParam = new SqlParameter("@IsPersonID", System.Data.SqlDbType.Bit)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        comm.Parameters.Add(outputParam);
                        var result = await comm.ExecuteScalarAsync();
                        return outputParam.Value != DBNull.Value && (bool)outputParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
              
                throw new Exception("An error occurred while checking if the ID exists.", ex);
            }
        }

        public static async Task<bool> IsPhoneNumberExists(string phoneNumber)
        {
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("[Person_SP_IsPhoneNumberExists]", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                       await con.OpenAsync();
                        SqlParameter outputParam = new SqlParameter("@IsPhoneNumberExists", System.Data.SqlDbType.Bit)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        comm.Parameters.Add(outputParam);

                        var result = await comm.ExecuteScalarAsync();
                        return outputParam.Value != DBNull.Value && (bool)outputParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
            
                throw new Exception("An error occurred while checking if the phone number exists.", ex);
            }
        }

        public static async Task <bool>UpdatePerson(PersonDTO person)
        {
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("[Person_SP_UpdatePerson]", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@PersonID", person.PersonID);
                        comm.Parameters.AddWithValue("@FirstName", person.FirstName);
                        comm.Parameters.AddWithValue("@LastName", person.LastName);
                        comm.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber);
                       await con.OpenAsync();
                        int rowsAffected = await comm.ExecuteNonQueryAsync();
                        return rowsAffected > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while updating the person.", ex);
            }
        }

        public static async Task<PersonDTO>  GetPersonByID(int prsonID)
        {
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("[Person_SP_GetPersonByID]", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@PersonID", prsonID);
                       await con.OpenAsync();

                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new PersonDTO(
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("FirstName")),
                                    reader.GetString(reader.GetOrdinal("LastName")),
                                    reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                );
                            }

                        }
                    }
                }
            } 
                catch (Exception ex)
            {
                
                throw new Exception("An error occurred while retrieving the person.", ex);
            }

            return null;

        }

        public static async Task<List<PersonDTO>> GetAllPeople()
        {
            List<PersonDTO> persons = new List<PersonDTO>();
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("[Person_SP_GetAllPeople]", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        await con.OpenAsync();
                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                persons.Add(new PersonDTO(
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("FirstName")),
                                    reader.GetString(reader.GetOrdinal("LastName")),
                                    reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                throw new Exception("An error occurred while retrieving the persons.", ex);
            }
            return persons;
        }

        public static async Task<bool>DeletePerson(int PersonID)
        {
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("Person_SP_DeletePerson", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@PersonID", PersonID);
                        await con.OpenAsync();
                        int rowsAffected = await comm.ExecuteNonQueryAsync();
                        return rowsAffected > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while deleting the person.", ex);
            }
        }

        public static async Task<List<PersonDTO>> SearchPeople(string searchTerm)
        {
            List<PersonDTO> persons = new List<PersonDTO>();
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("[Person_SP_SearchPeople]", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@SearchTerm", searchTerm);
                        await con.OpenAsync();
                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                persons.Add(new PersonDTO(
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("FirstName")),
                                    reader.GetString(reader.GetOrdinal("LastName")),
                                    reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while searching for persons.", ex);
            }
            return persons;
        }

        public static async Task<PersonDTO> GetPersonByPhoneNumber(string PhoneNumber)
        {
            try
            {
                using (SqlConnection con = ConnectionSettings._GetConnection())
                {
                    using (SqlCommand comm = new SqlCommand("[Person_SP_GetPersonByNumber]", con))
                    {
                        comm.CommandType = System.Data.CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                      await  con.OpenAsync();

                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new PersonDTO(
                                    reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    reader.GetString(reader.GetOrdinal("FirstName")),
                                    reader.GetString(reader.GetOrdinal("LastName")),
                                    reader.GetString(reader.GetOrdinal("PhoneNumber"))
                                );
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while retrieving the person.", ex);
            }

            return null;
        }
    }

}
