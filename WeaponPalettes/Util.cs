using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

		[DebuggerHidden]
		public static PluginException PluginException(
			string message,
			[CallerFilePath] string file = "",
			[CallerLineNumber] int line = 0)
		{
			return new PluginException($"[{file}:{line}] {message}");
		}

		[DebuggerHidden]
		public static PluginException NullParam(
			string paramName,
			[CallerFilePath] string file = "",
			[CallerLineNumber] int line = 0)
		{
			return new PluginException($"[{file}:{line}]", new ArgumentNullException(paramName));
		}

		[DebuggerHidden]
		public static PluginException InvalidOperation(
			[CallerFilePath] string file = "",
			[CallerLineNumber] int line = 0)
		{
			return new PluginException($"[{file}:{line}]", new InvalidOperationException());
		}
	}
}