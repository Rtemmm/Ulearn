using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
	private static readonly Physics StandardPhysics = new();

	private static readonly Rocket Rocket = new (new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
	private static readonly Vector Target = new (700, 500);

	public static IEnumerable<Level> CreateLevels()
	{
		yield return new Level("Zero", Rocket, Target,
			(_, _) => Vector.Zero, StandardPhysics);

		yield return new Level("Heavy", Rocket, Target, 
			(_, _) => new Vector(0, 0.9), StandardPhysics);
		
		yield return new Level("Up", Rocket, Target,
			(size, v) => new Vector(0d, -300 / (size.Y - v.Y + 300.0)), StandardPhysics);

		yield return new Level("WhiteHole", Rocket, Target, WhiteHoleGravity, StandardPhysics);

		yield return new Level("BlackHole", Rocket, Target, BlackHoleGravity, StandardPhysics);
		
		yield return new Level("BlackAndWhite", Rocket, Target, BlackAndWhiteHolesGravity, StandardPhysics);
	}
	
	private static Vector WhiteHoleGravity(Vector size, Vector v)
	{
		var d = (Target - v).Length;

		return (Target - v).Normalize() * (-140 * d) / (d * d + 1);
	}

	private static Vector BlackHoleGravity(Vector size, Vector v)
	{
		var anomaly = (Target + Rocket.Location) / 2;
		var d = (anomaly - v).Length;

		return (anomaly - v).Normalize() * (300 * d) / (d * d + 1);
	}

	private static Vector BlackAndWhiteHolesGravity(Vector size, Vector v) => 
		(WhiteHoleGravity(size, v) + BlackHoleGravity(size, v)) / 2;
}