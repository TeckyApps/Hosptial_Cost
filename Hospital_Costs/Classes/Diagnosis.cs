using Hospital_Costs.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hospital_Costs.Classes
{
    public class Diagnosis : Diagnosable
    {
        public int Code { get; set; }
        public string DRG_Definition { get; set; }
        public int Id { get; set; }
        public int Total { get; set; }
        public Color current_color { get; set; }
        public IEnumerable<Diagnosis> ReadDiagnosisCount(string state)
        {
            return Diagnosis_ToList(state);
        }
        // Method that gets the lowest values for the grid
        private IList<Diagnosis> Diagnosis_ToList(string state)
        {
            Color color = new Color();
            Diagnosis diagnosis = new Diagnosis();
            return diagnosis.GetDiagnosisCount(state).Select(current_diagnosis => new Diagnosis
            {
                Id = current_diagnosis.Id,
                DRG_Definition = GetDiagnosis_ByCode(current_diagnosis.Code).DRG_Definition,
                Code = current_diagnosis.Code,
                Total = current_diagnosis.Total,
                current_color = color.GetColor_ById(current_diagnosis.Id)
            }).ToList();
        }
        // Get Diagnosis by Code
        private Diagnosable GetDiagnosis_ByCode(int code)
        {
            Diagnosable diagnosis = new Diagnosis();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("[getDiagnosis_ByCode]"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@code", code);
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            diagnosis.Id = Convert.ToInt32(reader["Id"]);
                            diagnosis.Code = Convert.ToInt32(reader["Code"]);
                            diagnosis.DRG_Definition = reader["DRG_Definition"].ToString();
                        }
                    }
                }
            }
            return diagnosis;
        }
        // Method that gets the diagnosis count
        private List<Diagnosis> GetDiagnosisCount(string state)
        {
            List<Diagnosis> list = new List<Diagnosis>();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("getDiagnosisCount_ByState"))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@state", state.Trim());
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Diagnosis diagnosis = new Diagnosis();
                            diagnosis.Code = Convert.ToInt32(reader["Diagnosis_Id"]);
                            diagnosis.Total = Convert.ToInt32(reader["Count"]);
                            list.Add(diagnosis);
                        }
                    }
                }
            }
            return list;
        }
    }
}