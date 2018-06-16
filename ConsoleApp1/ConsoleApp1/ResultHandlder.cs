using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResultHandlder
{
	public static void Create()
	{
		File.Delete("ResultRepos.csv");
		File.AppendAllLines("ResultRepos.csv", new[] { "Title,Description,Tags,Time,Lang,Star,IsValid" });
		File.Delete("ResultTiming.txt");
	}

	public static void WriteResult(string Title, string Description, string Tags, string Time, string Lang, string Star, bool IsValid)
	{
		Title = Title.Replace(',', ' ');
		Description = Description.Replace(',', ' ');
		Tags = Tags.Replace(',', ' ');
		Time = Time.Replace(',', ' ');
		Lang = Lang.Replace(',', ' ');
		Star = Star.Replace(',', ' ');
		File.AppendAllLines("ResultRepos.csv", new[] { Title + "," + Description + "," + Tags + "," + Time + "," + Lang + "," + Star + "," + IsValid });
	}

	public static void WriteResultTiming(string Title, string timing)
	{

		File.AppendAllLines("ResultTiming.txt", new[] { Title + "(MS): " + timing });
	}
}