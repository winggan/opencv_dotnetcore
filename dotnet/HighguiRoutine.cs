using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace CoreCV
{
    public class HighguiRoutine
    {
		private class NativeInvoker
		{
			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvImread(string path, int flag, IntPtr matObj);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public int coreCvImwrite(string path, IntPtr matObj);
			//OPENCV_API void _CDECL coreCvImread(const char* path, int flag, void* mat);
			//OPENCV_API int/*bool*/ _CDECL coreCvImwrite(const char* path, void* mat);
		}


		public enum LOAD_IMAGE : int
		{
			/* 8bit, color or not */
			UNCHANGED = -1,
			/* 8bit, gray */
			GRAYSCALE = 0,
			/* ?, color */
			COLOR = 1,
			/* any depth, ? */
			ANYDEPTH = 2,
			/* ?, any color */
			ANYCOLOR = 4
		}

		public static Mat imread(string path, LOAD_IMAGE flag = LOAD_IMAGE.COLOR)
		{
			var ret = new Mat();
			if (path == null) return ret;
			NativeInvoker.coreCvImread(path, (int)flag, ret.MatObj); // check enum cast work
			return ret;
		}

		public static bool imwrite(string path, Mat img)
		{
			if (path == null || img == null) return false;
			return 0 == NativeInvoker.coreCvImwrite(path, img.MatObj) ? false : true;
		}
    }
}
