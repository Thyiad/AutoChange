using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoChange
{
    class Program
    {
        private static string old = "";
        private static string now = "";

        static void Main(string[] args)
        {
            Console.WriteLine("请输入你要替换的原文字......");
            old = Console.ReadLine();
            if (string.IsNullOrEmpty(old))
            {
                Console.WriteLine("将默认替换Thyiad......");
                old = "Thyiad";
            }

            while (string.IsNullOrEmpty(now))
            {
                Console.WriteLine("请输入你要替换的目标文字......");
                now = Console.ReadLine();
            }

            Console.WriteLine($"-------------------------------开始修改{Environment.CurrentDirectory}--------------------------------");
            ChangeDir(Environment.CurrentDirectory);
            Console.WriteLine($"-------------------------------结束修改{Environment.CurrentDirectory}--------------------------------");
        }

        private static void ChangeDir(string dirPath)
        {
            // 子文件
            var files = Directory.GetFiles(dirPath);
            foreach (var file in files)
            {
                ChangeFile(file);
            }

            // 子文件夹
            var dirs = Directory.GetDirectories(dirPath);
            foreach (var dir in dirs)
            {
                ChangeDir(dir);
            }

            int index = dirPath.LastIndexOf("\\");
            string baseDir = dirPath.Substring(0, index);
            string dirName = dirPath.Substring(index + 1);
            
            if (dirName.IndexOf(old) >= 0)
            {
                var newDirName = dirName.Replace(old, now);
                var newDirPath = baseDir + "\\" + newDirName;
                Directory.Move(dirPath, newDirPath);
                Console.WriteLine($"{dirPath}已经被重命名为{newDirName}");
            }
        }

        private static void ChangeFile(string filePath)
        {
            // 文件内容
            var lines = File.ReadAllLines(filePath);
            var hasChange = false;
            var newLines = new List<string>();
            foreach (var line in lines)
            {
                var newLine = line;
                if (newLine.IndexOf(old)>=0)
                {
                    hasChange = true;
                    newLine = newLine.Replace(old, now);
                    Console.WriteLine($"{filePath}已经被修改");
                }
                newLines.Add(newLine);
            }

            if (hasChange)
            {
                File.WriteAllLines(filePath, newLines);
                Console.WriteLine();
            }

            // 文件名
            int index = filePath.LastIndexOf("\\");
            string baseDir = filePath.Substring(0, index);
            string fileName = filePath.Substring(index + 1);

            if (fileName.IndexOf(old) >= 0)
            {
                var newFileName = fileName.Replace(old, now);
                var newFilePath = baseDir + "\\" + newFileName;
                Directory.Move(filePath, newFilePath);
                Console.WriteLine($"{filePath}已经被重命名为{newFileName}");
            }
        }
    }
}
