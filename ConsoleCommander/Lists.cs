using System.Collections;

namespace ConsoleCommander
{
    internal class Lists
    {
        public static ArrayList LeftList { get; set; } = new();
        public static ArrayList RightList { get; set; } = new();
        public static bool Left = false;
        public static void FillList(bool drive)
        {
            if (Left) { LeftList.Clear(); }
            else { RightList.Clear(); }

            if (drive)
            {
                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    _ = Left ? LeftList.Add(driveInfo) : RightList.Add(driveInfo);
                }
                return;
            }

            _ = Left ? LeftList.Add("/..") : RightList.Add("/..");

            DirectoryInfo currentDirectory = new(Left ? Input.LeftCurrentDirectory : Input.RightCurrentDirectory);

            foreach (DirectoryInfo directoryInfo in currentDirectory.EnumerateDirectories())
            {
                _ = Left ? LeftList.Add(directoryInfo) : RightList.Add(directoryInfo);
            }

            foreach (FileInfo fileInfo in currentDirectory.EnumerateFiles())
            {
                _ = Left ? LeftList.Add(fileInfo) : RightList.Add(fileInfo);
            }

            if (Input.LeftCurrentDirectory == Input.RightCurrentDirectory)
            {
                if (Left) { RightList.Clear(); RightList.AddRange(LeftList); }
                else { LeftList.Clear(); LeftList.AddRange(RightList); }
            }

        }
    }
}
