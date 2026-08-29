using System;
using System.Collections.Generic;
using System.Linq;

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return items;
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        T? item = items.FirstOrDefault(predicate);

        if (item != null)
        {
            items.Remove(item);
            return true;
        }

        return false;
    }
}

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}

public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();

    private Dictionary<int, List<Prescription>> _prescriptionMap =
        new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "Ama Mensah", 25, "Female"));
        _patientRepo.Add(new Patient(2, "Kojo Asante", 32, "Male"));
        _patientRepo.Add(new Patient(3, "Esi Boateng", 41, "Female"));

        _prescriptionRepo.Add(
            new Prescription(1, 1, "Paracetamol", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(2, 1, "Amoxicillin", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(3, 2, "Ibuprofen", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(4, 2, "Vitamin C", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(5, 3, "Cetirizine", DateTime.Now)
        );
    }

    public void BuildPrescriptionMap()
    {
        foreach (Prescription prescription in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] =
                    new List<Prescription>();
            }

            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("All Patients:");

        foreach (Patient patient in _patientRepo.GetAll())
        {
            Console.WriteLine(
                $"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}"
            );
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int patientId)
    {
        if (_prescriptionMap.ContainsKey(patientId))
        {
            return _prescriptionMap[patientId];
        }

        return new List<Prescription>();
    }

    public void PrintPrescriptionsForPatient(int id)
    {
        Console.WriteLine($"\nPrescriptions for Patient ID {id}:");

        List<Prescription> prescriptions =
            GetPrescriptionsByPatientId(id);

        foreach (Prescription prescription in prescriptions)
        {
            Console.WriteLine(
                $"Prescription ID: {prescription.Id}, Medication: {prescription.MedicationName}, Date: {prescription.DateIssued.ToShortDateString()}"
            );
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        HealthSystemApp app = new HealthSystemApp();

        app.SeedData();
        app.BuildPrescriptionMap();
        app.PrintAllPatients();
        app.PrintPrescriptionsForPatient(1);
    }
}
