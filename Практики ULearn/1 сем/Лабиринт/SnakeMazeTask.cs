namespace Mazes
{
	public static class SnakeMazeTask
	{
		public static void MoveRobot(Robot robot, int step, Direction direction)
		{
			for (int i = 0; i < step; i++)
				robot.MoveTo(direction);
		}

		public static void OneSnakeMove(Robot robot, int width, int height)
        {
			MoveRobot(robot, width - 3, Direction.Right);
			MoveRobot(robot, 2, Direction.Down);
			MoveRobot(robot, width - 3, Direction.Left);
		}
		public static void MoveOut(Robot robot, int width, int height)
		{
			while (!robot.Finished)
			{
				OneSnakeMove(robot, width, height);

				if (robot.Finished)
					break;

				MoveRobot(robot, 2, Direction.Down);
			}		
		}
	}
}