using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace LegendaryUmbrella.UserGroupLib
{
    public static class TestStuff
    {
		private static PrincipalContext context = new PrincipalContext(ContextType.Machine);
		private static PrincipalSearcher searcher = new PrincipalSearcher();


		public static PrincipalSearchResult<Principal> Groups
		{
			get
			{
				return SearchForAll(new GroupPrincipal(context));
			}
		}

		public static PrincipalSearchResult<Principal> Users
		{
			get
			{
				return SearchForAll(new UserPrincipal(context));
			}
		}

		private static PrincipalSearchResult<Principal> SearchForAll(Principal p)
		{
			searcher.QueryFilter = p;
			return searcher.FindAll();
        }
		public static void CreateLocalgroup(String name)
		{
			PrincipalContext context = new PrincipalContext(ContextType.Machine);
			GroupPrincipal group = new GroupPrincipal(context);
			group.Name = name;
			group.Save();
		}

		public static List<DirectoryEntry> GetAllComputersInNetwork()
		{
			List<DirectoryEntry> computerList = new List<DirectoryEntry>();
			DirectoryEntry root = new DirectoryEntry("WinNT:");
			foreach (DirectoryEntry computers in root.Children)
			{
				foreach (DirectoryEntry computer in computers.Children)
				{
					if (computer.Name != "Schema" && computer.SchemaClassName == "Computer")
					{
						computerList.Add(computer);
					}
				}
			}
			return computerList;
		}

		public static List<String> GetAllComputerNames()
		{
			List<String> list = new List<String>();
			foreach (DirectoryEntry entry in GetAllComputersInNetwork())
			{
				list.Add(entry.Name);
			}
			return list;
		}

		public static void PrintShares()
		{
			Console.WriteLine("Local shares: ");
			PrintAllShares(ShareCollection.LocalShares);
			Console.WriteLine();
			Console.WriteLine("Non-local shares:");
			foreach (DirectoryEntry computer in GetAllComputersInNetwork())
			{
				PrintAllShares(ShareCollection.GetShares(computer.Name));
			}
		}

		static void PrintAllShares(ShareCollection shares)
		{
			foreach (Share share in shares)
			{
				if (!share.IsFileSystem) continue;
				Console.WriteLine(share.ToString());
			}
		}
	}
}
