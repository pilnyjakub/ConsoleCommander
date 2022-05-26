namespace ConsoleCommander
{
    internal class Input
    {
        public static int LeftOffset = 0;
        public static int RightOffset = 0;
        public static int LeftSelected = 0;
        public static int RightSelected = 0;
        public static string LeftCurrentDirectory = Directory.GetCurrentDirectory();
        public static string RightCurrentDirectory = Directory.GetCurrentDirectory();
        public static ConsoleColor backgroundColor = ConsoleColor.Black;
        public static ConsoleColor foregroundColor = ConsoleColor.White;
        public static ConsoleColor selectedColor = ConsoleColor.DarkGray;
        public static void Navigation()
        {
            string inputText;
            (string source, string destination) multiInputText;
            Console.SetCursorPosition(0, Writer.ConsoleHeight - 1);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Tab:
                    Lists.Left = Lists.Left != true;
                    Directory.SetCurrentDirectory(Lists.Left ? LeftCurrentDirectory : RightCurrentDirectory);
                    return;
                case ConsoleKey.Enter:
                    if (Lists.Left)
                    {
                        if (Lists.LeftList[LeftSelected].GetType() == typeof(string))
                        {
                            if (Directory.GetDirectoryRoot(LeftCurrentDirectory) == LeftCurrentDirectory)
                            {
                                Lists.FillList(true);
                                Writer.Border();
                                LeftSelected = 0;
                                LeftOffset = 0;
                                return;
                            }
                            else
                            {
                                TrySet(Directory.GetParent(LeftCurrentDirectory).FullName);
                            }
                        }
                        else if (Lists.LeftList[LeftSelected].GetType() == typeof(DriveInfo))
                        {
                            DriveInfo driveInfo = (DriveInfo)Lists.LeftList[LeftSelected];
                            TrySet(driveInfo.RootDirectory.FullName);
                        }
                        else if (Lists.LeftList[LeftSelected].GetType() == typeof(DirectoryInfo))
                        {
                            DirectoryInfo directoryInfo = (DirectoryInfo)Lists.LeftList[LeftSelected];
                            TrySet(directoryInfo.FullName);
                        }
                        else { Files.Open(); }
                    }
                    else
                    {
                        if (Lists.RightList[RightSelected].GetType() == typeof(string))
                        {
                            if (Directory.GetDirectoryRoot(RightCurrentDirectory) == RightCurrentDirectory)
                            {
                                Lists.FillList(true);
                                Writer.Border();
                                RightSelected = 0;
                                RightOffset = 0;
                                return;
                            }
                            else
                            {
                                TrySet(Directory.GetParent(RightCurrentDirectory).FullName);
                            }
                        }
                        else if (Lists.RightList[RightSelected].GetType() == typeof(DriveInfo))
                        {
                            DriveInfo driveInfo = (DriveInfo)Lists.RightList[RightSelected];
                            TrySet(driveInfo.RootDirectory.FullName);
                        }
                        else if (Lists.RightList[RightSelected].GetType() == typeof(DirectoryInfo))
                        {
                            DirectoryInfo directoryInfo = (DirectoryInfo)Lists.RightList[RightSelected];
                            TrySet(directoryInfo.FullName);
                        }
                        else { Files.Open(); }
                    }
                    Lists.FillList(false);
                    Writer.Border();
                    return;
                case ConsoleKey.UpArrow:
                    if (Lists.Left) { if (LeftSelected != 0) { --LeftSelected; } }
                    else { if (RightSelected != 0) { --RightSelected; } }
                    return;
                case ConsoleKey.DownArrow:
                    if (Lists.Left) { if (LeftSelected != Lists.LeftList.Count - 1) { ++LeftSelected; } }
                    else { if (RightSelected != Lists.RightList.Count - 1) { ++RightSelected; } }
                    return;
                case ConsoleKey.End:
                    if (Lists.Left) { LeftSelected = Lists.LeftList.Count - 1; LeftOffset = Lists.LeftList.Count - Writer.ConsoleHeight + 1 > 0 ? Lists.LeftList.Count - Writer.ConsoleHeight + 1 : 0; }
                    else { RightSelected = Lists.RightList.Count - 1; RightOffset = Lists.RightList.Count - Writer.ConsoleHeight + 1 > 0 ? Lists.RightList.Count - Writer.ConsoleHeight + 1 : 0; }
                    return;
                case ConsoleKey.Home:
                    if (Lists.Left) { LeftSelected = 0; LeftOffset = 0; }
                    else { RightSelected = 0; RightOffset = 0; }
                    return;
                case ConsoleKey.F10:
                    Console.Clear();
                    Environment.Exit(0);
                    return;
                case ConsoleKey.F9:
                    inputText = Writer.InputBox("Input name of file.");
                    if (!string.IsNullOrWhiteSpace(inputText))
                    {
                        Files.Make(inputText, true);
                        Lists.FillList(false);
                    }
                    return;
                case ConsoleKey.F8:
                    if (Writer.MessageBox("Do you really want to remove this file or directory?", 1))
                    {
                        Files.Remove();
                        Lists.FillList(false);
                        if (Lists.Left)
                        {
                            --LeftSelected; if (LeftOffset > 0)
                            {
                                --LeftOffset;
                            }
                        }
                        else
                        {
                            --RightSelected; if (RightOffset > 0)
                            {
                                --RightOffset;
                            }
                        }
                    }
                    return;
                case ConsoleKey.F7:
                    inputText = Writer.InputBox("Input name of directory.");
                    if (!string.IsNullOrWhiteSpace(inputText))
                    {
                        Files.Make(inputText);
                        Lists.FillList(false);
                    }
                    return;
                case ConsoleKey.F6:
                    multiInputText = Writer.MultiInputBox("Move/Rename: Type source and destination path.");
                    if (!string.IsNullOrWhiteSpace(multiInputText.source) && !string.IsNullOrWhiteSpace(multiInputText.destination))
                    {
                        Files.Move(multiInputText.source, multiInputText.destination, true);
                    }
                    return;
                case ConsoleKey.F5:
                    multiInputText = Writer.MultiInputBox("Copy: Type source and destination path.");
                    if (!string.IsNullOrWhiteSpace(multiInputText.source) && !string.IsNullOrWhiteSpace(multiInputText.destination))
                    {
                        Files.Move(multiInputText.source, multiInputText.destination);
                    }
                    return;
                case ConsoleKey.F4:
                    Files.Edit();
                    return;
                case ConsoleKey.F3:
                    Files.Open();
                    return;
                case ConsoleKey.F2:
                    Writer.Menu();
                    return;
                case ConsoleKey.F1:
                    Writer.Help();
                    return;
                default:
                    return;
            }
        }
        public static void TrySet(string path = "")
        {
            try
            {
                Directory.SetCurrentDirectory(path);
                if (Lists.Left)
                {
                    LeftCurrentDirectory = path;
                    LeftSelected = 0;
                    LeftOffset = 0;
                }
                else
                {
                    RightCurrentDirectory = path;
                    RightSelected = 0;
                    RightOffset = 0;
                }
            }
            catch (Exception)
            {
                _ = Writer.MessageBox("Error: Access Denied");
            }
        }
    }
}
