using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace CoreCV
{
    public class CoreRoutine
    {
		private class NativeInvoker
		{
			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvSplit(IntPtr src, IntPtr dsts, int maxChannel);

			[DllImport(Platform.NativeLibName, CallingConvention = CallingConvention.Cdecl)]
			extern static public void coreCvMerge(IntPtr srcs, int n, IntPtr dsts);

			//OPENCV_API void _CDECL coreCvSplit(void* src, void* dsts, int maxChannel);
			//OPENCV_API void _CDECL coreCvMerge(void* srcs, int n, void* dst);
		}

		public static void split(Mat src, List<Mat> dst)
		{
			if (!src.Valid || src.IsEmpty)
				return;
			int channel = src.Channels;

			if (dst == null)
				dst = new List<Mat>();
			if (channel > dst.Count)
				while (dst.Count < channel) dst.Add(new Mat());
			else if (channel < dst.Count)
				dst.RemoveRange(channel, dst.Count - channel);

			var dstPtrs = new IntPtr[channel];
			for (int i = 0; i < channel; i++)
				if (dst[i].Valid)
					dstPtrs[i] = dst[i].MatObj;
				else
					return; // check every mat in dst is valid
			
			var dstPtrMat = new Mat(1, channel * Marshal.SizeOf<IntPtr>(), Mat.makeType(1, Mat.MatDepth.CV_8U));
			if (!dstPtrMat.Valid) // alloc unmanaged memory
				return;
			Marshal.Copy(dstPtrs, 0, dstPtrMat.Data, channel);
			NativeInvoker.coreCvSplit(src.MatObj, dstPtrMat.Data, channel);
		}

		public static void merge(List<Mat> src, Mat dst)
		{
			if (src == null || src.Count == 0)
				return;

			if (!src[0].Valid || src[0].IsEmpty)
				return;

			if (!dst.Valid)
				return;

			int rows = src[0].Rows, cols = src[0].Cols;

			int dstChannels = src[0].Channels;
			int n = src.Count; // src[i] may have more than 1 channel
			var srcPtrs = new IntPtr[n];
			srcPtrs[0] = src[0].MatObj;

			for (int i = 1; i < n; i++) // check valid, size, and sumation over channel
				if (src[i].Valid && src[i].Rows == rows && src[i].Cols == cols)
				{
					dstChannels += src[i].Channels;
					srcPtrs[i] = src[i].MatObj;
				}
				else
					return;

			if (dstChannels > Mat.CV_CN_MAX) // exceed max channels support
				return;

			var srcPtrMat = new Mat(1, n * Marshal.SizeOf<IntPtr>(), Mat.makeType(1, Mat.MatDepth.CV_8U));
			if (!srcPtrMat.Valid) // alloc unmanaged memory
				return;
			Marshal.Copy(srcPtrs, 0, srcPtrMat.Data, n);
			NativeInvoker.coreCvMerge(srcPtrMat.Data, n, dst.MatObj);
		}
    }
}
