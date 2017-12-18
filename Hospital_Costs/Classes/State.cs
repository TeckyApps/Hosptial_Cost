using Hospital_Costs.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hospital_Costs.Classes
{
    public class State : IState
    {
        public int Id { get; set; }
        public string State_Abbreviation { get; set; }
        public string State_Name { get; set; }
        public IEnumerable<IState> Read()
        {
            return GetAll();
        }
        private List<IState> GetAll()
        {
            List<IState> list = new List<IState>();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("getStates_Full"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            State state = new State();
                            state.Id = Convert.ToInt32(reader["Id"]);
                            state.State_Name = reader["State_Name"].ToString();
                            state.State_Abbreviation = reader["State_Abbreviation"].ToString();
                            list.Add(state);
                        }
                    }
                }
                return list;
            }
        }
    }
}