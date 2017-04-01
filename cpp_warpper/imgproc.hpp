#ifndef CORE_CV_IMGPROC_H
#define CORE_CV_IMGPROC_H
#include "common.h"
#include <opencv2/imgproc/imgproc.hpp>

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  OPENCV_API void _CDECL coreCvCvtColor(void *src, void *dst, int code);
  OPENCV_API void _CDECL coreCvWarpAffine(void *src, void *dst, void* trans, int outputW, int outputH, 
                                          int flags, int borderMode, double *borderValue);
  OPENCV_API void _CDECL coreCvResize(void *src, void *dst, int outputW, int outputH, 
                                      double fx, double fy, int interpolation);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif // CORE_CV_IMGPROC_H
