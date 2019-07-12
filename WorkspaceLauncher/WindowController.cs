using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using WorkspaceLauncher.Models;

namespace WorkspaceLauncher
{
	public static class WindowController
	{
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll", SetLastError = true)]
		static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WINDOWPLACEMENT
		{
			public int length;
			public int flags;
			public int showCmd;
			public System.Windows.Point ptMinPosition;
			public System.Windows.Point ptMaxPosition;
			public System.Windows.Shapes.Rectangle rcNormalPosition;
		}

		//Hide = 0,
		//Normal = 1,
		//Minimized = 2,
		//Maximized = 3,
		public static void PositionWindow(string processName, int xPos, int yPos, int wWidth, int wHeight, int status)
		{
			IntPtr prog;
			processName = processName.ToLower();
			Process[] ProcessList = Process.GetProcesses();
			foreach (Process Prcs in ProcessList)
			{
				if (!String.IsNullOrEmpty(Prcs.MainWindowTitle))
				{
					if (Prcs.ProcessName.ToLower().Contains(processName))
					{
						prog = Prcs.MainWindowHandle;
						if(status == 2)
						{
							MoveWindow(prog, xPos, yPos, wWidth, wHeight, true);
							ShowWindow(prog, status);
						}
						else
						{
							ShowWindow(prog, status);
							MoveWindow(prog, xPos, yPos, wWidth, wHeight, true);
						}
						WINDOWPLACEMENT Checker = new WINDOWPLACEMENT();
						GetWindowPlacement(prog, ref Checker);
					}
				}
			}
		}

		public static int WindowXPosition(Process proc)
		{
			RECT wnd;
			GetWindowRect(proc.MainWindowHandle, out wnd);
			return wnd.Left;
		}

		public static int WindowYPosition(Process proc)
		{
			RECT wnd;
			GetWindowRect(proc.MainWindowHandle, out wnd);
			return wnd.Top;
		}

		public static int GetWindowHeight(Process proc)
		{
			RECT wnd;
			GetWindowRect(proc.MainWindowHandle, out wnd);
			return wnd.Bottom - wnd.Top;
		}

		public static int GetWindowWidth(Process proc)
		{
			RECT wnd;
			GetWindowRect(proc.MainWindowHandle, out wnd);
			return wnd.Right - wnd.Left;
		}

		public static int GetWindowStatus(Process proc)
		{
			WINDOWPLACEMENT current = new WINDOWPLACEMENT();
			GetWindowPlacement(proc.MainWindowHandle, ref current);
			return current.showCmd;
		}

		public static IntPtr GetWindowHandleByCaption(string captionText)
		{
			Process[] processList = Process.GetProcesses();
			return processList.SingleOrDefault(x => x.MainWindowTitle.ToLower().Contains(captionText)).MainWindowHandle;
		}

		public static IntPtr GetWindowHandleByProcess(string processName)
		{
			processName = processName.ToLower();
			Process[] processList = Process.GetProcesses();
			return processList.SingleOrDefault(x => x.ProcessName.ToLower().Contains(processName)).MainWindowHandle;
		}

		public static IntPtr GetWindowHandleById(int id)
		{
			Process[] processList = Process.GetProcesses();
			return processList.SingleOrDefault(x => x.Id == id).MainWindowHandle;
		}

		public static void LaunchAll(List<WindowsProgram> programs)
		{
			foreach (WindowsProgram prog in programs)
			{
				try
				{
					if (!IsProcessOpen(prog.ProcessName))
					{
						string progPath = Path.GetDirectoryName(prog.StartPath);
						string progName = Path.GetFileName(prog.StartPath);
						string arguments = prog.Argument;
						ProcessStartInfo LaunchInfo = new ProcessStartInfo();
						LaunchInfo.WorkingDirectory = progPath;
						LaunchInfo.FileName = progName;
						LaunchInfo.Arguments = arguments;
						Process.Start(LaunchInfo);
					}
				}
				catch (InvalidOperationException)
				{

				}
			}
		}

		public static void PositionAll(List<WindowsProgram> programs)
		{
			foreach (WindowsProgram prog in programs)
			{
				try
				{
					PositionWindow(prog.ProcessName, prog.XPos, prog.YPos, prog.WindowWidth, prog.WindowHeight, prog.WindowState);
				}
				catch (InvalidOperationException)
				{

				}
			}
		}

		private static void WaitForLaunch(List<WindowsProgram> programs)
		{
			bool isOpen = false;
			foreach (WindowsProgram elmnt in programs)
			{
				while (!isOpen)
				{
					Process[] processList = Process.GetProcesses();
					foreach (Process proc in processList)
					{
						if (!String.IsNullOrEmpty(proc.MainWindowTitle))
						{
							if (proc.ProcessName == elmnt.ProcessName)
							{
								isOpen = true;
							}
						}
					}
				}
				isOpen = false;
				Thread.Sleep(100);
			}

		}

		public static void LaunchAndPositionAll(List<WindowsProgram> programs)
		{
			LaunchAll(programs);
			WaitForLaunch(programs);
			PositionAll(programs);
		}

		private static bool IsProcessOpen(string processName)
		{
			Process[] processList = Process.GetProcesses();
			foreach (Process proc in processList)
			{
				if (!String.IsNullOrEmpty(proc.MainWindowTitle))
				{
					if (proc.ProcessName == processName)
					{
						return true;
					}
				}
			}
			return false;

		}

	}
}
