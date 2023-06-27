using System;
using System.Text;

namespace hashes;

public class GhostsTask : 
	IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
	IMagic
{
	private readonly byte[] _content = { 1, 12, 123 };
	private readonly Vector _vector = new (1, 2);
	private readonly Segment _segment = new (new Vector(1, 1), new Vector(2, 2));
	private readonly Cat _cat = new ("Mok", "Moke", new DateTime(1234, 12, 1));
	private readonly Robot _robot = new ("Olo", 228);


	public void DoMagic()
	{
		_content[1] = _content[2];
		_vector.Add(new Vector(34, 43));
		_segment.End.Add(new Vector(34, 43));
		_cat.Rename("Moc");
		Robot.BatteryCapacity -= 32;
	}
	
	Vector IFactory<Vector>.Create() => _vector;

	Segment IFactory<Segment>.Create() => _segment;

	Cat IFactory<Cat>.Create() => _cat;

    Robot IFactory<Robot>.Create() => _robot;

    Document IFactory<Document>.Create() => new ("doc", Encoding.Default, _content);
}