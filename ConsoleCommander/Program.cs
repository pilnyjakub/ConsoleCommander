namespace ConsoleCommander
{
    internal class Program
    {
        private static void Main()
        {
            Lists.FillList(false);
            Lists.Left = true;
            Lists.FillList(false);
            Writer.Border(true);
            while (true)
            {
                Console.ForegroundColor = Input.foregroundColor;
                Writer.Write();
                Input.Navigation();
            }
        }
    }
}