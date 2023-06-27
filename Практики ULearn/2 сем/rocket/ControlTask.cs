namespace func_rocket;

public class ControlTask
{
	public static Turn ControlRocket(Rocket rocket, Vector target) =>
		rocket.Direction * 1/3 + rocket.Velocity.Angle * 2/3 > (target - rocket.Location).Angle
			? Turn.Left
			: Turn.Right;
}