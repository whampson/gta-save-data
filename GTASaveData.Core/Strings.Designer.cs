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
        ///   Looks up a localized string similar to The value must be non-negative..
        /// </summary>
        internal static string Error_Argument_NoNegative {
            get {
                return ResourceManager.GetString("Error_Argument_NoNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The character cannot be a surrogate..
        /// </summary>
        internal static string Error_Argument_NoSurrogateChars {
            get {
                return ResourceManager.GetString("Error_Argument_NoSurrogateChars", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The index must be smaller than the length of the sequence..
        /// </summary>
        internal static string Error_Argument_SequenceIndexOutOfRange {
            get {
                return ResourceManager.GetString("Error_Argument_SequenceIndexOutOfRange", resourceCulture);
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
        ///   Looks up a localized string similar to The collection cannot be modified while being changed by another thread..
        /// </summary>
        internal static string Error_InvalidOperation_CollectionReentrancy {
            get {
                return ResourceManager.GetString("Error_InvalidOperation_CollectionReentrancy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid padding type..
        /// </summary>
        internal static string Error_InvalidOperation_PaddingType {
            get {
                return ResourceManager.GetString("Error_InvalidOperation_PaddingType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; does not support serialization..
        /// </summary>
        internal static string Error_InvalidOperation_Serialization {
            get {
                return ResourceManager.GetString("Error_InvalidOperation_Serialization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected end while parsing JSON..
        /// </summary>
        internal static string Error_JsonSerialization_BinaryUnexpectedEnd {
            get {
                return ResourceManager.GetString("Error_JsonSerialization_BinaryUnexpectedEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected token while parsing JSON binary: {0}..
        /// </summary>
        internal static string Error_JsonSerialization_BinaryUnexpectedToken {
            get {
                return ResourceManager.GetString("Error_JsonSerialization_BinaryUnexpectedToken", resourceCulture);
            }
        }
    }
}
