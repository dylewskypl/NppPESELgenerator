using System;
using System.Linq;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
	class Main
	{
		internal const string PluginName = "NppPESELgenerator";
		static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

		public static void OnNotification(ScNotification notification)
		{
			// This method is invoked whenever something is happening in notepad++
			// use eg. as
			// if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
			// { ... }
			// or
			//
			// if (notification.Header.Code == (uint)SciMsg.SCNxxx)
			// { ... }
		}

		internal static void CommandMenuInit()
		{
			PluginBase.SetCommand(0, "GeneratePESEL", myMenuFunction, new ShortcutKey(true, true, false, Keys.P));
		}

		internal static void myMenuFunction()
		{
			Random random = new Random();
			DateTime start = new DateTime(1901, 1, 1);
			int rage = (DateTime.Today - start).Days;
			DateTime birthDate = start.AddDays(random.Next(rage));

			string pesel = string.Empty;
			pesel += string.Join(string.Empty, birthDate.Year.ToString().Skip(2));

			if (birthDate.Year > 1999)
			{
				pesel += birthDate.Month + 20;
			}
			else
			{
				pesel += birthDate.Month.ToString("00");
			}

			pesel += birthDate.Day.ToString("00");
			pesel += random.Next(10);
			pesel += random.Next(10);
			pesel += random.Next(10);
			pesel += random.Next(10);

			try
			{
				ulong controlSum = 9 * ulong.Parse(pesel[0].ToString()) +
						   7 * ulong.Parse(pesel[1].ToString()) +
						   3 * ulong.Parse(pesel[2].ToString()) +
						   1 * ulong.Parse(pesel[3].ToString()) +
						   9 * ulong.Parse(pesel[4].ToString()) +
						   7 * ulong.Parse(pesel[5].ToString()) +
						   3 * ulong.Parse(pesel[6].ToString()) +
						   1 * ulong.Parse(pesel[7].ToString()) +
						   9 * ulong.Parse(pesel[8].ToString()) +
						   7 * ulong.Parse(pesel[9].ToString());
				pesel += (controlSum % 10).ToString().Last();
			}
			catch (Exception e)
			{
				editor.SetText(e.Message);
			}

			if (editor.GetSelectionEnd().Value - editor.GetSelectionStart().Value > 0)
			{
				editor.ReplaceSel(pesel);
			}
			else
			{
				editor.AppendText(11, pesel);
			}
		}
	}
}
