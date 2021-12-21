﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GTASaveData {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GTASaveData.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to The sequence length must be less than or equal to the length of the array..
        /// </summary>
        internal static string Error_Argument_SequenceTooBig {
            get {
                return ResourceManager.GetString("Error_Argument_SequenceTooBig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The index is out of range..
        /// </summary>
        internal static string Error_ArgumentOutOfRange_IndexOutOfRange {
            get {
                return ResourceManager.GetString("Error_ArgumentOutOfRange_IndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An attempt was made to read or write past the end of the buffer..
        /// </summary>
        internal static string Error_EndOfStream {
            get {
                return ResourceManager.GetString("Error_EndOfStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file is too large to be a valid Grand Theft Auto save file..
        /// </summary>
        internal static string Error_InvalidData_FileTooLarge {
            get {
                return ResourceManager.GetString("Error_InvalidData_FileTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You weren&apos;t supposed to be able to get here, you know..
        /// </summary>
        internal static string Error_InvalidOperation_Generic {
            get {
                return ResourceManager.GetString("Error_InvalidOperation_Generic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The collection cannot be modified while being changed by another thread..
        /// </summary>
        internal static string Error_InvalidOperation_NoCollectionReentrancy {
            get {
                return ResourceManager.GetString("Error_InvalidOperation_NoCollectionReentrancy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; is not deep clonable..
        /// </summary>
        internal static string Error_InvalidOperation_NotDeepClonable {
            get {
                return ResourceManager.GetString("Error_InvalidOperation_NotDeepClonable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Size not defined for type &apos;{0}&apos;..
        /// </summary>
        internal static string Error_InvalidOperation_SizeNotDefined {
            get {
                return ResourceManager.GetString("Error_InvalidOperation_SizeNotDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected end of stream reached while parsing JSON..
        /// </summary>
        internal static string Error_JsonSerialization_EndOfStream {
            get {
                return ResourceManager.GetString("Error_JsonSerialization_EndOfStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected token while parsing JSON: {0}.
        /// </summary>
        internal static string Error_JsonSerialization_UnexpectedToken {
            get {
                return ResourceManager.GetString("Error_JsonSerialization_UnexpectedToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path does not exist: {0}.
        /// </summary>
        internal static string Error_PathNotFound {
            get {
                return ResourceManager.GetString("Error_PathNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The block size exceeds the size of the buffer: {0}.
        /// </summary>
        internal static string Error_Serialization_BadBlockSize {
            get {
                return ResourceManager.GetString("Error_Serialization_BadBlockSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; cannot be pre-allocated..
        /// </summary>
        internal static string Error_Serialization_NoPreAlloc {
            get {
                return ResourceManager.GetString("Error_Serialization_NoPreAlloc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; does not support serialization..
        /// </summary>
        internal static string Error_Serialization_NotAllowed {
            get {
                return ResourceManager.GetString("Error_Serialization_NotAllowed", resourceCulture);
            }
        }
    }
}
