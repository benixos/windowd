#gfx makefile
MY_TARGETDIR := $(EXAMPLES_BUILD_DIR)/gfx
MY_SRCDIR := $(EXAMPLES_DIR)/gfx
MY_TARGET :=  $(MY_TARGETDIR)/gfx
ifeq ($(call FINDINLIST,$(MY_TARGET),$(ALL)),1)

MY_SRCS := \
	main.c

MY_INCLUDES := $(STDINCLUDE)
MY_CFLAGS := $(USER_CFLAGS)
MY_LIBS := 
MY_LIBPATHS :=
MY_DEPS :=

include make/app.mk

endif
