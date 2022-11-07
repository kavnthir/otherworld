using System;

namespace otherworld
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Surface())
                game.Run();
        }
    }
}
