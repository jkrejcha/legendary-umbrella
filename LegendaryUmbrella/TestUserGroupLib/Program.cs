using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendaryUmbrella.UserGroupLib;
using System.DirectoryServices.AccountManagement;
using System.Threading;
using System.ComponentModel;

namespace LegendaryUmbrella.TestUserGroupLib
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WindowWidth = 112;
			Console.WindowHeight = 25;
			foreach (Principal p in TestStuff.Groups)
			{
				PrintInfo(p);
			}
			foreach (Principal p in TestStuff.Users)
			{
				PrintInfo(p);
			}
			while(true) Thread.Sleep(Int32.MaxValue);
			/*a:
			Console.Write("User group to create?>");
			String r = Console.ReadLine();
			try
			{
				UserGroupLib.TestStuff.CreateLocalgroup(r);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			goto a;*/
		}

		private static void PrintInfo(Principal p)
		{
			if (p is UserPrincipal)
			{
				UserPrincipal u = (UserPrincipal)p;
				foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(u))
				{
					string name = descriptor.Name;
					object value = descriptor.GetValue(u);
					Console.WriteLine("{0}={1}", name, value);
				}
				Console.WriteLine("Member of: " + ListNamesAsString(p.GetGroups()));
			}
			/*Console.WriteLine("Name: " + p.Name);
			Console.WriteLine("SID: " + p.Sid);
			Console.WriteLine("Description: " + p.Description);
			Console.WriteLine("Member of: " + ListNamesAsString(p.GetGroups()));
			if (p is UserPrincipal)
			{
				UserPrincipal u = (UserPrincipal)p;
				u.
			}*/
			Console.WriteLine();
		}

		private static string ListNamesAsString(PrincipalSearchResult<Principal> principalSearchResult)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Principal p in principalSearchResult)
			{
				sb.Append(p.Name + ", ");
			}
			return sb.ToString();
		}
	}
}
