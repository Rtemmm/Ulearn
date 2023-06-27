using System;
using System.Windows.Forms;

namespace Digger
{
    internal static class Program
    {
        public static Game game = new Game();
        [STAThread]
        private static void Main()
        {
            game.CreateMap();
            Application.Run(new DiggerWindow());
        }
    }
}