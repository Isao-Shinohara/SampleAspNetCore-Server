using MessagePack;

[MessagePackObject]
public class Stats
{
	[Key(0)]
	public int ConnectionNum { get; set; }
}
