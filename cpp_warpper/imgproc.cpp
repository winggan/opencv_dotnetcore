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

#ifdef __cplusplus
}
#endif // __cplusplus