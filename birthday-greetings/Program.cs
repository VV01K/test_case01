using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace birthday_greeting
{
    public class Program
    {
        public static List<(string, string, string)> SentGreetings = new List<(string, string, string)>();
        static void Main(string[] args)
        {
            SendGreetings();
            

            Console.ReadLine();
        }

        public static void SendGreetings()
        {
            var fileName = "employees.txt";

            try
            {
                var v = File.Exists(fileName);
                var lines = fileName.ReadAllLines();

                Console.WriteLine("Reading file...");
                var first_line = true;
                foreach (var line in lines)
                {
                    try
                    {
                        if (first_line)
                        {
                            first_line = false;
                        }
                        else
                        {
                            var tokens = line.Split(',');
                            for (var i = 0; i < tokens.Length; i++)
                                tokens[i] = tokens[i].Trim();

                            if (tokens.Length == 4)
                            {
                                var date = tokens[2].Split('/');
                                if (date.Length == 3)
                                {
                                    var cal = DateTime.Now;

                                    if (cal.Day == int.Parse(date[0]) && cal.Month == int.Parse(date[1]))
                                    {
                                        SendEmail(tokens[3], "Joyeux Anniversaire !", "Bonjour " + tokens[0] + ",\nJoyeux Anniversaire !\nA bientôt,");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Cannot read birthdate for " + tokens[0] + " " + tokens[1]);
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid file format");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }

                Console.WriteLine("Batch job done.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Unable to open file '" + fileName + "'");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error reading file '" + fileName + "'");
            }
        }

        public static void SendEmail(string to, string title, string body)
        {
            Console.WriteLine("Sending email to : " + to);
            Console.WriteLine("Title: " + title);
            Console.WriteLine("Body: Body\n" + body);
            Console.WriteLine("-------------------------");
            SentGreetings.Add((to,title,body));
        }
    }

    public static class FileExtension
    {
        public static string[] ReadAllLines(this string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { throw new ArgumentNullException($"Cannot read resource file beceause filename was null or empty"); }
            var assemblyTypeForLocation = typeof(Program);
            var assemblyFolder = Path.GetDirectoryName(assemblyTypeForLocation.Assembly.Location);
            var resourceFolder = Path.Combine(assemblyFolder);
            if (!Directory.Exists(resourceFolder))
            {
                resourceFolder = Path.Combine(Path.GetDirectoryName(assemblyFolder)); // MISC: Trick because local function and unit test does not work the same.
            }

            var filePath = Path.Combine(resourceFolder, fileName);
            if (!File.Exists(filePath)) { return new string[0]; }

            using var sr = new StreamReader(filePath, Encoding.UTF8);

            return sr.ReadToEnd().Split(Environment.NewLine);
        }
    }
}
