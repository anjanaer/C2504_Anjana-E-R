using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Week42Practice
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
            return $"\nDoctor ID: {DoctorID}, Patient Name: {PatientName}, Medication: {Medication}, Dosage: {Dosage} mg";
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
                for (int j = i + 1; j < prescriptions.Length; j++)
                {
                    if (prescriptions[j].PatientName.CompareTo(prescriptions[i].PatientName) < 0)
                    {
                        var temp = prescriptions[i];
                        prescriptions[i] = prescriptions[j];
                        prescriptions[j] = temp;
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
            double firstMinDos = double.MaxValue;
            double secondMinDos = double.MaxValue;
            double thirdMinDos = double.MaxValue;
            DoctorPrescription thirdLeastPrescription = null;
            foreach (var prescription in prescriptions)
            {
                double dosage = prescription.Dosage;
                if (dosage < firstMinDos)
                {
                    thirdMinDos = secondMinDos;
                    secondMinDos = firstMinDos;
                    firstMinDos = dosage;
                }
                else if (dosage < secondMinDos && dosage != firstMinDos)
                {
                    thirdMinDos = secondMinDos;
                    secondMinDos = dosage;
                }
                else if (dosage < thirdMinDos && dosage != secondMinDos && dosage != firstMinDos)
                {
                    thirdMinDos = dosage;
                }
            }

            foreach (var prescription in prescriptions)
            {
                if (prescription.Dosage == thirdMinDos)
                {
                    thirdLeastPrescription = prescription;
                    break;
                }
            }
            return thirdLeastPrescription;
        }
    }

    public class Program
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            DoctorPrescription[] prescriptions = new DoctorPrescription[5];
            try
            {
                DoctorPrescriptionService.Read(prescriptions);
            }
            catch (ServerException ex)
            {
                log.Error($"{ex.Message}");
            }

            DoctorPrescription maxPrescription = DoctorPrescriptionService.FindMax(prescriptions);
            log.Info($"Max Prescription: {maxPrescription}");

            DoctorPrescription thirdMinPrescription = DoctorPrescriptionService.FindThirdMin(prescriptions);
            log.Info($"Third Min Prescription: {thirdMinPrescription}");

            DoctorPrescriptionService.Sort(prescriptions);
            string sortedPrescriptions = string.Join(" ", prescriptions.Select(p => p.ToString()));
            log.Info($"Sorted Prescriptions: {sortedPrescriptions}");
        }
    }
}

OUTPUT
---------------------------------------------------------------------------------------------------------------------------------
2024-09-01 14:37:50,583 [1] INFO  Week42Practice.Program - Max Prescription:
Doctor ID: 5, Patient Name: Eve White, Medication: Metformin, Dosage: 1000 mg
2024-09-01 14:37:50,591 [1] INFO  Week42Practice.Program - Third Min Prescription:
Doctor ID: 1, Patient Name: John Doe, Medication: Amoxicillin, Dosage: 500 mg
2024-09-01 14:37:50,594 [1] INFO  Week42Practice.Program - Sorted Prescriptions:
Doctor ID: 1, Patient Name: Abhi, Medication: Fever, Dosage: 20 mg
Doctor ID: 3, Patient Name: Alice Johnson, Medication: Paracetamol, Dosage: 650 mg
Doctor ID: 4, Patient Name: Bob Brown, Medication: Atorvastatin, Dosage: 10 mg
Doctor ID: 5, Patient Name: Eve White, Medication: Metformin, Dosage: 1000 mg
Doctor ID: 1, Patient Name: John Doe, Medication: Amoxicillin, Dosage: 500 mg
Press any key to continue . . .

