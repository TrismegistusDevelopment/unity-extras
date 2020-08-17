#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Trismegistus.Core.Tools {
	/// <summary>
	/// Call this to change Scripting Define Symbols (can be found in Players Settings - Other Settings) in editor mode
	/// <example>
	/// </example>
	/// </summary>
	public static class ScriptingDefineSymbolsTool {
		/// <summary>
		/// Check if #DEFINE symbol exists
		/// </summary>
		/// <param name="symbol"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		public static bool HasSymbol(string symbol, BuildTargetGroup group = default) {
			return GetSymbols(group).Contains(symbol);
		}

		/// <summary>
		/// Add or remove #DEFINE symbol from Scripting Define Symbols
		/// <example>
		/// <code>
		/// [UnityEditor.Callbacks.DidReloadScripts]
		/// private static void OnScriptsReloaded() {
		///	SetSymbol("CUSTOM_SYMBOL", true); }
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="symbol"></param>
		/// <param name="mode"></param>
		/// <param name="group"></param>
		/// <returns>Changes have been made</returns>
		public static bool SetSymbol(string symbol, bool mode, BuildTargetGroup group = default) {
			if (group == default) group = EditorUserBuildSettings.selectedBuildTargetGroup;

			var hasSymbol = HasSymbol(symbol, group);

			if (mode == hasSymbol) return false;
			var symbols = GetSymbols(group).ToList();

			if (mode) {
				symbols.Add(symbol);
				Debug.Log($"Added {symbol} define for {group}");
			}
			else {
				var removed = symbols.RemoveAll(x => x.Equals(symbol));
				Debug.Log($"Removed {removed} occurrences of {symbol} define for {group}");
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", symbols));

			return true;
		}

		/// <summary>
		/// <example>
		/// <code>
		/// [UnityEditor.Callbacks.DidReloadScripts]
		/// private static void CheckOculus() {
		/// var isOculus = DefineClass("OVRManager", "OCULUS_SDK");}
		/// </code>
		/// </example> 
		/// </summary>
		/// <param name="className"></param>
		/// <param name="symbol"></param>
		/// <param name="group"></param>
		/// <returns>Class exists</returns>
		public static bool DefineClass(string className, string symbol, BuildTargetGroup group = default) {
			var isTypeExist = Type.GetType(className) == null;
			SetSymbol(symbol, isTypeExist, group);
			return isTypeExist;
		}

		private static string[] GetSymbols(BuildTargetGroup group = default) {
			if (group == default) group = EditorUserBuildSettings.selectedBuildTargetGroup;
			return PlayerSettings.GetScriptingDefineSymbolsForGroup(@group).Split(';');
		}
	}
}


#endif