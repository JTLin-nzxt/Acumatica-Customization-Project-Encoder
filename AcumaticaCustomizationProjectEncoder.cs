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

            //string filePath = Console.ReadLine();
            string filePath = Directory.GetCurrentDirectory();

            if (filePath.Contains(".zip"))
            {
                string folderPath = filePath.Replace(".zip", "");
                UnzipFile(filePath, folderPath);
                await EncodeProjectXMLAsync(folderPath);
            }
            else
            {
                string[] zipFiles = Directory.GetFiles(filePath, "*.zip", SearchOption.AllDirectories);
                foreach(string zipFile in zipFiles) 
                {
                    Console.WriteLine(zipFile);
                    string folderPath = zipFile.Replace(".zip", "");
                    UnzipFile(zipFile, folderPath);
                    await EncodeProjectXMLAsync(folderPath);
                    DeleteZip(zipFile);
                }
            }
        }

        public static void UnzipFile(string filePath, string folderPath)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(filePath, folderPath);
        }

        public static void DeleteZip(string filePath)
        {
            File.Delete(filePath);
        }

        public static async Task EncodeProjectXMLAsync(string folderPath) 
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