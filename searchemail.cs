using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace XmlReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\YourFolder"; // Update with your folder path
            string searchPattern = @"\S+@(johndeere\.com|gmail\.com)"; // Regex pattern for *@johndeere.com and *@gmail.com
            string outputFile = Path.Combine(folderPath, "search_results.txt");

            try
            {
                string[] xmlFiles = Directory.GetFiles(folderPath, "*.xml");

                if (xmlFiles.Length == 0)
                {
                    Console.WriteLine("No XML files found.");
                    return;
                }

                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    foreach (string filePath in xmlFiles)
                    {
                        ProcessXmlFile(filePath, searchPattern, writer);
                    }
                }

                Console.WriteLine($"Results saved to: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        static void ProcessXmlFile(string filePath, string searchPattern, StreamWriter writer)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);
                Regex regex = new Regex(searchPattern, RegexOptions.IgnoreCase);
                List<string> foundValues = new List<string>();

                var allElements = doc.Descendants();

                foreach (var element in allElements)
                {
                    // Check element value
                    if (!string.IsNullOrWhiteSpace(element.Value))
                    {
                        var matches = regex.Matches(element.Value);
                        foreach (Match match in matches)
                        {
                            foundValues.Add(match.Value);
                        }
                    }

                    // Check attributes
                    foreach (var attr in element.Attributes())
                    {
                        var matches = regex.Matches(attr.Value);
                        foreach (Match match in matches)
                        {
                            foundValues.Add(match.Value);
                        }
                    }
                }

                // Write results if any values found
                if (foundValues.Count > 0)
                {
                    string fileName = Path.GetFileName(filePath);
                    writer.WriteLine($"{fileName}:");
                    
                    HashSet<string> uniqueValues = new HashSet<string>(foundValues);
                    foreach (var value in uniqueValues)
                    {
                        writer.WriteLine($"  {value}");
                    }
                    
                    writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine($"Error processing file: {ex.Message}");
            }
        }
    }
}
