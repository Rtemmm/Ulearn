using System.Windows.Forms;

namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName() => "Terrain.png";
        
        public bool DeadInConflict(ICreature conflictedObject) => conflictedObject is Player
                                                                  || conflictedObject is BombExplosion;
        
        public int GetDrawingPriority() => 0;
        
        public CreatureCommand Act(int x, int y) => new CreatureCommand();
        
    }

    public class Player : ICreature
    {
        public static int X;
        public static int Y;
        
        public string GetImageFileName() => "Digger.png";

        public bool DeadInConflict(ICreature conflictedObject) =>
            conflictedObject is Sack || conflictedObject is Monster || conflictedObject is BombExplosion;
        
        public int GetDrawingPriority() => 1;

        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand();
            switch (Program.game.KeyPressed)
            {
                case Keys.Up:
                    command.DeltaY = IsCanMoveTo(x, y - 1) ? -1 : 0;
                    break;
                case Keys.Down:
                    command.DeltaY = IsCanMoveTo(x, y + 1) ? 1 : 0;
                    break;
                case Keys.Right:
                    command.DeltaX = IsCanMoveTo(x + 1, y) ? 1 : 0;
                    break;
                case Keys.Left:
                    command.DeltaX = IsCanMoveTo(x - 1, y) ? -1 : 0;
                    break;
                case Keys.W:
                    Program.game.Map[x, y - 1] = new Bomb(Bomb.Directions.Up);
                    break;
                case Keys.A:
                    Program.game.Map[x - 1 , y] = new Bomb(Bomb.Directions.Left);
                    break;
                case Keys.S:
                    Program.game.Map[x, y + 1] = new Bomb(Bomb.Directions.Down);
                    break;
                case Keys.D:
                    Program.game.Map[x + 1, y] = new Bomb(Bomb.Directions.Right);
                    break;
            }
            return command;
        }
        
        public static bool IsPlayerOnMap()
        {
            for (var x = 0; x < Program.game.MapWidth; x++)
            {
                for (var y = 0; y < Program.game.MapHeight; y++)
                {
                    if (Program.game.Map[x, y] is Player)
                    {
                        X = x;
                        Y = y;
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsCanMoveTo(int x, int y) =>
            IsCoordinatesInMap(x, y)
            && !(Program.game.Map[x, y] is Sack
                || Program.game.Map[x, y] is Bomb
                || Program.game.Map[x, y] is BombPlanted);
        
        public static bool IsCoordinatesInMap(int x, int y) =>
            x >= 0 && x < Program.game.MapWidth && y >= 0 && y < Program.game.MapHeight;
    }

    public class Sack : ICreature
    {
        private int dropCells;
        
        public string GetImageFileName() => "Sack.png";

        public bool DeadInConflict(ICreature conflictedObject) => conflictedObject is BombExplosion;

        public int GetDrawingPriority() => 2;

        private bool IsCanDrop(int y, ICreature objectInCell) =>
            objectInCell == null || ((objectInCell is Player || objectInCell is Monster) && dropCells > 0);

        public CreatureCommand Act(int x, int y)
        {
            if (y != Program.game.MapHeight - 1 && IsCanDrop(y, Program.game.Map[x, y + 1]))
            {
                dropCells++;
                return Fall();
            }

            return dropCells > 1 ? Break() : Lay();
        }

        private CreatureCommand Fall() => new CreatureCommand() { DeltaY = 1 };

        private CreatureCommand Break() => new CreatureCommand() { TransformTo = new Gold() };

        private CreatureCommand Lay()
        {
            dropCells = 0;
            return new CreatureCommand();
        }
    }

    public class Gold : ICreature
    {
        public string GetImageFileName() => "Gold.png";

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
                Program.game.Scores += 10;
            return true;
        }
        
        public int GetDrawingPriority() => 3;
        
        public CreatureCommand Act(int x, int y) => new CreatureCommand();
    }

    public class Monster : ICreature
    {
        public string GetImageFileName() => "Monster.png";

		public bool DeadInConflict(ICreature conflictedObject) =>
			conflictedObject is Sack || conflictedObject is Monster || conflictedObject is BombExplosion;

        public int GetDrawingPriority() => 0;

        public CreatureCommand Act(int x, int y)
        {
            var deltaX = 0;
            var deltaY = 0;
            if (Player.IsPlayerOnMap())
            {
                if (x == Player.X)
                    deltaY = y > Player.Y ? -1 : 1;
                else
                    deltaX = x > Player.X ? -1 : 1;
            }

            return IsCanMoveTo(x + deltaX, y + deltaY)
                ? new CreatureCommand() { DeltaX = deltaX, DeltaY = deltaY }
                : new CreatureCommand();
        }

        private static bool IsCanMoveTo(int x, int y)
        {
            var objectInCell = Program.game.Map[x, y];
            return Player.IsCoordinatesInMap(x, y) &&
                   (objectInCell == null || !(objectInCell is Terrain)
                       && !(objectInCell is Monster) && !(objectInCell is Sack) && !(objectInCell is BombPlanted));
        }
    }

    public class Bomb : ICreature
    {
        public enum Directions
        {
            Up,
            Right,
            Down,
            Left
        }

        private int cellsToGo = 2;
        private readonly Directions direction;

        public Bomb(Directions direction)
        {
            this.direction = direction;
        }
        
        public string GetImageFileName() => "bomb.png";

        public int GetDrawingPriority() => 3;

        public CreatureCommand Act(int x, int y)
        {
            cellsToGo -= 1;
            var deltaX = 0;
            var deltaY = 0;
            switch (direction)
            {
                case Directions.Up:
                    deltaY = IsCanMoveTo(x, y - 1) ? -1 : 0;
                    break;
                case Directions.Down:
                    deltaY = IsCanMoveTo(x, y + 1) ? 1 : 0;
                    break;
                case Directions.Left:
                    deltaX = IsCanMoveTo(x - 1, y) ? -1 : 0;
                    break;
                case Directions.Right:
                    deltaX = IsCanMoveTo(x + 1, y) ? 1 : 0;
                    break;
            }

            return (deltaX == 0 && deltaY == 0) || cellsToGo == 0
                ? new CreatureCommand { TransformTo = new BombPlanted() }
                : new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };
        }

        public bool IsCanMoveTo(int x, int y)
        {
            if (x >= 0 && x < Program.game.MapWidth - 1 && y >= 0 && y < Program.game.MapHeight - 1)
            {
                var objectInCell = Program.game.Map[x, y];
                if (!(objectInCell is Terrain || objectInCell is Sack || objectInCell is Monster))
                    return true;
            }
            return false;
        }
            

        public bool DeadInConflict(ICreature conflictedObject) => false;
    }

    public class BombPlanted : ICreature
    {
        private int timer = 30;

        public string GetImageFileName() => "bomb_planted.png";

        public int GetDrawingPriority() => 6;

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public CreatureCommand Act(int x, int y)
        {
            if (--timer == 0)
            {
                var maxX = x < Program.game.MapWidth - 1 ? 1 : 0;
                var minX = x > 0 ? -1 : 0;
                var maxY = y < Program.game.MapHeight - 1 ? 1 : 0;
                var minY = y > 0 ? -1 : 0;
            
                for (var deltaX = minX; deltaX <= maxX; deltaX++)
                {
                    for (var deltaY = minY; deltaY <= maxY; deltaY++)
                        Program.game.Map[x + deltaX, y + deltaY] = new BombExplosion();

                }

                return new CreatureCommand() { TransformTo = new BombExplosion() };
            }
            return new CreatureCommand();
        }
    }

    public class BombExplosion : ICreature
    {
        private int timer = 5;
        
        public string GetImageFileName() => "bomb_explanation.png";

        public int GetDrawingPriority() => 6;

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public CreatureCommand Act(int x, int y)
        {
            if (--timer == 0)
                return new CreatureCommand { NeedToDie = true };
            return new CreatureCommand();
        }
        
    }
}