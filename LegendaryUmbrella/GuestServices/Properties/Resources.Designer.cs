﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GuestServices.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GuestServices.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Restrictions have been temporarily disabled..
        /// </summary>
        internal static string RestrictionsDisabledMsg {
            get {
                return ResourceManager.GetString("RestrictionsDisabledMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This operation has been canceled due to restrictions in effect on this computer. Please contact your system administrator..
        /// </summary>
        internal static string RestrictionsMsg {
            get {
                return ResourceManager.GetString("RestrictionsMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error has occurred while attempting to log on:
        ///%0
        ///
        ///Press &apos;OK&apos; to retry logging on..
        /// </summary>
        internal static string ShellLaunchFailMsg {
            get {
                return ResourceManager.GetString("ShellLaunchFailMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user account is provided as a convienence to you. There are some things to note that should be obvious, but will be noted anyway.
        ///
        ///Please do not:
        ///
        ///* Mess with the settings, personalization, or otherwise. This includes changing the background or defaults.
        ///* Download and install applications.
        ///* Do anything malicious.
        ///* Do anything illegal.
        ///* Remove or modify this note.
        ///* Attempt to bypass or bypass restrictions in effect on this user account.
        ///
        ///Also, please be aware that this is a public user a [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WarningMsg {
            get {
                return ResourceManager.GetString("WarningMsg", resourceCulture);
            }
        }
    }
}
