#ifndef CORE_CV_COMMON_H
#define CORE_CV_COMMON_H

#ifdef _WIN32
#define _CDECL __cdecl
#define OPENCV_API __declspec(dllexport)
#else // _WIN32
#define _CDECL 
#define OPENCV_API 
#endif // _WIN32

#endif // CORE_CV_COMMON_H