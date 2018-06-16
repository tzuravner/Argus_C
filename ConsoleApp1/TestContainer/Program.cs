using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestContainer
{
	public class TestContainer
	{

		IWebDriver Driver;

		public TestContainer()
		{
			var options = new ChromeOptions();
			Driver = new ChromeDriver(options);
			Driver.Manage().Window.Maximize();
			Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

		}

		public void RunTest()
		{
			ResultHandlder.Create();
			EnterIntoReposHub();

			for (int i = 0; i < 5; i++)
			{
				ExtractAllReposDetails();
				ClickOnNextPage();
			}
		}

		public void EnterIntoReposHub()
		{
			Actions builder = new Actions(Driver);

			Driver.Navigate().GoToUrl(@"https://github.com/");
			IWebElement Searchbox = Driver.FindElement(By.XPath(@"/html/body/div[1]/header/div/div[2]/div/div/div/div/form/label/input[1]"));
			Searchbox.Click();
			Searchbox.SendKeys("selenium");
			Searchbox.SendKeys(Keys.Enter);

			var watch = System.Diagnostics.Stopwatch.StartNew();
			builder.SendKeys(Keys.Enter);
			watch.Stop();

			WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
			wait.Until(d => d.FindElements(By.CssSelector(".repo-list-item")).Count > 5);

			//var elapsedMs = watch.ElapsedMilliseconds;
			ResultHandlder.WriteResultTiming("Search result", watch.ElapsedMilliseconds.ToString());
		}

		public void ExtractAllReposDetails()
		{

			Driver.Navigate().Refresh();
			IList<IWebElement> Repos = Driver.FindElements(By.CssSelector(".repo-list-item"));
			foreach (IWebElement IW in Repos)
			{

				string Title = IW.FindElement(By.CssSelector(@".v-align-middle")).Text;

				string URL = IW.FindElement(By.CssSelector(@".v-align-middle")).GetAttribute("href");
				bool IsValidLink = RemoteLinkExists(URL);

				string Description;
				try
				{
					Description = IW.FindElement(By.CssSelector(@".pr-4")).Text;
				}
				catch
				{
					Description = "None";
				}

				IList<IWebElement> Tags = IW.FindElements(By.CssSelector(@".topic-tag"));
				string TagsResut = "";
				if (Tags.Count == 0)
					TagsResut = "None";
				else
				{
					try
					{
						foreach (IWebElement t in Tags)
						{
							string Tag = t.Text;
							TagsResut = TagsResut + " | " + Tag;

						}
						TagsResut = TagsResut + " | ";
					}
					catch
					{
						TagsResut = "None";
					}
				}

				string Time = IW.FindElement(By.CssSelector(@"relative-time")).Text;
				string Lang = IW.FindElement(By.CssSelector(@".d-table-cell")).Text;
				string Star = IW.FindElement(By.CssSelector(@".muted-link:not(.mt-2)")).Text;

				ResultHandlder.WriteResult(Title, Description, TagsResut, Time, Lang, Star, IsValidLink);
			}
		}

		private bool RemoteLinkExists(string url)
		{

			try
			{
				Uri uri = new Uri(url);
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				WebRequest http = HttpWebRequest.Create(url);
				HttpWebResponse response = (HttpWebResponse)http.GetResponse();
				Stream stream = response.GetResponseStream();
				return true;
			}
			catch (UriFormatException e)
			{
				return false;
			}
			catch (IOException e)
			{
				return false;
			}

		}

		public void ClickOnNextPage()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();


			Driver.FindElement(By.CssSelector(@":not(.next_page)[rel='next']")).Click();

			WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
			wait.Until(d => d.FindElements(By.CssSelector(".repo-list-item")).Count > 5);

			watch.Stop();
			//var elapsedMs = watch.ElapsedMilliseconds;
			ResultHandlder.WriteResultTiming("Next Page Timing", watch.ElapsedMilliseconds.ToString());
		}

		public void Close()
		{
			Driver.Quit();
			Driver.Dispose();

		}
	}
}
