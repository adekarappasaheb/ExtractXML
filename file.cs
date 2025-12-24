using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace XmlReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\YourFolder"; // Update with your folder path
            string searchText = "abc@johndeere.com";

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
                    ProcessXmlFile(filePath, searchText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        static void ProcessXmlFile(string filePath, string searchText)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);

                var matchingElements = doc.Descendants()
                    .Where(e => e.Value.Contains(searchText) || 
                               e.Attributes().Any(a => a.Value.Contains(searchText)));

                foreach (var element in matchingElements)
                {
                    // Print element value if it contains the search text
                    if (!string.IsNullOrWhiteSpace(element.Value) && element.Value.Contains(searchText))
                    {
                        Console.WriteLine(element.Value.Trim());
                    }
                    
                    // Print attribute values if they contain the search text
                    var matchingAttrs = element.Attributes()
                        .Where(a => a.Value.Contains(searchText));
                    
                    foreach (var attr in matchingAttrs)
                    {
                        Console.WriteLine(attr.Value);
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
