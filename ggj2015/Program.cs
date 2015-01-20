#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

#endregion

namespace ggj2015
{
#if WINDOWS || LINUX
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (!Debugger.IsAttached)
			{
				//Don't show the "You crashed" message
				NativeMethods.SetErrorMode(NativeMethods.SetErrorMode(0) | ErrorModes.SEM_NOGPFAULTERRORBOX | ErrorModes.SEM_FAILCRITICALERRORS | ErrorModes.SEM_NOOPENFILEERRORBOX);

				//Bring up a notepad doc of the exception if we crash
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			}


			using (var game = new Game1())
				game.Run();
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (File.Exists("crash.txt"))
				File.Delete("crash.txt");
			File.WriteAllText("crash.txt", e.ExceptionObject.ToString());
			Process.Start("crash.txt");
		}
	}

	[Flags]
	internal enum ErrorModes : uint
	{
		SYSTEM_DEFAULT = 0x0,
		SEM_FAILCRITICALERRORS = 0x0001,
		SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
		SEM_NOGPFAULTERRORBOX = 0x0002,
		SEM_NOOPENFILEERRORBOX = 0x8000
	}

	internal static class NativeMethods
	{
		[DllImport("kernel32.dll")]
		internal static extern ErrorModes SetErrorMode(ErrorModes mode);
	}
#endif
}
