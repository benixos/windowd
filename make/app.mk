MY_TARGET_IN := $(MY_TARGET)
MY_TARGETDIR_IN := $(MY_TARGETDIR)
MY_SRCDIR_IN := $(MY_SRCDIR)
MY_SRCS_IN := $(MY_SRCS)
MY_EXTRAOBJS_IN := $(MY_EXTRAOBJS)
MY_CFLAGS_IN := $(MY_CFLAGS)
MY_CPPFLAGS_IN := $(MY_CPPFLAGS)
MY_LDFLAGS_IN := $(MY_LDFLAGS)
MY_INCLUDES_IN := $(MY_INCLUDES)
MY_LIBS_IN := $(MY_LIBS)
MY_LIBPATHS_IN := $(MY_LIBPATHS)
MY_DEPS_IN := $(MY_DEPS)
MY_LINKSCRIPT_IN := $(MY_LINKSCRIPT)
MY_GLUE_IN := $(MY_GLUE)

$(warning MY_OBJS = $(MY_OBJS))

# extract the different source types out of the list
$(warning MY_SRCS_IN = $(MY_SRCS_IN))
MY_CPPSRCS_IN := $(filter %.cpp,$(MY_SRCS_IN))
MY_CSRCS_IN := $(filter %.c,$(MY_SRCS_IN))
MY_ASMSRCS_IN := $(filter %.S,$(MY_SRCS_IN))

$(warning MY_CPPSRCS_IN = $(MY_CPPSRCS_IN))
$(warning MY_CSRCS_IN = $(MY_CSRCS_IN))
$(warning MY_ASMSRCS_IN = $(MY_ASMSRCS_IN))

# build a list of objects
MY_CPPOBJS_IN := $(addprefix $(MY_TARGETDIR_IN)/,$(patsubst %.cpp,%.o,$(MY_CPPSRCS_IN)))
MY_COBJS_IN := $(addprefix $(MY_TARGETDIR_IN)/,$(patsubst %.c,%.o,$(MY_CSRCS_IN)))
MY_ASMOBJS_IN := $(addprefix $(MY_TARGETDIR_IN)/,$(patsubst %.S,%.o,$(MY_ASMSRCS_IN)))
_TEMP_OBJS := $(MY_ASMOBJS_IN) $(MY_CPPOBJS_IN) $(MY_COBJS_IN) $(MY_EXTRAOBJS_IN)
$(warning _TEMP_OBJS = $(_TEMP_OBJS))

# add to the global object list
ALL_OBJS := $(ALL_OBJS) $(_TEMP_OBJS)

# add to the global deps
ALL_DEPS := $(ALL_DEPS) $(_TEMP_OBJS:.o=.d)

$(MY_TARGET_IN): MY_LDFLAGS_IN:=$(MY_LDFLAGS_IN)
$(MY_TARGET_IN): MY_LIBS_IN:=$(MY_LIBS_IN)
$(MY_TARGET_IN): MY_LIBPATHS_IN:=$(MY_LIBPATHS_IN)
$(MY_TARGET_IN): _TEMP_OBJS:=$(_TEMP_OBJS)
$(MY_TARGET_IN):: $(_TEMP_OBJS) $(MY_DEPS_IN)
#	@$(MKDIR)
	@mkdir -p $(MY_TARGETDIR_IN)
	@echo $(MY_SRCS_IN)
	@echo linking $@
	@$(CC) -L $(LIBGCC_PATH) -L $(LIBS_BUILD_DIR) $(MY_LIBPATHS_IN) -o $@ $(_TEMP_OBJS) $(MY_LIBS_IN) $(LIBGCC) 

include make/compile.mk

MY_TARGET :=
MY_TARGETDIR :=
MY_SRCDIR :=
MY_SRCS :=
MY_EXTRAOBJS :=
MY_CFLAGS :=
MY_CPPFLAGS :=
MY_LDFLAGS :=
MY_INCLUDES :=
MY_LIBS :=
MY_LIBPATHS :=
MY_DEPS :=
MY_LINKSCRIPT := 
MY_GLUE :=
