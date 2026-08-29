using System;
using System.Collections.Generic;
using System.IO;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Score { get; set; }

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80 && Score <= 100)
            return "A";
        else if (Score >= 70)
            return "B";
        else if (Score >= 60)
            return "C";
        else if (Score >= 50)
            return "D";
        else
            return "F";
    }
}

public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message)
    {
    }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message)
    {
    }
}

public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        List<Student> students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                if (parts.Length != 3)
                {
                    throw new MissingFieldException(
                        $"Missing field in record: {line}"
                    );
                }

                if (!int.TryParse(parts[0].Trim(), out int id))
                {
                    throw new MissingFieldException(
                        $"Invalid or missing student ID in record: {line}"
                    );
                }

                string fullName = parts[1].Trim();

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    throw new MissingFieldException(
                        $"Student name is missing in record: {line}"
                    );
                }

                if (!int.TryParse(parts[2].Trim(), out int score))
                {
                    throw new InvalidScoreFormatException(
                        $"Invalid score format in record: {line}"
                    );
                }

                students.Add(new Student(id, fullName, score));
            }
        }

        return students;
    }

    public void WriteReportToFile(List<Student> students, string outputFilePath)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (Student student in students)
            {
                writer.WriteLine(
                    $"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}"
                );
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = "students.txt";
        string outputFilePath = "report.txt";

        try
        {
            StudentResultProcessor processor = new StudentResultProcessor();

            List<Student> students =
                processor.ReadStudentsFromFile(inputFilePath);

            processor.WriteReportToFile(students, outputFilePath);

            Console.WriteLine("Student report created successfully.");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File Error: {ex.Message}");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine($"Score Error: {ex.Message}");
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine($"Missing Field Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
        }
    }
}
