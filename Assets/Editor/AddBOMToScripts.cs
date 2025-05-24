using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

public class AddBOMToScripts
{
    [MenuItem("Tools/Add BOM to Scripts")]
    public static void AddBOM()
    {
        string directoryPath = "Assets/Game/Scripts";
        string[] files = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

        byte[] bom = new byte[] { 0xEF, 0xBB, 0xBF }; // BOM для UTF-8

        foreach (string file in files)
        {
            // Чтение содержимого файла в кодировке Windows-1251
            string fileContent;
            using (var reader = new StreamReader(file, Encoding.GetEncoding(1251)))
            {
                fileContent = reader.ReadToEnd();
            }

            // Проверка, содержит ли файл BOM
            byte[] existingFileContent = File.ReadAllBytes(file);
            if (existingFileContent.Length < 3 || existingFileContent[0] != bom[0] || existingFileContent[1] != bom[1] || existingFileContent[2] != bom[2])
            {
                // Запись файла с добавленным BOM в кодировке UTF-8
                using (var fs = new FileStream(file, FileMode.Create))
                {
                    fs.Write(bom, 0, bom.Length);
                    byte[] utf8Content = Encoding.UTF8.GetBytes(fileContent);
                    fs.Write(utf8Content, 0, utf8Content.Length);
                }
                Debug.Log($"Added BOM to: {file}");
            }
        }
    }
}