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

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvWarpAffine(IntPtr src, IntPtr dst, IntPtr trans, 
			                                           int outputW, int outputH, int flags, 
			                                           int borderMode, IntPtr borderValue);
			//OPENCV_API void _CDECL coreCvWarpAffine(void* src, void* dst, void* trans, int outputW, int outputH,
			//							  int flags, int borderMode, double* borderValue);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvResize(IntPtr src, IntPtr dst, int outputW, int outputH,
												   double fx, double fy, int interpolation);
			//OPENCV_API void _CDECL coreCvResize(void* src, void* dst, int outputW, int outputH,
			//									double fx, double fy, int interpolation);
		}

		public enum Interpolation : int
		{
			INTER_NN = 0,
			INTER_LINEAR = 1,
			INTER_CUBIC = 2,
			INTER_AREA = 3,
			INTER_LANCZOS4 = 4
		}

		private enum InverseMap : int
		{
			NOT_WARP_INVERSE_MAP = 0,
			WARP_INVERSE_MAP = 16
		}

		public enum BorderMode : int
		{
			BORDER_CONSTANT = 0,
			BORDER_REPLICATE = 1,
			BORDER_REFLECT = 2,
			BORDER_WRAP = 3,
			BORDER_REFLECT_101 = 4,
			BORDER_REFLECT101 = BORDER_REFLECT_101,
			BORDER_TRANSPARENT = 5,
			BORDER_DEFAULT = BORDER_REFLECT_101,
			// BORDER_ISOLATED = 16
		}

		public static void cvtColor(Mat src, Mat dst, CvtColorCode code)
		{
			NativeInvoker.coreCvCvtColor(src.MatObj, dst.MatObj, (int)code);
		}

		public static void resize(Mat src, Mat dst, Size dSize, 
			double fx = 0.0, double fy = 0.0, Interpolation inter = Interpolation.INTER_LINEAR)
		{
			NativeInvoker.coreCvResize(src.MatObj, dst.MatObj, (int)dSize.w, (int)dSize.h, fx, fy, (int)inter);
		}

		public static void warpAffine(Mat src, Mat dst, System.Numerics.Matrix3x2 trans, Size dSize,
			Interpolation inter = Interpolation.INTER_LINEAR, bool inverseMap = false, 
			BorderMode borderMode = BorderMode.BORDER_CONSTANT, double[] borderValue = null)
		{
			Mat transMat = trans;
			int flags = inverseMap ? ((int)InverseMap.WARP_INVERSE_MAP | (int)inter) : (int)inter;
			Mat matBorderValue = null;
			IntPtr ptrBorderValue = IntPtr.Zero;
			if (borderMode == BorderMode.BORDER_CONSTANT && borderValue != null && borderValue.Length > 0)
			{
				matBorderValue = new Mat(1, borderValue.Length, Mat.makeType(1, Mat.MatDepth.CV_64F));
				if (matBorderValue.Valid && matBorderValue.copyAll(borderValue, 0, borderValue.Length))
					ptrBorderValue = matBorderValue.Data;
			}
			NativeInvoker.coreCvWarpAffine(src.MatObj, dst.MatObj, transMat.MatObj, (int)dSize.w, (int)dSize.h,
				flags, (int)borderMode, ptrBorderValue);
		}
    }
}
