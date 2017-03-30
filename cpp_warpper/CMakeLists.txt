cmake_minimum_required(VERSION 2.8)
project(opencv_dotnetcore CXX)

find_package(OpenCV REQUIRED core imgproc highgui)
file(GLOB_RECURSE lib_src "${CMAKE_SOURCE_DIR}/*.cpp")

include_directories(${CMAKE_SOURCE_DIR})
include_directories(${OpenCV_INCLUDE_DIRS})

add_library(opencv_dotnetcore SHARED ${lib_src})
target_link_libraries(opencv_dotnetcore opencv_core opencv_highgui opencv_imgproc)
