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
                // Get all XML files from the folder
                string[] xmlFiles = Directory.GetFiles(folderPath, "*.xml");

                if (xmlFiles.Length == 0)
                {
                    Console.WriteLine($"No XML files found in folder: {folderPath}");
                    return;
                }

                Console.WriteLine($"Found {xmlFiles.Length} XML file(s) in folder.\n");
                Console.WriteLine(new string('=', 70));

                // Process each file one by one
                foreach (string filePath in xmlFiles)
                {
                    ProcessXmlFile(filePath, searchText);
                }

                Console.WriteLine("\nProcessing complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void ProcessXmlFile(string filePath, string searchText)
        {
            string fileName = Path.GetFileName(filePath);
            Console.WriteLine($"\n>>> Processing file: {fileName}");
            Console.WriteLine(new string('-', 70));

            try
            {
                XDocument doc = XDocument.Load(filePath);

                // Search in all elements and attributes
                var matchingElements = doc.Descendants()
                    .Where(e => e.Value.Contains(searchText) || 
                               e.Attributes().Any(a => a.Value.Contains(searchText)))
                    .ToList();

                if (matchingElements.Count == 0)
                {
                    Console.WriteLine($"No matches found for '{searchText}' in this file.");
                }
                else
                {
                    Console.WriteLine($"Found {matchingElements.Count} match(es) containing '{searchText}':\n");

                    foreach (var element in matchingElements)
                    {
                        Console.WriteLine($"  Element: {element.Name}");
                        
                        // Show element value if it contains the search text
                        if (element.Value.Contains(searchText))
                        {
                            Console.WriteLine($"  Value: {element.Value.Trim()}");
                        }
                        
                        // Show attributes if any contain the search text
                        var matchingAttrs = element.Attributes()
                            .Where(a => a.Value.Contains(searchText));
                        
                        foreach (var attr in matchingAttrs)
                        {
                            Console.WriteLine($"  Attribute: {attr.Name} = {attr.Value}");
                        }
                        
                        Console.WriteLine($"  XML: {element}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file '{fileName}': {ex.Message}");
            }

            Console.WriteLine(new string('-', 70));
        }
    }
}
