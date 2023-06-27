using System;

namespace Mazes
{
	public static class EmptyMazeTask
	{
		public static void MoveRobot(Robot robot, int step, Direction direction)
        {
			for (int i = 0; i < step; i++)
				robot.MoveTo(direction);
        }

		public static void MoveOut(Robot robot, int width, int height)
		{
			MoveRobot(robot, width - 3, Direction.Right);
			MoveRobot(robot, height - 3, Direction.Down);
		}
	}
}