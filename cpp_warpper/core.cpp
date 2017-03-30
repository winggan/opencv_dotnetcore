#include "core.hpp"
#include <new>
#include <vector>

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

  void _CDECL coreCvConvertMat(void *src, void *dst, int dstType, double alpha, double beta)
  {
    if (!src || !dst)
      return;
    
    ((cv::Mat *)src)->convertTo(*(cv::Mat *)dst, dstType, alpha, beta);
  }
  
  void _CDECL coreCvAssignMat(void *src, void *dst)
  {
    if (!src || !dst)
      return;
    *(cv::Mat *)dst = *((cv::Mat *)src);
  }

  void _CDECL coreCvMatRoi(void *src, int x, int y, int w, int h, void *dst)
  {
    if (!src || !dst)
      return;
    cv::Rect roi(x, y, w, h);
    const cv::Mat &srcMat = *((cv::Mat *)src);
    if (x >= 0 && y >= 0 && x + w <= srcMat.cols && y + h <= srcMat.rows)
      *(cv::Mat *)dst = srcMat(roi);
  }

  void _CDECL coreCvSplit(void *src, void *dsts, int maxChannel)
  {
    if (!src || !dsts)
      return;
    cv::Mat &srcMat = *(cv::Mat*)src;
    if (maxChannel < srcMat.channels())
      return;
    
    int channels = srcMat.channels();
    cv::Mat **dstMats = (cv::Mat **)dsts;
    for (int i = 0; i < channels; i++)
      if (!dstMats[i]) return;
    
    std::vector<cv::Mat> tmp;
    cv::split(srcMat, tmp);
    
    for (int i = 0; i < channels; i++)
      *dstMats[i] = tmp[i];
  }
  void _CDECL coreCvMerge(void *srcs, int n, void *dst)
  {
    if (!srcs || !dst)
      return;

    cv::Mat **srcMats = (cv::Mat **)srcs;
    for (int i = 0; i < n; i++)
      if (!srcMats[i]) return;
    
    std::vector<cv::Mat> tmp;
    tmp.reserve(n);
    for (int i = 0; i < n; i++)
      tmp.push_back(*srcMats[i]);

    cv::Mat &dstMat = *(cv::Mat *)dst;
    cv::merge(tmp, dstMat);
  }

#ifdef __cplusplus
}
#endif // __cplusplus