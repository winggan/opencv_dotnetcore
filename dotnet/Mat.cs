using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace CoreCV
{
	public class Mat : IDisposable
	{
		private IntPtr matObj = IntPtr.Zero;

		private class NativeInvoker
		{
			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public IntPtr coreCvCreateMat();

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvdestroyMat(IntPtr matObj);

			//OPENCV_API void* _CDECL coreCvCreateMat();
			//OPENCV_API void _CDECL coreCvdestroyMat(void* mat);
			//
			////rest of the funtions assume pointer "mat" is valid for efficiency
			//
			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public int coreCvGetCols(IntPtr matObj);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public int coreCvGetRows(IntPtr matObj);
			//OPENCV_API int _CDECL coreCvGetCols(void* mat);
			//OPENCV_API int _CDECL coreCvGetRows(void* mat);
			//

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public int coreCvGetType(IntPtr matObj);
			//OPENCV_API int _CDECL coreCvGetType(void* mat);
			//

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public IntPtr coreCvGetRow(IntPtr matObj, int row);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public IntPtr coreCvGetData(IntPtr matObj);
			//OPENCV_API void* _CDECL coreCvGetRow(void* mat, int row);
			//
			//// return null if mat is not continuous
			//OPENCV_API void* _CDECL coreCvGetData(void* mat);
			//

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public int coreCvIsContinuous(IntPtr matObj);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public int coreCvGetElementSize(IntPtr matObj);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public int corCvIsEmpty(IntPtr matObj);
			//OPENCV_API int/*bool*/ _CDECL coreCvIsContinuous(void* mat);
			//OPENCV_API int _CDECL coreCvGetElementSize(void* mat);
			//OPENCV_API int/*bool*/ _CDECL corCvIsEmpty(void* mat);
		}

		public int Rows { get {
				return NativeInvoker.coreCvGetRows(matObj);
			} }
		public int Cols { get {
				return NativeInvoker.coreCvGetCols(matObj);
			} }

		public IntPtr MatObj { get {
				return matObj;
			} }

		public Mat()
		{
			matObj = NativeInvoker.coreCvCreateMat();
		}

		~Mat()
		{
			Dispose();
		}

		public void Dispose()
		{
			NativeInvoker.coreCvdestroyMat(matObj);
			matObj = IntPtr.Zero;
		}
	}
}
