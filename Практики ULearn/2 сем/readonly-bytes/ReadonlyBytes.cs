using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hashes;

public class Fnv
{
	private int _hash = unchecked((int)2161624234456544661);
	private int _prime = 916777619;

	public int CreateHash(int length, byte[] bytes)
	{
		unchecked
		{
			for (var i = 0; i < length; i++)
				_hash = (_hash ^ bytes[i]) * _prime;
		}

		return _hash;
	}
}

public class ReadonlyBytes : IEnumerable<byte>
{
	public int Length => _bytes.Length;
	
	private Lazy<int> _hash;
	private readonly byte[] _bytes;
	
	public ReadonlyBytes(params byte[] bytes)
	{
		_bytes = bytes ?? throw new ArgumentNullException();
		_hash = new Lazy<int>(() => new Fnv().CreateHash(Length, _bytes));
	}

	public byte this[int index] => _bytes[index];

	public override string ToString() => $"[{string.Join(", ", _bytes)}]";

	public override bool Equals(object obj)
	{
		var bytes = obj as ReadonlyBytes; 
		
		if (ReferenceEquals(null, obj) || obj.GetType() != GetType() || bytes.Length != Length)
			return false;

		for (var i = 0; i < Length; i++)
			if (bytes[i] != _bytes[i])
				return false;

		return true;
	}

	public override int GetHashCode() => _hash.Value;

	public IEnumerator<byte> GetEnumerator()
	{
		for (var i = 0; i < Length; i++)
			yield return _bytes[i];
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
