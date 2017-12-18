using Hospital_Costs.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Hospital_Costs.Classes
{
    public class Charge : Chargeable
    {
        public int Id { get; set; }
        public int Hospital_Id { get; set; }
        public string Total_Cost { get; set; }
        public double Total_Medicare_Payments { get; set; }
        public double Total_Payments { get; set; }
        public Diagnosable Current_Diagnosis { get; set; }
        public IHospital Current_Hospital { get; set; }

        public IEnumerable<Chargeable> Read()
        {
            return GetAll();
        }
        public IEnumerable<Chargeable> Read(string state, int totalResults)
        {
            return GetTopResults_ByState(state, totalResults);
        }
        public IEnumerable<Chargeable> ReadLowestResults(string state, int totalResults)
        {
            return Results_ByState(state, totalResults);
        }
        // Method that gets the lowest values for the grid
        private IList<Charge> Results_ByState(string state, int totalResults)
        {
            Charge charge = new Charge();
            return charge.GetCharge_LowestResults_ByState(state, totalResults).Select(current_charge => new Charge
            {
                Id = current_charge.Id,
                Current_Diagnosis = GetDiagnosis_ByCode(current_charge.Current_Diagnosis.Code),
                Current_Hospital = GetHospital_ByHospitalId(current_charge.Current_Hospital.Hospital_Id),
                Total_Cost = formatMoney(current_charge.Total_Cost),
                Total_Medicare_Payments = current_charge.Total_Medicare_Payments,
                Total_Payments = current_charge.Total_Payments
            }).ToList();
        }

        // Method that gets the top values for the grid
        private IList<Charge> GetTopResults_ByState(string state, int totalResults)
        {
            Charge charge = new Charge();
            return charge.GetCharge_TopResults_ByState(state, totalResults).Select(current_charge => new Charge
            {
                Id = current_charge.Id,
                Current_Diagnosis = GetDiagnosis_ByCode(current_charge.Current_Diagnosis.Code),
                Current_Hospital = GetHospital_ByHospitalId(current_charge.Current_Hospital.Hospital_Id),
                Total_Cost = formatMoney(current_charge.Total_Cost),
                Total_Medicare_Payments = current_charge.Total_Medicare_Payments,
                Total_Payments = current_charge.Total_Payments
            }).ToList();
        }

        private string formatMoney(string value)
        {
            StringBuilder sb = new StringBuilder();
            int indexOfDecimal = value.IndexOf(".") + 2;
            sb.Append(value.Substring(0, ++indexOfDecimal));
            return sb.ToString();
        }
        // Method that gets all the values for the grid
        private IList<Charge> GetAll()
        {
            Charge charge = new Charge();
            return charge.GetCharge().Select(current_charge => new Charge
            {
                Id = current_charge.Id,
                Current_Diagnosis = GetDiagnosis_ByCode(current_charge.Current_Diagnosis.Code),
                Current_Hospital = GetHospital_ByHospitalId(current_charge.Current_Hospital.Hospital_Id),
                Total_Cost = current_charge.Total_Cost,
                Total_Medicare_Payments = current_charge.Total_Medicare_Payments,
                Total_Payments = current_charge.Total_Payments
            }).ToList();
        }
        // Get Hospital By Hospital_Id
        private IHospital GetHospital_ByHospitalId(int id)
        {
            IHospital hospital = new Hospital();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("getHospitals_ByHospitalId"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@HospitalId", id);
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            hospital.Id = Convert.ToInt32(reader["Id"]);
                            hospital.Hospital_Id = Convert.ToInt32(reader["Hospital_Id"]);
                            hospital.Name = reader["Name"].ToString();
                            hospital.Address = reader["Address"].ToString();
                            hospital.City = reader["City"].ToString();
                            hospital.State = reader["State"].ToString();
                            hospital.Zip = Convert.ToInt32(reader["Zip"]);
                            hospital.RegionDescription = reader["RegionDescription"].ToString();
                        }
                    }
                }
            }
            return hospital;
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
        //
        private List<Chargeable> GetCharge_LowestResults_ByState(string state, int totalResults)
        {
            List<Chargeable> list = new List<Chargeable>();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("getCharges_LowestResults_ByState"))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@state", state.Trim());
                    command.Parameters.AddWithValue("@totalResults", totalResults);
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chargeable charge = new Charge();
                            Diagnosable diagnosis = new Diagnosis();
                            IHospital hospital = new Hospital();
                            charge.Id = Convert.ToInt32(reader["Id"]);
                            hospital.Hospital_Id = Convert.ToInt32(reader["Hospital_Id"]);
                            diagnosis.Code = Convert.ToInt32(reader["Diagnosis_Id"]);
                            charge.Total_Cost = reader["Total_Cost"].ToString();
                            charge.Total_Payments = Convert.ToDouble(reader["Total_Payments"]);
                            charge.Total_Medicare_Payments = Convert.ToDouble(reader["Total_Medicare_Payments"]);
                            charge.Current_Diagnosis = diagnosis;
                            charge.Current_Hospital = hospital;
                            list.Add(charge);
                        }
                    }
                }
            }
            return list;
        }


        private List<Chargeable> GetCharge_TopResults_ByState(string state, int totalResults)
        {
            List<Chargeable> list = new List<Chargeable>();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("getCharges_TopResults_ByState"))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@state", state.Trim());
                    command.Parameters.AddWithValue("@totalResults", totalResults);
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chargeable charge = new Charge();
                            Diagnosable diagnosis = new Diagnosis();
                            IHospital hospital = new Hospital();
                            charge.Id = Convert.ToInt32(reader["Id"]);
                            hospital.Hospital_Id = Convert.ToInt32(reader["Hospital_Id"]);
                            diagnosis.Code = Convert.ToInt32(reader["Diagnosis_Id"]);
                            charge.Total_Cost = reader["Total_Cost"].ToString();
                            charge.Total_Payments = Convert.ToDouble(reader["Total_Payments"]);
                            charge.Total_Medicare_Payments = Convert.ToDouble(reader["Total_Medicare_Payments"]);
                            charge.Current_Diagnosis = diagnosis;
                            charge.Current_Hospital = hospital;
                            list.Add(charge);
                        }
                    }
                }
            }
            return list;
}

        // Method that gets all the charges
        private List<Chargeable> GetCharge()
        {
            List<Chargeable> list = new List<Chargeable>();
            using (var conn = new SqlConnection(new SqlConnect().GetSqlConnection_String()))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("getCharges_Full"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chargeable charge = new Charge();
                            Diagnosable diagnosis = new Diagnosis();
                            IHospital hospital = new Hospital();
                            charge.Id = Convert.ToInt32(reader["Id"]);
                            hospital.Hospital_Id = Convert.ToInt32(reader["Hospital_Id"]);
                            diagnosis.Code = Convert.ToInt32(reader["Diagnosis_Id"]);
                            charge.Total_Cost = reader["Total_Cost"].ToString();
                            charge.Total_Payments = Convert.ToDouble(reader["Total_Payments"]);
                            charge.Total_Medicare_Payments = Convert.ToDouble(reader["Total_Medicare_Payments"]);
                            charge.Current_Diagnosis = diagnosis;
                            charge.Current_Hospital = hospital;
                            list.Add(charge);
                        }
                    }
                }
            }
            return list;
        }
    }
}