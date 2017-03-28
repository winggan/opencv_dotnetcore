#include "core.hpp"
#include <new>

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus

  void* _CDECL coreCvCreateEmptyMat()
  {
    return new (std::nothrow) cv::Mat();
  }
  void* _CDECL coreCvCreateMat(int rows, int cols, int type)
  {
    return new (std::nothrow) cv::Mat(rows, cols, type);
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
  int/*bool*/ _CDECL coreCvIsEmpty(void *mat)
  {
    return ((cv::Mat*)mat)->empty();
  }

  void _CDECL coreCvCloneMat(void *src, void *dst)
  {
    if (!src || !dst)
      return;

    *(cv::Mat *)dst = ((cv::Mat *)src)->clone();
  }

  OPENCV_API void _CDECL coreCvConvertMat(void *src, void *dst, int dstType, double alpha, double beta)
  {
    if (!src || !dst)
      return;
    
    ((cv::Mat *)src)->convertTo(*(cv::Mat *)dst, dstType, alpha, beta);
  }

#ifdef __cplusplus
}
#endif // __cplusplus