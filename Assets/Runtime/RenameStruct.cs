#if UNITY_EDITOR
using System;

/*
 * VRSuya Core
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 */

namespace VRSuya.Core {

	public class RenameStruct {

		[Serializable]
		public struct RenameExpression {
			public string Before;
			public string After;

			public RenameExpression(string BeforeWord, string AfterWord) {
				Before = BeforeWord;
				After = AfterWord;
			}
		};
	}
}
#endif