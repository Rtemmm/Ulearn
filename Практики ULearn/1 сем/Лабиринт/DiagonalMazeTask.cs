namespace Mazes
{
	public static class DiagonalMazeTask
	{
		public static void MoveRobot(Robot robot, int stepCount, Direction direction)
		{
			for (int i = 0; i < stepCount; i++)
				robot.MoveTo(direction);
		}

		public static void DiagonalMove(Robot robot, int stepCount, Direction firstDirection, Direction secondDirection)
        {
			while (!robot.Finished)
            {
				MoveRobot(robot, stepCount, firstDirection);

				if (robot.Finished)
					break;

				MoveRobot(robot, 1, secondDirection);
			}
        }

		public static void MoveOut(Robot robot, int width, int height)
		{
			if (height < width)
				DiagonalMove(robot, (width - 2) / (height - 2), Direction.Right, Direction.Down);
            else
				DiagonalMove(robot, (height - 2) / (width - 2), Direction.Down, Direction.Right);
		}
	}
}