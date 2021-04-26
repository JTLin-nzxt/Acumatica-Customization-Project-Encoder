using System;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace AcumaticaCustomizationProjectParser
{
    class AcumaticaCustomizationProjectParser
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the folder path (where your project.xml) :");
            string folderPath = Console.ReadLine();

            var xmldoc = new XmlDocument();
            xmldoc.Load(String.Format("{0}\\project.xml", folderPath));

            // 1. trans cs class
            XmlNodeList graphNodes = xmldoc.GetElementsByTagName("Graph");
            foreach (XmlNode node in graphNodes)
            {
                Console.WriteLine("Node={0} ClassName={1}", node.Name, node.Attributes["ClassName"].Value);
                Console.WriteLine(node.InnerText);
                await File.WriteAllTextAsync(String.Format("{0}\\{1}.cs", folderPath, node.Attributes["ClassName"].Value), node.InnerText);
            }

            Console.WriteLine("Parsing completed.");
        }
    }
}