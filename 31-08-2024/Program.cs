using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.SqlClient;

using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{


    public class ServerException : Exception
    {
        public ServerException(string message) : base(message) { }
    }

    public class DoctorPrescription
    {
        public int DoctorID { get; set; }
        public string PatientName { get; set; }
        public string Medication { get; set; }
        public double Dosage { get; set; }

        public override string ToString()
        {
            return $"Doctor ID: {DoctorID}, Patient Name: {PatientName}, Medication: {Medication}, Dosage: {Dosage} mg";
        }
    }

    public class DoctorPrescriptionService
    {
        private static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DoctorPrescriptionsDb;Integrated Security=True;";
        public static void Read(DoctorPrescription[] prescriptions)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DoctorID, PatientName, Medication, Dosage FROM DoctorPrescriptions";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    for (int i = 0; i < prescriptions.Length; i++)
                    {
                        if (!reader.Read())
                        {
                            throw new ServerException("[0101]Server Error.");
                        }
                        prescriptions[i] = new DoctorPrescription
                        {
                            DoctorID = (int)reader["DoctorID"],
                            PatientName = reader["PatientName"].ToString(),
                            Medication = reader["Medication"].ToString(),
                            Dosage = (double)reader["Dosage"]
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ServerException($"[0102]Server Error. {ex.Message}");
            }
            catch (ServerException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServerException($"[0103]Server Error. {ex.Message}");
            }
        }

        public static void Sort(DoctorPrescription[] prescriptions)
        {
            for (int i = 0; i < prescriptions.Length - 1; i++)
            {
                for (int j = 0; j < prescriptions.Length - i - 1; j++)
                {
                    if (prescriptions[j].PatientName.CompareTo(prescriptions[j + 1].PatientName) > 0)
                    {
                        var temp = prescriptions[j];
                        prescriptions[j] = prescriptions[j + 1];
                        prescriptions[j + 1] = temp;
                    }
                }
            }
        }

        public static DoctorPrescription FindMax(DoctorPrescription[] prescriptions)
        {
            double maxDosage = double.MinValue;
            DoctorPrescription maxPrescription = null;

            foreach (var prescription in prescriptions)
            {
                if (prescription.Dosage > maxDosage)
                {
                    maxDosage = prescription.Dosage;
                    maxPrescription = prescription;
                }
            }

            return maxPrescription;
        }

        public static DoctorPrescription FindThirdMin(DoctorPrescription[] prescriptions)
        {
            List<double> uniqueDosages = prescriptions.Select(p => p.Dosage).Distinct().OrderBy(d => d).ToList();
            if (uniqueDosages.Count < 3) return null;

            double thirdMinDosage = uniqueDosages[2];
            return prescriptions.FirstOrDefault(p => p.Dosage == thirdMinDosage);
        }
    }

    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            DoctorPrescription[] prescriptions = new DoctorPrescription[3];
            try
            {
                DoctorPrescriptionService.Read(prescriptions);
            }
            catch (ServerException ex)
            {
                log.Error(ex.Message);
            }

            DoctorPrescription max = DoctorPrescriptionService.FindMax(prescriptions);
            log.Info($"Max Dosage: {max}");

            DoctorPrescription thirdMin = DoctorPrescriptionService.FindThirdMin(prescriptions);
            log.Info($"Third Minimum Dosage: {thirdMin}");

            DoctorPrescriptionService.Sort(prescriptions);
            string output = string.Join(" ", prescriptions.Select(p => p.ToString()));
            log.Info($"Sorted Prescriptions by Patient Name: {output}");
        }
    }
}
