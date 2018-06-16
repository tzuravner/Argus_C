using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using System.Net;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace ConsoleApp1
{
	class Program
	{
		
		static void Main(string[] args)
		{
			TestContainer test = new TestContainer();

			ResultHandlder.Create();
			test.EnterIntoReposHub();						// open the github repo with the selenium search 

			for (int i = 0; i < 5; i++)						// number of page the explore
			{
				test.ExtractAllReposDetails();
				test.ClickOnNextPage();						// click on the next result page
			}
			test.Close();
		}
		
	}
		
}
