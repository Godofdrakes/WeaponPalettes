using System;
using System.Collections.Generic;

namespace WeaponPalettes
{
	public class WeaponSetMap
	{
		private Dictionary<WeaponSet, WeaponPalette> _weaponPalettes = new();

		public bool ShouldSave => _doNotSave > 0;
		
		private int _doNotSave;

		public Util.ScopedDestructor DoNotSave()
		{
			_doNotSave += 1;

			return new Util.ScopedDestructor(() => _doNotSave -= 1);
		}

		public WeaponPalette GetWeaponPalette(WeaponSet weaponSet)
		{
			throw new NotImplementedException();
		}

		public WeaponPalette GetOrAddWeaponPalette(WeaponSet weaponSet)
		{
			throw new NotImplementedException();
		}
	}
}