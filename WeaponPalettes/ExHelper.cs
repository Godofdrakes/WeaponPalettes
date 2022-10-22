using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WeaponPalettes
{
	public static class ExHelper
	{
		private class PluginException : Exception
		{
			public PluginException(string message, Exception innerException) : base(message, innerException) { }
		}

		[DebuggerHidden]
		public static Exception ArgumentNull(string param, [CallerFilePath] string? file = default, [CallerLineNumber] int? line = default)
		{
			return new PluginException($"{file}:{line}", new ArgumentNullException(param));
		}
	}
}