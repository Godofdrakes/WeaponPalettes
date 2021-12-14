using System;
using System.Runtime.Serialization;

namespace WeaponPalettes
{
	[Serializable]
	public struct WeaponSet
	{
		private static string GetWeaponType(Weapon weapon, bool isOffhand) =>
			weapon != null && !isOffhand || Plugin.MatchOffhand ? weapon.Type.ToString() : string.Empty;

		private static string GetWeaponUid(Weapon weapon, bool isOffhand) =>
			weapon != null && Plugin.MatchUid && (!isOffhand || Plugin.MatchOffhand) ? weapon.UID : string.Empty;

		public static WeaponSet FromCharacter(Character character)
		{
			var weaponType = GetWeaponType(character.CurrentWeapon, false);
			var weaponUid = GetWeaponUid(character.CurrentWeapon, false);
			var offhandType = GetWeaponType(character.LeftHandWeapon, true);
			var offhandUid = GetWeaponUid(character.LeftHandWeapon, true);
			return new WeaponSet(weaponType, weaponUid, offhandType, offhandUid);
		}

		public WeaponSet(
			string weaponType,
			string weaponUid,
			string offhandType,
			string offhandUid)
		{
			WeaponType = weaponType;
			WeaponUid = weaponUid;
			OffhandType = offhandType;
			OffhandUid = offhandUid;
		}

		[DataMember]
		public readonly string WeaponType;

		[DataMember]
		public readonly string WeaponUid;

		[DataMember]
		public readonly string OffhandType;

		[DataMember]
		public readonly string OffhandUid;

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = StringComparer.CurrentCultureIgnoreCase.GetHashCode(WeaponType);
				hashCode = (hashCode * 397) ^ StringComparer.CurrentCultureIgnoreCase.GetHashCode(WeaponUid);
				hashCode = (hashCode * 397) ^ StringComparer.CurrentCultureIgnoreCase.GetHashCode(OffhandType);
				hashCode = (hashCode * 397) ^ StringComparer.CurrentCultureIgnoreCase.GetHashCode(OffhandUid);
				return hashCode;
			}
		}

		public bool Equals(WeaponSet other)
		{
			return
				string.Equals(WeaponType, other.WeaponType, StringComparison.CurrentCultureIgnoreCase) &&
				string.Equals(WeaponUid, other.WeaponUid, StringComparison.CurrentCultureIgnoreCase) &&
				string.Equals(OffhandType, other.OffhandType, StringComparison.CurrentCultureIgnoreCase) &&
				string.Equals(OffhandUid, other.OffhandUid, StringComparison.CurrentCultureIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			return obj is WeaponSet other && Equals(other);
		}
	}
}