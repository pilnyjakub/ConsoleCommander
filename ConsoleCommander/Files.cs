using System.Text;

namespace ConsoleCommander
{
    internal class Files
    {
        public static string Size(long size)
        {
            int unit = 0;
            while (size >= 1000)
            {
                size /= 1000;
                unit++;
            }
            return size + new string[] { "B", "k", "M", "G", "T", "P", "E", "Z", "Y" }[unit];
        }
        public static void Open()
        {
            FileInfo fileInfo;
            Console.Clear();
            if (Lists.Left && Lists.LeftList[Input.LeftSelected].GetType() == typeof(FileInfo))
            {
                fileInfo = (FileInfo)Lists.LeftList[Input.LeftSelected];
                try
                {
                    Console.WriteLine($" {fileInfo.Name} ".PadLeft(Writer.ConsoleWidth / 2 + fileInfo.Name.Length / 2, '=').PadRight(Writer.ConsoleWidth, '='));
                    Console.WriteLine(File.ReadAllText(fileInfo.FullName));
                    _ = Console.ReadKey();
                }
                catch (Exception)
                {
                    _ = Writer.MessageBox("Error: You don't have access to this file or it's in use.");
                }
            }
            else if (!Lists.Left && Lists.RightList[Input.RightSelected].GetType() == typeof(FileInfo))
            {
                fileInfo = (FileInfo)Lists.RightList[Input.RightSelected];
                try
                {
                    Console.WriteLine($" {fileInfo.Name} ".PadLeft(Writer.ConsoleWidth / 2 + fileInfo.Name.Length / 2, '=').PadRight(Writer.ConsoleWidth, '='));
                    Console.WriteLine(File.ReadAllText(fileInfo.FullName));
                    _ = Console.ReadKey();
                }
                catch (Exception)
                {
                    _ = Writer.MessageBox("Error: You don't have access to this file or it's in use.");
                }
            }
            Writer.Border(true);
        }
        public static void Make(string name, bool file = false)
        {
            try
            {
                string fullName = $@"{Directory.GetCurrentDirectory()}\{name}";
                if (!file) { _ = Directory.CreateDirectory(fullName); }
                else { File.Create(fullName).Close(); }
            }
            catch (Exception)
            {
                _ = Writer.MessageBox("Error: Access denied.");
            }
        }
        public static void Remove()
        {
            try
            {
                if (Lists.Left && Lists.LeftList[Input.LeftSelected].GetType() == typeof(FileInfo))
                {
                    FileInfo fileInfo = (FileInfo)Lists.LeftList[Input.LeftSelected];
                    File.Delete(fileInfo.FullName);
                }
                else if (!Lists.Left && Lists.RightList[Input.RightSelected].GetType() == typeof(FileInfo))
                {
                    FileInfo fileInfo = (FileInfo)Lists.RightList[Input.RightSelected];
                    File.Delete(fileInfo.FullName);
                }
                else if (Lists.Left && Lists.LeftList[Input.LeftSelected].GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo directoryInfo = (DirectoryInfo)Lists.LeftList[Input.LeftSelected];
                    Directory.Delete(directoryInfo.FullName, true);
                }
                else if (!Lists.Left && Lists.RightList[Input.RightSelected].GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo directoryInfo = (DirectoryInfo)Lists.RightList[Input.RightSelected];
                    Directory.Delete(directoryInfo.FullName, true);
                }
            }
            catch (Exception)
            {
                _ = Writer.MessageBox("Error: Access denied.");
            }
        }
        public static void Move(string source, string destination, bool move = false)
        {
            if (move)
            {
                try
                {
                    if (File.Exists(source))
                    {
                        if (Directory.Exists(destination))
                        {
                            FileInfo fileInfo = new(source);
                            File.Move(source, destination + "/" + fileInfo.Name);
                        }
                        else
                        {
                            File.Move(source, destination);
                        }
                    }
                    else if (Directory.Exists(source))
                    {
                        DirectoryInfo sourceInfo = new(source);
                        if (Directory.Exists(destination))
                        {
                            Directory.Move(source, destination + "/" + sourceInfo.Name);
                        }
                        else if (File.Exists(destination))
                        {
                            Directory.Move(source, Directory.GetParent(destination).FullName + "/" + sourceInfo.Name);
                        } else
                        {
                            _ = Writer.MessageBox("Error: Destination is not valid.");
                        }
                    }
                    else
                    {
                        _ = Writer.MessageBox("Error: Source is not valid.");
                    }
                }
                catch (Exception)
                {
                    _ = Writer.MessageBox("Error: Access denied.");
                }
            } else
            {
                try
                {
                    if (File.Exists(source))
                    {
                        if (Directory.Exists(destination))
                        {
                            FileInfo fileInfo = new(source);
                            File.Copy(source, destination + "/" + fileInfo.Name);
                        }
                        else
                        {
                            File.Copy(source, destination);
                        }
                    }
                    else if (Directory.Exists(source))
                    {
                        if (Directory.Exists(destination))
                        {
                            CopyFilesRecursively(Directory.GetParent(source), new DirectoryInfo(destination));
                        }
                        else
                        {
                            _ = Writer.MessageBox("Error: Destination is valid.");
                        }
                    }
                    else
                    {
                        _ = Writer.MessageBox("Error: Source is not valid.");
                    }
                }
                catch (Exception)
                {
                    _ = Writer.MessageBox("Error: Access denied.");
                }
            }
            Input.LeftSelected = 0;
            Input.RightSelected = 0;
            Lists.FillList(false);
            Lists.Left = Lists.Left != true;
            Lists.FillList(false);
            Lists.Left = Lists.Left != true;
        }
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
        public static void Edit()
        {
            FileInfo fileInfo;
            if (Lists.Left && Lists.LeftList[Input.LeftSelected].GetType() == typeof(FileInfo))
            {
                fileInfo = (FileInfo)Lists.LeftList[Input.LeftSelected];
                try
                {
                    Editor(fileInfo);
                }
                catch (Exception)
                {
                    _ = Writer.MessageBox("Error: You don't have access to this file or it's in use.");
                }
            }
            else if (!Lists.Left && Lists.RightList[Input.RightSelected].GetType() == typeof(FileInfo))
            {
                fileInfo = (FileInfo)Lists.RightList[Input.RightSelected];
                try
                {
                    Editor(fileInfo);
                }
                catch (Exception)
                {
                    _ = Writer.MessageBox("Error: You don't have access to this file or it's in use.");
                }
            }
            Writer.Border(true);
        }
        public static void Editor(FileInfo fileInfo)
        {
            StringBuilder stringBuilder = new(File.ReadAllText(fileInfo.FullName));
            int offset = 0;
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($" {fileInfo.Name} ".PadLeft(Writer.ConsoleWidth / 2 + fileInfo.Name.Length / 2, '=').PadRight(Writer.ConsoleWidth, '='));
                Console.SetCursorPosition(0, 1);
                Console.WriteLine(stringBuilder.ToString());
                Console.SetCursorPosition(stringBuilder.ToString().Length - offset, stringBuilder.ToString().Split('\n').Length);
                ConsoleKeyInfo consoleKey = Console.ReadKey();
                if(consoleKey.Modifiers == ConsoleModifiers.Control && consoleKey.Key == ConsoleKey.X)
                {
                    break;
                } else if (consoleKey.Modifiers == ConsoleModifiers.Control && consoleKey.Key == ConsoleKey.S)
                {
                    File.WriteAllText(fileInfo.FullName, stringBuilder.ToString());
                    break;
                } else if (consoleKey.Key == ConsoleKey.Backspace)
                {
                    if (stringBuilder.ToString().Length - offset > 0)
                    {
                        _ = stringBuilder.Remove(stringBuilder.Length - 1 - offset, 1);
                    }
                    if (offset == stringBuilder.Length + 1)
                    {
                        --offset;
                    }
                    continue;
                }
                else if (consoleKey.Key == ConsoleKey.Delete)
                {
                    if (offset != 0)
                    {
                        _ = stringBuilder.Remove(stringBuilder.Length - offset, 1);
                        offset--;
                    }
                }
                else if (consoleKey.Key == ConsoleKey.LeftArrow)
                {
                    if (offset != stringBuilder.ToString().Length)
                    {
                        ++offset;
                    }
                    continue;
                }
                else if (consoleKey.Key == ConsoleKey.RightArrow)
                {
                    if (offset != 0)
                    {
                        --offset;
                    }
                    continue;
                }
                else if (consoleKey.Key == ConsoleKey.Enter)
                {
                    _ = stringBuilder.Insert(stringBuilder.Length - offset, '\n');
                }
                else
                {
                    _ = stringBuilder.Insert(stringBuilder.Length - offset, consoleKey.KeyChar);
                }
            }
            Writer.Border(true);
        }
    }
}
