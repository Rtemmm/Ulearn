using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
	public Rocket GetNextMove(Rocket rocket)
	{
		var turn = Turn.None;
		var maxScore = double.MinValue;
		
		Parallel.ForEach(CreateTasks(rocket), task =>
			{
				var taskResult = task.GetAwaiter().GetResult();

				if (taskResult.Score < maxScore)
					return;

				turn = taskResult.Turn;
				maxScore = taskResult.Score;
			}
		);

		return rocket.Move(turn, level);
	}

	public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
	{
		return new List<Task<(Turn Turn, double Score)>>
			{ 
				Task.Run(() => SearchBestMove(rocket, new 
					Random(random.Next()), iterationsCount / threadsCount)),
				Task.Run(() => SearchBestMove(rocket, new 
					Random(random.Next()), iterationsCount / threadsCount))
			};
	}
}