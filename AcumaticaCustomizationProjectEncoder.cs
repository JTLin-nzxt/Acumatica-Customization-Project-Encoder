using System;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace AcumaticaCustomizationProjectEncoder
{
    class AcumaticaCustomizationProjectEncoder
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the zip file path (or only path to unzip all file in the same document) :");

            string filePath = Console.ReadLine();

            if (filePath.Contains(".zip"))
            {
                string folderPath = filePath.Replace(".zip", "");
                unzipFile(filePath, folderPath);
                await encodeProjectXMLAsync(folderPath);
            }
            else
            {
                string[] zipFiles = Directory.GetFiles(filePath, "*.zip", SearchOption.AllDirectories);
                foreach(string zipFile in zipFiles) 
                {
                    Console.WriteLine(zipFile);
                    string folderPath = zipFile.Replace(".zip", "");
                    unzipFile(zipFile, folderPath);
                    await encodeProjectXMLAsync(folderPath);
                }
            }
        }

        public static void unzipFile(string filePath, string folderPath)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(filePath, folderPath);
        }

        public static async Task encodeProjectXMLAsync(string folderPath) 
        {
            var xmldoc = new XmlDocument();
            string xmlFilePath = String.Format("{0}\\project.xml", folderPath);
            xmldoc.Load(xmlFilePath);

            // trans cs class
            XmlNodeList graphNodes = xmldoc.GetElementsByTagName("Graph");
            foreach (XmlNode node in graphNodes)
            {
                Console.WriteLine("Node={0} ClassName={1}", node.Name, node.Attributes["ClassName"].Value);
                Console.WriteLine(node.InnerText);
                string fileName = String.Format("{0}.cs", node.Attributes["ClassName"].Value);
                await File.WriteAllTextAsync(String.Format("{0}\\{1}", folderPath, fileName), node.InnerText);
                node.InnerText = fileName;
            }

            xmldoc.Save(xmlFilePath);

            Console.WriteLine("Encoding completed.");
        }

    }
}