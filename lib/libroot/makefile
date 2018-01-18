MY_TARGET := $(LIBS_BUILD_DIR)/libroot.so
MY_STATIC_TARGET := $(LIBS_BUILD_DIR)/libroot.a
MY_TARGETDIR := $(LIBS_BUILD_DIR)/libroot

MY_SRCDIR := $(LIBS_DIR)/libroot
LIBDEV_DIR := $(MY_SRCDIR)

MY_SRCS := \
	FileSystem.cpp \
	RootFs.cpp

#include $(addsuffix /makefile, $(addprefix $(MY_SRCDIR)/, \
#        lib \
#))

MY_INCLUDES := $(STDINCLUDE)
MY_CFLAGS := $(USER_CFLAGS) $(LIBC_EXTRA_CFLAGS)
MY_CPPFLAGS := $(USER_CFLAGS) $(LIBC_EXTRA_CFLAGS)
MY_LINKSCRIPT := $(LIBS_LDSCRIPT)

include make/lib.mk
