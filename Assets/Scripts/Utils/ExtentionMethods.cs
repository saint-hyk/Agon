// -----------------------------------------------
// Filename: ExtentionMethods.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections.Generic;

namespace Utils {
   public static class ExtentionMethods {
      #region methods

      public static void AddInOrder<T>(this List<T> list, params T[] items) {
         foreach (T item in items) {
            list.Add(item);
         }
      }

      public static void AddInOrderWithOffset(this List<int> list, int offset, params int[] items) {
         foreach (int item in items) {
            list.Add(offset + item);
         }
      }

      #endregion methods
   }
}
