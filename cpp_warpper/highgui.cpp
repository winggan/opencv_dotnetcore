#include "highgui.hpp"
#include "core.hpp"
#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  void* _CDECL coreCvImread(const char *path, int flag)
  {
    cv::Mat *p = (cv::Mat *)coreCvCreateMat();
    if (!p) return p;
    *p = cv::imread(path, flag);
    return p;
  }
  int/*bool*/ _CDECL coreCvImwrite(const char *path, void *mat)
  {
    if (!path || !mat) return 0;
    return cv::imwrite(path, *(cv::Mat *)mat);
  }

#ifdef __cplusplus
}
#endif // __cplusplus