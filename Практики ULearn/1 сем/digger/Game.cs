using System;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace Digger
{
    public class Game
    {
        private const string mapWithPlayerTerrain = @"
TTT T
TTP T
T T T
TT TT";

        private const string mapWithPlayerTerrainSackGold = @"
PTTGTT TS
TST  TSTT
TTTTTTSTT
T TSTS TT
T TTTG ST
TSTSTT TT";

        private const string mapWithPlayerTerrainSackGoldMonster = @"
PTTGTT TST
TST  TSTTM
TTT TTSTTT
T TSTS TTT
T TTTGMSTS
T TMT M TS
TSTSTTMTTT
S TTST  TG
 TGST MTTT
 T  TMTTTT";
        
        private const string mapWithBomb = @"
PTTGTT TSS
TST  TSTTM
TTT TTSTTT
T TSTS TTT
T TTTGMSTS
T TMT M TS
TSTSTTMTTT
S TTST  TG
 TGST MTTT
 T  TMTTTT";

        public static string GenerateMap()
        {
            var random = new Random();
            var result = new StringBuilder();
            var size = random.Next(3, 20);
            var cells = "TTTTTTMGS   ";

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    var cell = random.Next(0, 11);
                    result.Append(cells[cell]);
                }

                if (x != size - 1)
                    result.Append(Environment.NewLine);
            }

            var positionXOfPlayer = random.Next(0, size);
            var positionYOfPlayer = random.Next(0, size);
            result[positionXOfPlayer * positionYOfPlayer] = 'P';
            return string.Format(result.ToString());
        }

        public ICreature[,] Map;

        private int scores;
        public int Scores
        {
            get => scores;
            set
            {
                if (value >= 0)
                    scores = value;
            }
        }

        public bool IsOver;

        public Keys KeyPressed;
        public int MapWidth => Map.GetLength(0);
        public int MapHeight => Map.GetLength(1);

        public void CreateMap()
        {
            Map = CreatureMapCreator.CreateMap(mapWithPlayerTerrainSackGoldMonster);
        }
    }
}