#include "imgproc.hpp"

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  void _CDECL coreCvCvtColor(void *src, void *dst, int code)
  {
    if (!src || !dst)
      return;
    cv::cvtColor(*(cv::Mat*)src, *(cv::Mat *)dst, code);
  }

  OPENCV_API void _CDECL coreCvWarpAffine(void *src, void *dst, void* trans, int outputW, int outputH, 
                                          int flags, int borderMode, double *borderValue)
  {
    if (!src || !dst || !trans)
      return;
    const cv::Mat &transMat = *(cv::Mat *)trans;
    if (transMat.type() != CV_32F || transMat.rows != 2 || transMat.cols != 3)
      return;
    cv::Scalar bValue;
    if (borderMode == cv::BORDER_CONSTANT && !borderValue)
      bValue = cv::Scalar::all(0.0);
    else 
    {
      bValue[0] = borderValue[0];
      bValue[1] = borderValue[1];
      bValue[2] = borderValue[2];
      bValue[3] = borderValue[3];
    }
    cv::warpAffine(*(cv::Mat*)src, *(cv::Mat*)dst, transMat, cv::Size(outputW, outputH), flags, borderMode, bValue);
  }

  OPENCV_API void _CDECL coreCvResize(void *src, void *dst, int outputW, int outputH, 
                                      double fx, double fy, int interpolation)
  {
    if (!src || !dst)
      return;
    cv::Size dSize(outputW, outputH);
    const cv::Mat &srcMat = *(cv::Mat*)src;
    if (dSize.height > 0 && dSize.width > 0)
      cv::resize(srcMat, *(cv::Mat*)dst, dSize, 0.0, 0.0, interpolation);
    else if (cv::saturate_cast<int>(fx * srcMat.cols) > 0 
          && cv::saturate_cast<int>(fy * srcMat.rows) > 0)
      cv::resize(srcMat, *(cv::Mat*)dst, cv::Size(), fx, fy, interpolation);
  }

#ifdef __cplusplus
}
#endif // __cplusplus