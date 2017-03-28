using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace CoreCV
{
	public partial class ImgprocRoutine
    {
		private class NativeInvoker
		{
			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvCvtColor(IntPtr src, IntPtr dst, int code);
			//OPENCV_API void _CDECL coreCvCvtColor(void* src, void* dst, int code);
		}

		public static void cvtColor(Mat src, Mat dst, CvtColorCode code)
		{
			NativeInvoker.coreCvCvtColor(src.MatObj, dst.MatObj, (int)code);
		}
    }
}
