MY_TARGET := $(LIBS_BUILD_DIR)/libhttp.so
MY_STATIC_TARGET := $(LIBS_BUILD_DIR)/libhttp.a
MY_TARGETDIR := $(LIBS_BUILD_DIR)/libhttp

MY_SRCDIR := $(LIBS_DIR)/libhttp
LIBDEV_DIR := $(MY_SRCDIR)

MY_SRCS := \
	http.c \
	http_parser.c

#include $(addsuffix /makefile, $(addprefix $(MY_SRCDIR)/, \
#        lib \
#))

MY_INCLUDES := $(STDINCLUDE)
MY_CFLAGS := $(USER_CFLAGS) $(LIBC_EXTRA_CFLAGS)
MY_CPPFLAGS := $(USER_CFLAGS) $(LIBC_EXTRA_CFLAGS)
MY_LINKSCRIPT := $(LIBS_LDSCRIPT)

include make/lib.mk
