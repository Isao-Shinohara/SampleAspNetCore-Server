using MessagePack;

[MessagePackObject]
public class Address
{
	[Key(0)]
	public string Country { get; set; }
}
