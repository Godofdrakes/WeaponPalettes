using System;

namespace WeaponPalettes
{
	public static class Util
	{
		public static bool IsLocalPlayer(Character character) => character != null && character.IsLocalPlayer;
		
		public class ScopedDestructor : IDisposable
		{
			private readonly Action _onDispose;

			public ScopedDestructor(Action onDispose)
			{
				_onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
			}
			
			public void Dispose() => _onDispose.Invoke();
		}
	}
}