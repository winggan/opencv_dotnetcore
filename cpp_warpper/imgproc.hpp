#ifndef CORE_CV_IMGPROC_H
#define CORE_CV_IMGPROC_H
#include "common.h"
#include <opencv2/imgproc/imgproc.hpp>

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  OPENCV_API void _CDECL coreCvCvtColor(void *src, void *dst, int code);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif // CORE_CV_IMGPROC_H
