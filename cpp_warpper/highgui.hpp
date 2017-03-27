#ifndef CORE_CV_HIGHGUI_H
#define CORE_CV_HIGHGUI_H

#include "common.h"
#include <opencv2/highgui/highgui.hpp>

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  OPENCV_API void* _CDECL coreCvImread(const char *path, int flag);
  OPENCV_API int/*bool*/ _CDECL coreCvImwrite(const char *path, void *mat);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif // CORE_CV_HIGHGUI_H