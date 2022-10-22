using System;
using System.Runtime.Serialization;

namespace WeaponPalettes.Model
{
	[Serializable]
	public record WeaponSet(
		[property: DataMember] string MainHand,
		[property: DataMember] string OffHand)
	{
		public static WeaponSet Empty { get; } = new(string.Empty, string.Empty);
	}
}