using System;
using System.Runtime.Serialization;

namespace WeaponPalettes
{
	[Serializable]
	public struct WeaponSet
	{
		public WeaponSet(string mainHand, string offHand)
		{
			MainHand = mainHand;
			OffHand = offHand;
		}

		[DataMember] public readonly string MainHand;

		[DataMember] public readonly string OffHand;

		public bool Equals(WeaponSet other)
		{
			return MainHand == other.MainHand && OffHand == other.OffHand;
		}

		public override bool Equals(object obj)
		{
			return obj is WeaponSet other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((MainHand != null ? MainHand.GetHashCode() : 0) * 397) ^
				       (OffHand != null ? OffHand.GetHashCode() : 0);
			}
		}
	}
}