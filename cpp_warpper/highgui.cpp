#include "highgui.hpp"
#include "core.hpp"
#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  void _CDECL coreCvImread(const char *path, int flag, void *mat)
  {
    if (!path || !mat) return;
    *(cv::Mat *)mat = cv::imread(path, flag);
  }
  int/*bool*/ _CDECL coreCvImwrite(const char *path, void *mat)
  {
    if (!path || !mat) return 0;
    return cv::imwrite(path, *(cv::Mat *)mat);
  }

#ifdef __cplusplus
}
#endif // __cplusplus