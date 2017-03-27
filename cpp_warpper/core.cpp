#include "core.hpp"
#include <new>

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  void* _CDECL coreCvCreateMat()
  {
    return new (std::nothrow) cv::Mat();
  }
  void _CDECL coreCvdestroyMat(void *mat)
  {
    if (mat) delete (cv::Mat*)mat;
  }

  int _CDECL coreCvGetCols(void *mat)
  {
    return ((cv::Mat*)mat)->cols;
  }
  int _CDECL coreCvGetRows(void *mat)
  {
    return ((cv::Mat*)mat)->rows;
  }

  int _CDECL coreCvGetType(void *mat)
  {
    return ((cv::Mat*)mat)->type();
  }

  void *_CDECL coreCvGetRow(void *mat, int row)
  {
    return ((cv::Mat*)mat)->ptr(row);
  }

  // return null if mat is not continuous
  void *_CDECL coreCvGetData(void *mat)
  {
    cv::Mat *p = (cv::Mat *)mat;
    if (!p->isContinuous()) return NULL;
    return p->data;
  }

  int/*bool*/ _CDECL coreCvIsContinuous(void *mat)
  {
    return ((cv::Mat*)mat)->isContinuous();
  }
  int _CDECL coreCvGetElementSize(void *mat)
  {
    return ((cv::Mat*)mat)->elemSize();
  }
  int/*bool*/ _CDECL corCvIsEmpty(void *mat)
  {
    return ((cv::Mat*)mat)->empty();
  }


#ifdef __cplusplus
}
#endif // __cplusplus