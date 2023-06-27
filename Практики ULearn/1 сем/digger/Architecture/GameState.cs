using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Digger
{
    public class GameState
    {
        public const int ElementSize = 32;
        public List<CreatureAnimation> Animations = new List<CreatureAnimation>();

        public void BeginAct()
        {
            Animations.Clear();
            for (var x = 0; x < Program.game.MapWidth; x++)
            for (var y = 0; y < Program.game.MapHeight; y++)
            {

                var creature = Program.game.Map[x, y];
                if (creature == null) continue;
                if (creature is Bomb) continue; 
                var command = creature.Act(x, y);

                if (x + command.DeltaX < 0 || x + command.DeltaX >= Program.game.MapWidth || y + command.DeltaY < 0 ||
                    y + command.DeltaY >= Program.game.MapHeight)
                    throw new Exception($"The object {creature.GetType()} falls out of the game field");

                Animations.Add(
                    new CreatureAnimation
                    {
                        Command = command,
                        Creature = creature,
                        Location = new Point(x * ElementSize, y * ElementSize),
                        TargetLogicalLocation = new Point(x + command.DeltaX, y + command.DeltaY)
                    });
            }

            for (var x = 0; x < Program.game.MapWidth; x++)
                for (var y = 0; y < Program.game.MapHeight; y++)
                {

                    var creature = Program.game.Map[x, y];
                    if (creature == null) continue;
                    if (!(creature is Bomb)) continue;
                    var command = creature.Act(x, y);

                    if (x + command.DeltaX < 0 || x + command.DeltaX >= Program.game.MapWidth || y + command.DeltaY < 0 ||
                        y + command.DeltaY >= Program.game.MapHeight)
                        throw new Exception($"The object {creature.GetType()} falls out of the game field");

                    Animations.Add(
                        new CreatureAnimation
                        {
                            Command = command,
                            Creature = creature,
                            Location = new Point(x * ElementSize, y * ElementSize),
                            TargetLogicalLocation = new Point(x + command.DeltaX, y + command.DeltaY)
                        });
                }

            Animations = Animations.OrderByDescending(z => z.Creature.GetDrawingPriority()).ToList();
        }

        public void EndAct()
        {
            var creaturesPerLocation = GetCandidatesPerLocation();
            for (var x = 0; x < Program.game.MapWidth; x++)
            for (var y = 0; y < Program.game.MapHeight; y++)
                Program.game.Map[x, y] = SelectWinnerCandidatePerLocation(creaturesPerLocation, x, y);
        }

        private static ICreature SelectWinnerCandidatePerLocation(List<ICreature>[,] creatures, int x, int y)
        {
            var candidates = creatures[x, y];
            var aliveCandidates = candidates.ToList();
            foreach (var candidate in candidates)
            foreach (var rival in candidates)
                if (rival != candidate && candidate.DeadInConflict(rival))
                    aliveCandidates.Remove(candidate);
            if (aliveCandidates.Count > 1)
                throw new Exception(
                    $"Creatures {aliveCandidates[0].GetType().Name} and {aliveCandidates[1].GetType().Name} claimed the same map cell");

            return aliveCandidates.FirstOrDefault();
        }

        private List<ICreature>[,] GetCandidatesPerLocation()
        {
            var X = 0;
            var Y = 0;

            var creatures = new List<ICreature>[Program.game.MapWidth, Program.game.MapHeight];
            for (var x = 0; x < Program.game.MapWidth; x++)
            for (var y = 0; y < Program.game.MapHeight; y++)
            {
                creatures[x, y] = new List<ICreature>();
                if (Program.game.Map[x, y] is Player)
                {
                    X = x;
                    Y = y;
                }
            }
                

            foreach (var e in Animations)
            {
                var x = e.TargetLogicalLocation.X;
                var y = e.TargetLogicalLocation.Y;

                var dist = Math.Sqrt((X - x) * (X - x) + (Y - y) * (Y - y));
                
                if (dist > 3)
                    e.Command.IsHidden = true;

                var nextCreature = e.Command.TransformTo ?? e.Creature;
                if (!e.Command.NeedToDie)
                    creatures[x, y].Add(nextCreature);
            }

            return creatures;
        }
    }
}