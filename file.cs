using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;

namespace XmlReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\YourFolder"; // Update with your folder path
            string searchPattern = @"\S+@johndeere\.com"; // Regex pattern for *@johndeere.com

            try
            {
                string[] xmlFiles = Directory.GetFiles(folderPath, "*.xml");

                if (xmlFiles.Length == 0)
                {
                    Console.WriteLine("No XML files found.");
                    return;
                }

                foreach (string filePath in xmlFiles)
                {
                    ProcessXmlFile(filePath, searchPattern);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        static void ProcessXmlFile(string filePath, string searchPattern)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);
                Regex regex = new Regex(searchPattern, RegexOptions.IgnoreCase);

                var allElements = doc.Descendants();

                foreach (var element in allElements)
                {
                    // Check element value
                    if (!string.IsNullOrWhiteSpace(element.Value))
                    {
                        var matches = regex.Matches(element.Value);
                        foreach (Match match in matches)
                        {
                            Console.WriteLine(match.Value);
                        }
                    }

                    // Check attributes
                    foreach (var attr in element.Attributes())
                    {
                        var matches = regex.Matches(attr.Value);
                        foreach (Match match in matches)
                        {
                            Console.WriteLine(match.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
