using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hospital_Costs.Classes
{
    public class Color
    {
        public int Id { get; set; }
        public string Color_Code { get; set; }
        public Color GetColor_ById(int id)
        {
            Color color = new Color();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("[getListOfColors_ById]"))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id",id);
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            color.Id = Convert.ToInt32(reader["Id"]);
                            color.Color_Code = reader["Color_Code"].ToString();
                        }
                    }
                }
            }
            return color;
        }
    }
}