#ifndef CORE_CV_CORE_H
#define CORE_CV_CORE_H
#include "common.h"
#include <opencv2/core/core.hpp>

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  OPENCV_API void* _CDECL coreCvCreateMat();
  OPENCV_API void _CDECL coreCvdestroyMat(void *mat);

  //rest of the funtions assume pointer "mat" is valid for efficiency

  OPENCV_API int _CDECL coreCvGetCols(void *mat);
  OPENCV_API int _CDECL coreCvGetRows(void *mat);

  OPENCV_API int _CDECL coreCvGetType(void *mat);

  OPENCV_API void *_CDECL coreCvGetRow(void *mat, int row);

  // return null if mat is not continuous
  OPENCV_API void *_CDECL coreCvGetData(void *mat);

  OPENCV_API int/*bool*/ _CDECL coreCvIsContinuous(void *mat);
  OPENCV_API int _CDECL coreCvGetElementSize(void *mat);
  OPENCV_API int/*bool*/ _CDECL corCvIsEmpty(void *mat);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif // CORE_CV_CORE_H