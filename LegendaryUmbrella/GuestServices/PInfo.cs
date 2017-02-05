using System;
using System.Net;
using System.Collections;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Management;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

using HANDLE = System.IntPtr;
using System.Diagnostics;

namespace GuestServices
{

	public static class Utils
	{
		public const int NO_ERROR = 0;
		public const int MIB_TCP_STATE_CLOSED = 1;
		public const int MIB_TCP_STATE_LISTEN = 2;
		public const int MIB_TCP_STATE_SYN_SENT = 3;
		public const int MIB_TCP_STATE_SYN_RCVD = 4;
		public const int MIB_TCP_STATE_ESTAB = 5;
		public const int MIB_TCP_STATE_FIN_WAIT1 = 6;
		public const int MIB_TCP_STATE_FIN_WAIT2 = 7;
		public const int MIB_TCP_STATE_CLOSE_WAIT = 8;
		public const int MIB_TCP_STATE_CLOSING = 9;
		public const int MIB_TCP_STATE_LAST_ACK = 10;
		public const int MIB_TCP_STATE_TIME_WAIT = 11;
		public const int MIB_TCP_STATE_DELETE_TCB = 12;

		#region helper function

		const int MAXSIZE = 16384; // size _does_ matter
		public const int TOKEN_QUERY = 0X00000008;

		const int ERROR_NO_MORE_ITEMS = 259;

		enum TOKEN_INFORMATION_CLASS
		{
			TokenUser = 1,
			TokenGroups,
			TokenPrivileges,
			TokenOwner,
			TokenPrimaryGroup,
			TokenDefaultDacl,
			TokenSource,
			TokenType,
			TokenImpersonationLevel,
			TokenStatistics,
			TokenRestrictedSids,
			TokenSessionId
		}

		[StructLayout(LayoutKind.Sequential)]
		struct TOKEN_USER
		{
			public _SID_AND_ATTRIBUTES User;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct _SID_AND_ATTRIBUTES
		{
			public IntPtr Sid;
			public int Attributes;
		}

		[DllImport("advapi32")]
		static extern bool OpenProcessToken(
			HANDLE ProcessHandle, // handle to process
			int DesiredAccess, // desired access to process
			ref IntPtr TokenHandle // handle to open access token
		);

		[DllImport("kernel32")]
		static extern HANDLE GetCurrentProcess();

		[DllImport("advapi32", CharSet = CharSet.Auto)]
		static extern bool GetTokenInformation(
			HANDLE hToken,
			TOKEN_INFORMATION_CLASS tokenInfoClass,
			IntPtr TokenInformation,
			int tokeInfoLength,
			ref int reqLength
		);

		[DllImport("kernel32")]
		static extern bool CloseHandle(HANDLE handle);

		[DllImport("advapi32", CharSet = CharSet.Auto)]
		static extern bool LookupAccountSid
		(
			[In, MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, // name of local or remote computer
			IntPtr pSid, // security identifier
			StringBuilder Account, // account name buffer
			ref int cbName, // size of account name buffer
			StringBuilder DomainName, // domain name
			ref int cbDomainName, // size of domain name buffer
			ref int peUse // SID type
						  // ref _SID_NAME_USE peUse // SID type
		);

		[DllImport("advapi32", CharSet = CharSet.Auto)]
		static extern bool ConvertSidToStringSid(
			IntPtr pSID,
			[In, Out, MarshalAs(UnmanagedType.LPTStr)] ref string pStringSid
		);

		[DllImport("advapi32", CharSet = CharSet.Auto)]
		static extern bool ConvertStringSidToSid(
			[In, MarshalAs(UnmanagedType.LPTStr)] string pStringSid,
			ref IntPtr pSID
		);

		/// <summary>
		/// Collect User Info
		/// </summary>
		/// <param name="pToken">Process Handle</param>
		public static bool DumpUserInfo(HANDLE pToken, out IntPtr SID)
		{
			int Access = TOKEN_QUERY;
			HANDLE procToken = IntPtr.Zero;
			bool ret = false;
			SID = IntPtr.Zero;
			try
			{
				if (OpenProcessToken(pToken, Access, ref procToken))
				{
					ret = ProcessTokenToSid(procToken, out SID);
					CloseHandle(procToken);
				}
				return ret;
			}
			catch (Exception err)
			{
				log.Error("Method [" + new StackFrame(0).GetMethod().Name + "]. Error " + err.Message);
				return false;
			}
		}

		private static bool ProcessTokenToSid(HANDLE token, out IntPtr SID)
		{
			TOKEN_USER tokUser;
			const int bufLength = 256;
			IntPtr tu = Marshal.AllocHGlobal(bufLength);
			bool ret = false;
			SID = IntPtr.Zero;
			try
			{
				int cb = bufLength;
				ret = GetTokenInformation(token, TOKEN_INFORMATION_CLASS.TokenUser, tu, cb, ref cb);
				if (ret)
				{
					tokUser = (TOKEN_USER)Marshal.PtrToStructure(tu, typeof(TOKEN_USER));
					SID = tokUser.User.Sid;
				}
				return ret;
			}
			catch (Exception err)
			{
				log.Error("Method [" + new StackFrame(0).GetMethod().Name + "]. Error " + err.Message);
				return false;
			}
			finally
			{
				Marshal.FreeHGlobal(tu);
			}
		}

		public static string ExGetProcessInfoByPID(int PID, out string SID)//, out string OwnerSID)
		{
			IntPtr _SID = IntPtr.Zero;
			SID = String.Empty;
			try
			{
				Process process = Process.GetProcessById(PID);
				if (DumpUserInfo(process.Handle, out _SID))
				{
					ConvertSidToStringSid(_SID, ref SID);
				}
				return process.ProcessName;
			}
			catch
			{
				return "Unknown";
			}
		}

		private class log
		{
			internal static void Error(string v)
			{
				return;
			}
		}

		#endregion
	}
}
