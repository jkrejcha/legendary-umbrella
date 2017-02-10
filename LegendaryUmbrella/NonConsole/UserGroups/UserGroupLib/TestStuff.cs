using System;
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
    }
}
