using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleCommander
{
    internal class Writer
    {
        public static int ConsoleWidth = Console.BufferWidth;
        public static int ConsoleHeight = Console.BufferHeight;
        public static bool MessageBox(string message, int type = 0)
        {
            MessageBorder(message);
            if (type == 0)
            {
                Console.SetCursorPosition(ConsoleWidth / 4, (ConsoleHeight / 4) + (ConsoleHeight / 2) - 1);
                Console.WriteLine("Press any key to continue...".PadLeft((ConsoleWidth / 4) + ("Press any key to continue...".Length / 2)));
                switch (Console.ReadKey().Key)
                {
                    default:
                        Border(true);
                        return false;
                }
            }
            else if (type == 1)
            {
                Console.SetCursorPosition(ConsoleWidth / 4, (ConsoleHeight / 4) + (ConsoleHeight / 2) - 1);
                Console.WriteLine("Press Enter to confirm or any key to exit.".PadLeft((ConsoleWidth / 4) + ("Press Enter to confirm or any key to exit.".Length / 2)));
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        Border(true);
                        return true;
                    default:
                        Border(true);
                        return false;
                }
            }
            Border(true);
            return false;
        }
        public static string InputBox(string message)
        {
            MessageBorder(message);
            Console.SetCursorPosition(ConsoleWidth / 4, (ConsoleHeight / 4) + (ConsoleHeight / 2) - 1);
            Console.WriteLine("Press Enter to confirm or Escape to cancel.".PadLeft((ConsoleWidth / 4) + ("Press Enter to confirm or Escape to cancel.".Length / 2)));
            StringBuilder inputText = new();
            int offset = 0;
            int hiddenChars;
            int hiddenCharsEnd;
            while (true)
            {
                Console.BackgroundColor = Input.selectedColor;
                if (inputText.Length + 2 > ConsoleWidth / 2 - 2) { hiddenChars = inputText.Length + 1 - (ConsoleWidth / 2 - 2); }
                else { hiddenChars = 0; }
                if (offset >= ConsoleWidth / 2 - 2) { hiddenCharsEnd = offset + 1 - (ConsoleWidth / 2 - 2); }
                else { hiddenCharsEnd = 0; }
                Console.SetCursorPosition((ConsoleWidth / 4) + 1, (ConsoleHeight / 4) + (ConsoleHeight / 4));
                Console.Write("".PadRight((ConsoleWidth / 2) - 2));
                Console.SetCursorPosition((ConsoleWidth / 4) + 1, (ConsoleHeight / 4) + (ConsoleHeight / 4));
                Console.Write(inputText.ToString()[(hiddenChars - hiddenCharsEnd)..^hiddenCharsEnd]);
                Console.SetCursorPosition((ConsoleWidth / 4) + 1 + inputText.ToString()[(hiddenChars - hiddenCharsEnd)..^(hiddenCharsEnd)].Length - offset + hiddenCharsEnd, (ConsoleHeight / 4) + (ConsoleHeight / 4));
                ConsoleKeyInfo consoleKey = Console.ReadKey();
                if (consoleKey.Key == ConsoleKey.Backspace)
                {
                    if (inputText.ToString().Length - offset > 0)
                    {
                        _ = inputText.Remove(inputText.Length - 1 - offset, 1);
                    }
                    if (offset == inputText.Length + 1)
                    {
                        --offset;
                    }
                    continue;
                }
                else if (consoleKey.Key == ConsoleKey.Delete)
                {
                    if (offset != 0)
                    {
                        _ = inputText.Remove(inputText.Length - offset, 1);
                        offset--;
                    }
                }
                else if (consoleKey.Key == ConsoleKey.LeftArrow)
                {
                    if (offset != inputText.ToString().Length)
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
                    if (inputText.Length > 0 && inputText.Length < 251 && Regex.IsMatch(inputText.ToString(), "^[a-zA-Z0-9]"))
                    {
                        Border(true);
                        return inputText.ToString();
                    }
                }
                else if (consoleKey.Key == ConsoleKey.Escape)
                {
                    Console.BackgroundColor = Input.backgroundColor;
                    break;
                }
                else if (Regex.IsMatch(consoleKey.KeyChar.ToString(), "^[a-zA-Z0-9\\.\\-_\\s]$"))
                {
                    _ = inputText.Insert(inputText.Length - offset, consoleKey.KeyChar);
                }
                Console.BackgroundColor = Input.backgroundColor;
            }
            Border(true);
            return "";
        }

        public static (string, string) MultiInputBox(string message)
        {
            MessageBorder(message);
            Console.SetCursorPosition(ConsoleWidth / 4, (ConsoleHeight / 4) + (ConsoleHeight / 2) - 1);
            Console.WriteLine("Press Enter to confirm or Escape to cancel.".PadLeft((ConsoleWidth / 4) + ("Press Enter to confirm or Escape to cancel.".Length / 2)));
            string leftName = "";
            string rightName = "";
            if (Lists.LeftList[Input.LeftSelected].GetType() == typeof(FileInfo))
            {
                FileInfo fileInfo = (FileInfo)Lists.LeftList[Input.LeftSelected];
                leftName = "/" + fileInfo.Name;
            } else if (Lists.LeftList[Input.LeftSelected].GetType() == typeof(DirectoryInfo))
            {
                DirectoryInfo directoryInfo = (DirectoryInfo)Lists.LeftList[Input.LeftSelected];
                leftName = "/" + directoryInfo.Name;
            }
            if (Lists.RightList[Input.RightSelected].GetType() == typeof(FileInfo))
            {
                FileInfo fileInfo = (FileInfo)Lists.RightList[Input.RightSelected];
                rightName = "/" + fileInfo.Name;
            }
            else if (Lists.RightList[Input.RightSelected].GetType() == typeof(DirectoryInfo))
            {
                DirectoryInfo directoryInfo = (DirectoryInfo)Lists.RightList[Input.RightSelected];
                rightName = "/" + directoryInfo.Name;
            }
            StringBuilder sourceText = new(Lists.Left ? Input.LeftCurrentDirectory + leftName : Input.RightCurrentDirectory + rightName);
            StringBuilder destinationText = new(!Lists.Left ? Input.LeftCurrentDirectory + leftName : Input.RightCurrentDirectory + rightName);
            bool source = true;
            int sourceOffset = 0;
            int destinationOffset = 0;
            int sourceHiddenChars;
            int sourceHiddenCharsEnd;
            int destinationHiddenChars;
            int destinationHiddenCharsEnd;
            while (true)
            {
                Console.BackgroundColor = Input.selectedColor;

                if (sourceText.Length + 2 > ConsoleWidth / 2 - 2) { sourceHiddenChars = sourceText.Length + 1 - (ConsoleWidth / 2 - 2); }
                else { sourceHiddenChars = 0; }
                if (sourceOffset >= ConsoleWidth / 2 - 2) { sourceHiddenCharsEnd = sourceOffset + 1 - (ConsoleWidth / 2 - 2); }
                else { sourceHiddenCharsEnd = 0; }
                if (destinationText.Length + 2 > ConsoleWidth / 2 - 2) { destinationHiddenChars = destinationText.Length + 1 - (ConsoleWidth / 2 - 2); }
                else { destinationHiddenChars = 0; }
                if (destinationOffset >= ConsoleWidth / 2 - 2) { destinationHiddenCharsEnd = destinationOffset + 1 - (ConsoleWidth / 2 - 2); }
                else { destinationHiddenCharsEnd = 0; }

                Console.SetCursorPosition((ConsoleWidth / 4) + 1, (ConsoleHeight / 4) + (ConsoleHeight / 4));
                Console.Write("".PadRight((ConsoleWidth / 2) - 2));
                Console.SetCursorPosition((ConsoleWidth / 4) + 1, (ConsoleHeight / 4) + (ConsoleHeight / 4));
                Console.Write(sourceText.ToString()[(sourceHiddenChars - sourceHiddenCharsEnd)..^sourceHiddenCharsEnd]);
                Console.SetCursorPosition((ConsoleWidth / 4) + 1, (ConsoleHeight / 4) + (ConsoleHeight / 4) + 2);
                Console.Write("".PadRight((ConsoleWidth / 2) - 2));
                Console.SetCursorPosition((ConsoleWidth / 4) + 1, (ConsoleHeight / 4) + (ConsoleHeight / 4) + 2);
                Console.Write(destinationText.ToString()[(destinationHiddenChars - destinationHiddenCharsEnd)..^destinationHiddenCharsEnd]);
                if (source)
                {
                    Console.SetCursorPosition((ConsoleWidth / 4) + 1 + sourceText.ToString()[(sourceHiddenChars - sourceHiddenCharsEnd)..^(sourceHiddenCharsEnd)].Length - sourceOffset + sourceHiddenCharsEnd, (ConsoleHeight / 4) + (ConsoleHeight / 4));
                } else
                {
                    Console.SetCursorPosition((ConsoleWidth / 4) + 1 + destinationText.ToString()[(destinationHiddenChars - destinationHiddenCharsEnd)..^(destinationHiddenCharsEnd)].Length - destinationOffset + destinationHiddenCharsEnd, (ConsoleHeight / 4) + (ConsoleHeight / 4) + 2);
                }
                ConsoleKeyInfo consoleKey = Console.ReadKey();
                if (consoleKey.Key == ConsoleKey.Backspace)
                {
                    if (source && sourceText.ToString().Length - sourceOffset > 0)
                    {
                        _ = sourceText.Remove(sourceText.Length - 1 - sourceOffset, 1);
                    }
                    else if (!source && destinationText.ToString().Length - destinationOffset > 0)
                    {
                        _ = destinationText.Remove(destinationText.Length - 1 - destinationOffset, 1);
                    }
                    if (source && sourceOffset == sourceText.Length + 1)
                    {
                        --sourceOffset;
                    }
                    else if (!source && destinationOffset == destinationText.Length + 1)
                    {
                        --destinationOffset;
                    }
                    continue;
                }
                else if (consoleKey.Key == ConsoleKey.Delete)
                {
                    if (source && sourceOffset != 0)
                    {
                        _ = sourceText.Remove(sourceText.Length - sourceOffset, 1);
                        sourceOffset--;
                    }
                    else if (!source && destinationOffset != 0)
                    {
                        _ = destinationText.Remove(destinationText.Length - destinationOffset, 1);
                        destinationOffset--;
                    }
                }
                else if (consoleKey.Key == ConsoleKey.LeftArrow)
                {
                    if (source && sourceOffset != sourceText.ToString().Length)
                    {
                        ++sourceOffset;
                    }
                    else if (!source && destinationOffset != destinationText.ToString().Length)
                    {
                        ++destinationOffset;
                    }
                    continue;
                }
                else if (consoleKey.Key == ConsoleKey.RightArrow)
                {
                    if (source && sourceOffset != 0)
                    {
                        --sourceOffset;
                    }
                    else if (!source && destinationOffset != 0)
                    {
                        --destinationOffset;
                    }
                    continue;
                }
                else if (consoleKey.Key == ConsoleKey.Enter)
                {
                    if (sourceText.Length > 0 && destinationText.Length > 0 && Regex.IsMatch(sourceText.ToString(), "^[a-zA-Z0-9/\\\\]") && Regex.IsMatch(destinationText.ToString(), "^[a-zA-Z0-9/\\\\]"))
                    {
                        Border(true);
                        return (sourceText.ToString(), destinationText.ToString());
                    }
                }
                else if (consoleKey.Key == ConsoleKey.Escape)
                {
                    Console.BackgroundColor = Input.backgroundColor;
                    break;
                }
                else if (consoleKey.Key == ConsoleKey.Tab)
                {
                    _ = source ? source = false : source = true;
                }
                else if (Regex.IsMatch(consoleKey.KeyChar.ToString(), "^[a-zA-Z0-9:/\\\\\\.\\-_\\s]$"))
                {
                    _ = source ? sourceText.Insert(sourceText.Length - sourceOffset, consoleKey.KeyChar) : destinationText.Insert(destinationText.Length - destinationOffset, consoleKey.KeyChar);
                }
                Console.BackgroundColor = Input.backgroundColor;
            }
            Border(true);
            return ("", "");
        }
        public static void Reset()
        {
            if (ConsoleWidth > Console.BufferWidth || ConsoleHeight > Console.BufferHeight) { Console.Clear(); }
            Console.CursorTop = Console.WindowTop;
            ConsoleWidth = Console.BufferWidth;
            ConsoleHeight = Console.BufferHeight;
        }
        public static void Border(bool both = false)
        {
            Reset();
            Console.BackgroundColor = Input.backgroundColor;
            if (Lists.Left || both)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("┌");
                for (int i = 0; i < (ConsoleWidth / 2) - 2; i++)
                {
                    Console.Write("─");
                }
                Console.WriteLine("┐");
                Console.SetCursorPosition(2, 0);
                Console.Write(Input.LeftCurrentDirectory[(Input.LeftCurrentDirectory.Length - ConsoleWidth / 4)..]);
                Console.SetCursorPosition(0, 1);
                for (int i = 0; i < ConsoleHeight - 4; i++)
                {
                    Console.Write("│");
                    for (int j = 0; j < (ConsoleWidth / 2) - 2; j++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("│");
                }
                Console.Write("└");
                for (int i = 0; i < (ConsoleWidth / 2) - 2; i++)
                {
                    Console.Write("─");
                }
                Console.WriteLine("┘");
            }
            if (!Lists.Left || both)
            {
                Console.SetCursorPosition(ConsoleWidth / 2, 0);
                Console.Write("┌");
                for (int i = (ConsoleWidth / 2) - 2; i < ConsoleWidth - 4; i++)
                {
                    Console.Write("─");
                }
                Console.WriteLine("┐");
                Console.SetCursorPosition(ConsoleWidth / 2 + 2, 0);
                Console.Write(Input.RightCurrentDirectory[(Input.RightCurrentDirectory.Length - ConsoleWidth / 4)..]);
                for (int i = 0; i < ConsoleHeight - 4; i++)
                {
                    Console.SetCursorPosition(ConsoleWidth / 2, i + 1);
                    Console.Write("│");
                    for (int j = (ConsoleWidth / 2) - 2; j < ConsoleWidth - 4; j++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("│");
                }
                Console.SetCursorPosition(ConsoleWidth / 2, ConsoleHeight - 3);
                Console.Write("└");
                for (int i = (ConsoleWidth / 2) - 2; i < ConsoleWidth - 4; i++)
                {
                    Console.Write("─");
                }
                Console.WriteLine("┘");
            }
            string shortcuts = ConsoleWidth > 120 ? "F1 - Help │ F2 - Menu │ F3 - Open │ F4 - Edit │ F5 - Copy │ F6 - Move │ F7 - MkDir │ F8 - Rem │ F9 - MkFile │ F10 - Exit" : "F1 │ F2 │ F3 │ F4 │ F5 │ F6 │ F7 │ F8 │ F9 │ F10";
            Console.WriteLine(shortcuts.PadLeft((ConsoleWidth / 2) + (shortcuts.Length / 2)).PadRight(ConsoleWidth));
        }
        public static void MessageBorder(string message)
        {
            Reset();
            Console.SetCursorPosition((ConsoleWidth / 4) - 1, ConsoleHeight / 4);
            Console.Write("┌");
            for (int i = 0; i < ConsoleWidth / 2; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┐");
            for (int i = 0; i < ConsoleHeight / 2; i++)
            {
                Console.SetCursorPosition((ConsoleWidth / 4) - 1, (ConsoleHeight / 4) + i + 1);
                Console.Write("│");
                for (int j = 0; j < ConsoleWidth / 2; j++)
                {
                    Console.Write(" ");
                }
                Console.Write("│");
            }
            Console.SetCursorPosition((ConsoleWidth / 4) - 1, (ConsoleHeight / 4) + (ConsoleHeight / 2) + 1);
            Console.Write("└");
            for (int i = 0; i < ConsoleWidth / 2; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┘");
            Console.SetCursorPosition(ConsoleWidth / 4, (ConsoleHeight / 4) + 2);
            Console.WriteLine(message.PadLeft((ConsoleWidth / 4) + (message.Length / 2)));
        }
        public static void Write()
        {
            if (Console.BufferHeight != ConsoleHeight || Console.BufferWidth != ConsoleWidth) { Border(true); }
            if (Input.LeftSelected >= (ConsoleHeight - 4 + Input.LeftOffset)) { Input.LeftOffset++; }
            else if (Input.LeftSelected < Input.LeftOffset) { Input.LeftOffset--; }

            if (Input.RightSelected >= (ConsoleHeight - 4 + Input.RightOffset)) { Input.RightOffset++; }
            else if (Input.RightSelected < Input.RightOffset) { Input.RightOffset--; }

            for (int i = 0 + Input.LeftOffset; i < ((Lists.LeftList.Count > (ConsoleHeight - 4 + Input.LeftOffset)) ? ConsoleHeight - 4 + Input.LeftOffset : Lists.LeftList.Count); i++)
            {
                Console.BackgroundColor = i == Input.LeftSelected && Lists.Left ? Input.selectedColor : Input.backgroundColor;
                Console.SetCursorPosition(1, i + 1 - Input.LeftOffset);
                if (Lists.LeftList[i].GetType() == typeof(DriveInfo))
                {
                    DriveInfo driveInfo = (DriveInfo)Lists.LeftList[i];
                    string driveName = driveInfo.Name.Length > (ConsoleWidth / 2) - 12 ? driveInfo.Name[..((ConsoleWidth / 2) - 18)] + "..." : driveInfo.Name.PadRight((ConsoleWidth / 2) - 12);
                    Console.WriteLine(driveName + $" {Files.Size(driveInfo.TotalSize - driveInfo.TotalFreeSpace)}/{Files.Size(driveInfo.TotalSize)}".PadLeft(10));
                    continue;
                }

                if (Lists.LeftList[i].GetType() == typeof(string))
                {
                    Console.WriteLine(Lists.LeftList[i].ToString().PadRight((ConsoleWidth / 2) - 2));
                }
                else if (Lists.LeftList[i].GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo directoryInfo = (DirectoryInfo)Lists.LeftList[i];
                    string directoryName = ("/" + (directoryInfo.Name.Length > (ConsoleWidth / 2) - 26 ? directoryInfo.Name[..((ConsoleWidth / 2) - 29)] + "..." : directoryInfo.Name)).PadRight((ConsoleWidth / 2) - 23);
                    Console.WriteLine(directoryName + " DIR " + directoryInfo.LastWriteTime.ToString("dd-MM-yyyy HH:mm"));
                    continue;
                }
                else if (Lists.LeftList[i].GetType() == typeof(FileInfo))
                {
                    FileInfo fileInfo = (FileInfo)Lists.LeftList[i];
                    string fileName = fileInfo.Name.Length > (ConsoleWidth / 2) - 25 ? fileInfo.Name[..((ConsoleWidth / 2) - 28)] + "..." : fileInfo.Name.PadRight((ConsoleWidth / 2) - 25);
                    Console.WriteLine(fileName + $" {Files.Size(fileInfo.Length)} ".PadLeft(7) + fileInfo.LastWriteTime.ToString("dd-MM-yyyy HH:mm"));
                    continue;
                }
            }
            for (int i = 0 + Input.RightOffset; i < ((Lists.RightList.Count > (ConsoleHeight - 4 + Input.RightOffset)) ? ConsoleHeight - 4 + Input.RightOffset : Lists.RightList.Count); i++)
            {
                Console.SetCursorPosition((ConsoleWidth / 2) + 1, i + 1 - Input.RightOffset);
                Console.BackgroundColor = i == Input.RightSelected && !Lists.Left ? Input.selectedColor : Input.backgroundColor;
                if (Lists.RightList[i].GetType() == typeof(DriveInfo))
                {
                    DriveInfo driveInfo = (DriveInfo)Lists.RightList[i];
                    string driveName = driveInfo.Name.Length > (ConsoleWidth / 2) - 12 ? driveInfo.Name[..((ConsoleWidth / 2) - 18)] + "..." : driveInfo.Name.PadRight((ConsoleWidth / 2) - 12);
                    Console.WriteLine(driveName + $" {Files.Size(driveInfo.TotalSize - driveInfo.TotalFreeSpace)}/{Files.Size(driveInfo.TotalSize)}".PadLeft(10));
                    continue;
                }

                if (Lists.RightList[i].GetType() == typeof(string))
                {
                    Console.WriteLine(Lists.RightList[i].ToString().PadRight((ConsoleWidth / 2) - 2));
                }
                else if (Lists.RightList[i].GetType() == typeof(DirectoryInfo))
                {
                    DirectoryInfo directoryInfo = (DirectoryInfo)Lists.RightList[i];
                    string directoryName = ("/" + (directoryInfo.Name.Length > (ConsoleWidth / 2) - 26 ? directoryInfo.Name[..((ConsoleWidth / 2) - 29)] + "..." : directoryInfo.Name)).PadRight((ConsoleWidth / 2) - 23);
                    Console.WriteLine(directoryName + " DIR " + directoryInfo.LastWriteTime.ToString("dd-MM-yyyy HH:mm"));
                    continue;
                }
                else if (Lists.RightList[i].GetType() == typeof(FileInfo))
                {
                    FileInfo fileInfo = (FileInfo)Lists.RightList[i];
                    string fileName = fileInfo.Name.Length > (ConsoleWidth / 2) - 25 ? fileInfo.Name[..((ConsoleWidth / 2) - 28)] + "..." : fileInfo.Name.PadRight((ConsoleWidth / 2) - 25);
                    Console.WriteLine(fileName + $" {Files.Size(fileInfo.Length)} ".PadLeft(7) + fileInfo.LastWriteTime.ToString("dd-MM-yyyy HH:mm"));
                    continue;
                }
            }
            Console.BackgroundColor = Input.backgroundColor;
        }
        public static void Help()
        {
            Reset();
            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            for (int i = 0; i < ConsoleWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┐");

            for (int i = 0; i < ConsoleHeight - 4; i++)
            {
                Console.Write("│");
                for (int j = 0; j < ConsoleWidth - 2; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("│");
            }
            Console.Write("└");
            for (int i = 0; i < ConsoleWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┘");
            Console.WriteLine("Press any key to continue...".PadRight(ConsoleWidth - 2));
            string[] lines = new string[] { "Up/Down Arrow - Change row", "Tab - Change column", "Enter - Enter directories", "F1 - Shows this screen", "F2 - Set theme", "F3 (Enter) - Open file", "F4 - Open file in editing mode", "F5 - Copy file/directory", "F6 - Move/rename file/direcotry", "F7 - Create new directory", "F8 - Remove file/directory", "F9 - Create new file", "F10 - Exit app" };
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(1, i + 1);
                Console.WriteLine(lines[i]);
            }
            _ = Console.ReadKey();
            Border(true);
        }
        public static void Menu()
        {
            Reset();
            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            for (int i = 0; i < ConsoleWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┐");

            for (int i = 0; i < ConsoleHeight - 4; i++)
            {
                Console.Write("│");
                for (int j = 0; j < ConsoleWidth - 2; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("│");
            }
            Console.Write("└");
            for (int i = 0; i < ConsoleWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.WriteLine("┘");
            Console.WriteLine("Press number of theme you want to select or any key to exit...".PadRight(ConsoleWidth - 2));
            string[] lines = new string[] { "1 - Black", "2 - Blue", "3 - Red", "4 - Yellow", "5 - Green" };
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(1, i + 1);
                Console.WriteLine(lines[i]);
            }
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Input.backgroundColor = ConsoleColor.Black;
                    Input.selectedColor = ConsoleColor.DarkGray;
                    Input.foregroundColor = ConsoleColor.White;
                    break;
                case ConsoleKey.D2:
                    Input.backgroundColor = ConsoleColor.Blue;
                    Input.selectedColor = ConsoleColor.DarkBlue;
                    Input.foregroundColor = ConsoleColor.White;
                    break;
                case ConsoleKey.D3:
                    Input.backgroundColor = ConsoleColor.Red;
                    Input.selectedColor = ConsoleColor.DarkRed;
                    Input.foregroundColor = ConsoleColor.White;
                    break;
                case ConsoleKey.D4:
                    Input.backgroundColor = ConsoleColor.Yellow;
                    Input.selectedColor = ConsoleColor.DarkYellow;
                    Input.foregroundColor = ConsoleColor.Black;
                    break;
                case ConsoleKey.D5:
                    Input.backgroundColor = ConsoleColor.Green;
                    Input.selectedColor = ConsoleColor.DarkGreen;
                    Input.foregroundColor = ConsoleColor.White;
                    break;
            }
            Console.BackgroundColor = Input.backgroundColor;
            Console.ForegroundColor = Input.foregroundColor;
            Border(true);
        }
    }
}
