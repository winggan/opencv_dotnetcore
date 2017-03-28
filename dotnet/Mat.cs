using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace CoreCV
{
	public class Mat : IDisposable
	{
		private static readonly List<Type> typeList = initTypeList();
		private static List<Type> initTypeList()
		{
			var ret = new List<Type>();
			ret.Add(typeof(byte));
			ret.Add(typeof(sbyte));
			ret.Add(typeof(UInt16));
			ret.Add(typeof(Int16));
			ret.Add(typeof(Int32));
			ret.Add(typeof(float));
			ret.Add(typeof(double));
			return ret;
		}

		// constants
		public const int CV_CN_MAX = 512;
		public const int CV_CN_SHIFT = 3;
		public const int CV_DEPTH_MAX = (1 << CV_CN_SHIFT);
		public const int CV_MAT_DEPTH_MASK = CV_DEPTH_MAX - 1;
		public const int CV_CN_MASK = CV_CN_MAX - 1;

		public static int makeType(int channels, MatDepth depth) => (((channels - 1) & CV_CN_MASK) << CV_CN_SHIFT) | (int)depth;

		private IntPtr matObj = IntPtr.Zero;

		private class NativeInvoker
		{
			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public IntPtr coreCvCreateEmptyMat();

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public IntPtr coreCvCreateMat(int rows, int cols, int type);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvdestroyMat(IntPtr matObj);

			//OPENCV_API void* _CDECL coreCvCreateEmptyMat();
			//OPENCV_API void* _CDECL coreCvCreateMat(int rows, int cols, int type);
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
			extern static public int coreCvIsEmpty(IntPtr matObj);
			//OPENCV_API int/*bool*/ _CDECL coreCvIsContinuous(void* mat);
			//OPENCV_API int _CDECL coreCvGetElementSize(void* mat);
			//OPENCV_API int/*bool*/ _CDECL coreCvIsEmpty(void* mat);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvCloneMat(IntPtr src, IntPtr dst);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvConvertMat(IntPtr src, IntPtr dst, int dstType, double alpha, double beta);
			//OPENCV_API void _CDECL coreCvCloneMat(void *src, void *dst);
			//OPENCV_API void _CDECL coreCvConvertMat(void* src, void* dst, int dstType, double alpha, double beta);
		}

		public int Rows => IntPtr.Zero != matObj ? NativeInvoker.coreCvGetRows(matObj) : -1;
		public int Cols => IntPtr.Zero != matObj ? NativeInvoker.coreCvGetCols(matObj) : -1;
		public int Type => IntPtr.Zero != matObj ? NativeInvoker.coreCvGetType(matObj) : -1;
		public IntPtr getRow(int row) => IntPtr.Zero != matObj ? NativeInvoker.coreCvGetRow(matObj, row) : IntPtr.Zero;
		public IntPtr Data => IntPtr.Zero != matObj ? NativeInvoker.coreCvGetData(matObj) : IntPtr.Zero;
		public bool IsContinuous => IntPtr.Zero != matObj ? (NativeInvoker.coreCvIsContinuous(matObj) != 0) : false;
		public bool IsEmpty => IntPtr.Zero != matObj ? (NativeInvoker.coreCvIsEmpty(matObj) != 0) : false;
		public int ElementSize => IntPtr.Zero != matObj ? NativeInvoker.coreCvGetElementSize(matObj) : -1;
		public int Channels => ((Type >> CV_CN_SHIFT) & CV_CN_MASK) + 1;
		public MatDepth Depth => (MatDepth)(Type & CV_MAT_DEPTH_MASK);
		public string TypeString => new StringBuilder(Depth.ToString()).Append("C").Append(Channels.ToString()).ToString();
		public bool Valid => IntPtr.Zero != matObj;

		public enum MatDepth : int
		{
			CV_8U = 0,
			CV_8S = 1,
			CV_16U = 2,
			CV_16S = 3,
			CV_32S = 4,
			CV_32F = 5,
			CV_64F = 6,
			CV_USRTYPE1 = 7
		}


		internal IntPtr MatObj { get {
				return matObj;
			} }

		public Mat()
		{
			matObj = NativeInvoker.coreCvCreateEmptyMat();
		}

		public Mat(int rows, int cols, int type)
		{
			matObj = NativeInvoker.coreCvCreateMat(rows, cols, type);
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

		public Mat clone()
		{
			Mat ret = new Mat();
			if (!ret.Valid) return null;
			NativeInvoker.coreCvCloneMat(MatObj, ret.MatObj);
			return ret;
		}

		public void convertTo(Mat dst, int type, double alpha = 1.0, double beta = 0.0)
		{
			if (dst == null || !dst.Valid)
				return;
			NativeInvoker.coreCvConvertMat(MatObj, dst.MatObj, type, alpha, beta);
		}

		public Mat convertTo(int type, double alpha = 1.0, double beta = 0.0)
		{
			Mat ret = new Mat();
			if (!ret.Valid) return null;
			convertTo(ret, type, alpha, beta);
			return ret;
		}

		//return number of elements copied
		public int copyRow<T>(int row, T[] dst, int startIndex) where T : struct
		{
			if (!Valid || IsEmpty)
				return 0;

			if (dst.Length - startIndex < Cols * ElementSize / Marshal.SizeOf<T>())
				return 0;

			var inputType = typeof(T);
			if (inputType == typeList[(int)MatDepth.CV_8U])
			{
				int count = Cols * ElementSize;
				Marshal.Copy(getRow(row), dst as byte[], startIndex, count);
				return count;
			}
			else if (inputType == typeList[(int)Depth])
			{
				int count = Cols * ElementSize / Marshal.SizeOf<T>();
				switch (Depth)
				{
					case MatDepth.CV_8S:
						Marshal.Copy(getRow(row), dst as Array as byte[], startIndex, count);
						break;
					case MatDepth.CV_16U:
					case MatDepth.CV_16S:
						Marshal.Copy(getRow(row), dst as Array as char[], startIndex, count);
						break;
					case MatDepth.CV_32S:
						Marshal.Copy(getRow(row), dst as Int32[], startIndex, count);
						break;
					case MatDepth.CV_32F:
						Marshal.Copy(getRow(row), dst as float[], startIndex, count);
						break;
					case MatDepth.CV_64F:
						Marshal.Copy(getRow(row), dst as double[], startIndex, count);
						break;
					default:
						return 0;
				}
				return count;
			}
			else
				return 0;
		}

		// return true when copy succeed
		public bool copyRow<T>(int row, T[] src, int startIndex, int count) where T : struct
		{
			if (!Valid || IsEmpty)
				return false;

			if (count + startIndex > src.Length)
				return false;

			if (count > Cols * ElementSize / Marshal.SizeOf<T>())
				return false;

			var inputType = typeof(T);
			if (inputType == typeList[(int)MatDepth.CV_8U])
			{
				Marshal.Copy(src as byte[], startIndex, getRow(row), count);
				return true;
			}
			else if (inputType == typeList[(int)Depth])
			{
				switch (Depth)
				{
					case MatDepth.CV_8S:
						Marshal.Copy(src as Array as byte[], startIndex, getRow(row), count);
						break;
					case MatDepth.CV_16U:
					case MatDepth.CV_16S:
						Marshal.Copy(src as Array as char[], startIndex, getRow(row), count);
						break;
					case MatDepth.CV_32S:
						Marshal.Copy(src as Int32[], startIndex, getRow(row), count);
						break;
					case MatDepth.CV_32F:
						Marshal.Copy(src as float[], startIndex, getRow(row), count);
						break;
					case MatDepth.CV_64F:
						Marshal.Copy(src as double[], startIndex, getRow(row), count);
						break;
					default:
						return false;
				}
				return true;
			}
			else
				return false;
		}

		// return number of elements copied
		public int copyAll<T> (T[] dst, int startIndex) where T: struct
		{
			if (!Valid || IsEmpty)
				return 0;

			if (dst.Length - startIndex < Rows * Cols * ElementSize / Marshal.SizeOf<T>())
				return 0;

			if (!IsContinuous)
			{
				int count = 0;
				int rows = Rows;
				for (int i = 0; i < rows; i++)
					count += copyRow<T>(dst, startIndex + count);

				return count;
			}

			var inputType = typeof(T);
			if (inputType == typeList[(int)MatDepth.CV_8U])
			{
				int count = Rows * Cols * ElementSize;
				Marshal.Copy(Data, dst as byte[], startIndex, count);
				return count;
			}
			else if (inputType == typeList[(int)Depth])
			{
				int count = Rows * Cols * ElementSize / Marshal.SizeOf<T>();
				switch (Depth)
				{
					case MatDepth.CV_8S:
						Marshal.Copy(Data, dst as Array as byte[], startIndex, count);
						break;
					case MatDepth.CV_16U:
					case MatDepth.CV_16S:
						Marshal.Copy(Data, dst as Array as char[], startIndex, count);
						break;
					case MatDepth.CV_32S:
						Marshal.Copy(Data, dst as Int32[], startIndex, count);
						break;
					case MatDepth.CV_32F:
						Marshal.Copy(Data, dst as float[], startIndex, count);
						break;
					case MatDepth.CV_64F:
						Marshal.Copy(Data, dst as double[], startIndex, count);
						break;
					default:
						return 0;
				}
				return count;
			}
			else
				return 0;
		}

		// return true when copy succeed
		public bool copyAll<T>(T[] src, int startIndex, int count) where T : struct
		{
			if (!Valid || IsEmpty)
				return false;

			if (count + startIndex > src.Length)
				return false;

			if (count > Rows * Cols * ElementSize / Marshal.SizeOf<T>())
				return false;

			if (!IsContinuous)
			{
				var ret = true;
				int rows = Rows;
				int step = Cols * ElementSize / Marshal.SizeOf<T>();
				for (int i = 0; i < rows && ret; i++)
					ret = ret && copyRow<T>(src, startIndex + i * step, step);

				return ret;
			}

			var inputType = typeof(T);
			if (inputType == typeList[(int)MatDepth.CV_8U])
			{
				Marshal.Copy(src as byte[], startIndex, Data, count);
				return true;
			}
			else if (inputType == typeList[(int)Depth])
			{
				switch (Depth)
				{
					case MatDepth.CV_8S:
						Marshal.Copy(src as Array as byte[], startIndex, Data, count);
						break;
					case MatDepth.CV_16U:
					case MatDepth.CV_16S:
						Marshal.Copy(src as Array as char[], startIndex, Data, count);
						break;
					case MatDepth.CV_32S:
						Marshal.Copy(src as Int32[], startIndex, Data, count);
						break;
					case MatDepth.CV_32F:
						Marshal.Copy(src as float[], startIndex, Data, count);
						break;
					case MatDepth.CV_64F:
						Marshal.Copy(src as double[], startIndex, Data, count);
						break;
					default:
						return false;
				}
				return true;
			}
			else
				return false;
		}
	}
}
