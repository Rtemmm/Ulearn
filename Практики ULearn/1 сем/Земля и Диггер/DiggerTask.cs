using System;
using System.Windows.Forms;

namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName() => "Terrain.png";
        
        public int GetDrawingPriority() => 0;

        public CreatureCommand Act(int x, int y) => new CreatureCommand();

        public bool DeadInConflict(ICreature conflictedObject) => true;
    }

    public class Player : ICreature
    {
        public static int PosX;
        public static int PosY;

        public static bool IsAlive()
        {
            for (var x = 0; x < Game.MapWidth; x++)
                for (var y = 0; y < Game.MapHeight; y++)
                    if (Game.Map[x, y] is Player)
                    {
                        Player.PosX = x;
                        Player.PosY = y;

                        return true;
                    }

            return false;
        }

        public string GetImageFileName() => "Digger.png";

        public int GetDrawingPriority() => 1;

        public CreatureCommand Act(int x, int y)
        {
            var creatureCommand = new CreatureCommand();
            
            switch (Game.KeyPressed)
            {
                case Keys.Up: 
                    if (y > 0) 
                        creatureCommand.DeltaY = -1;
                    break;
                case Keys.Down:
                    if (y < Game.MapHeight - 1) 
                        creatureCommand.DeltaY = 1;
                    break;          
                case Keys.Left:
                    if (x > 0) 
                        creatureCommand.DeltaX = -1;
                    break;              
                case Keys.Right:
                    if (x < Game.MapWidth - 1) 
                        creatureCommand.DeltaX = 1;
                    break;
            }
            
            if (Game.Map[x + creatureCommand.DeltaX, y + creatureCommand.DeltaY] is Sack)
                return new CreatureCommand();

            return creatureCommand;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Gold)
                Game.Scores += 10;

            return conflictedObject is Sack || conflictedObject is Monster;
        }
    }

    public class Sack : ICreature
    {
        private int fallCount;

        public string GetImageFileName() => "Sack.png";

        public int GetDrawingPriority() => 2;

        bool IsFallPossible(int x, int y) => Game.Map[x, y] == null || 
            (Game.Map[x, y] is Player || Game.Map[x, y] is Monster) && fallCount > 0;
        
        CreatureCommand Fall() => new CreatureCommand { DeltaY = 1 };
        
        CreatureCommand Break() => new CreatureCommand { TransformTo = new Gold() };
        
        CreatureCommand Lay() => new CreatureCommand();
        
        public CreatureCommand Act(int x, int y)
        {
            while (y < Game.MapHeight - 1)
            {
                if (IsFallPossible(x, y + 1)) 
                {
                    fallCount++;
                    return Fall();
                }

                if (fallCount > 1)
                    return Break();

                fallCount = 0;
                return Lay();
            }

            if (fallCount > 1)
                return Break();

            fallCount = 0;
            return Lay();
        }

        public bool DeadInConflict(ICreature conflictedObject) => false;
    }

    public class Gold : ICreature
    {
        public string GetImageFileName() => "Gold.png";

        public int GetDrawingPriority() => 3;

        public CreatureCommand Act(int x, int y) => new CreatureCommand();

        public bool DeadInConflict(ICreature conflictedObject) => true;
    }
    
    public class Monster : ICreature
    {
        public string GetImageFileName() => "Monster.png";

        public int GetDrawingPriority() => 4;

        public CreatureCommand Act(int x, int y)
        {
            var creatureCommand = new CreatureCommand();
            
            if (Player.IsAlive())
            {
                if (Player.PosX < x && !(Game.Map[x - 1, y] is Terrain))
                    creatureCommand.DeltaX = -1;
                else if (Player.PosX > x && !(Game.Map[x + 1, y] is Terrain))
                    creatureCommand.DeltaX = 1;
                else if (Player.PosY < y && !(Game.Map[x, y - 1] is Terrain))
                    creatureCommand.DeltaY = -1;
                else if (Player.PosY > y && !(Game.Map[x, y + 1] is Terrain))
                    creatureCommand.DeltaY = 1;
            }
            else
                return creatureCommand;
            
            var posX = x + creatureCommand.DeltaX;
            var posY = y + creatureCommand.DeltaY;

            if (posX < 0 || posX > Game.MapWidth || posY < 0 || posY > Game.MapHeight)
                return new CreatureCommand();

            if (Game.Map[posX, posY] is Sack || Game.Map[posX, posY] is Terrain || 
                Game.Map[posX, posY] is Monster)    
                return new CreatureCommand();

            return creatureCommand;
        }   

        public bool DeadInConflict(ICreature conflictedObject) => conflictedObject is Sack || 
            conflictedObject is Monster;
    }
}


