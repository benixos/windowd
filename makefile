include make/make.syscfg
include make/macros.mk

# sub-makefiles have to fill these in
ALL_DEPS =
CLEAN =
FINAL =
TOOLS =
ALL_OBJS =


FINAL := $(call TOBUILDDIR, final)
#$(warning FINAL = $(FINAL))

final: $(FINAL)

include config/build.config

include apps/makefile
include cmd/makefile
include data/makefile
include docs/makefile
include examples/makefile
include kits/makefile
include lib/makefile
include test/makefile
include vendor/makefile

apps: $(APPS)

examples: $(EXAMPLES)

kits: $(KITS)

libs: $(LIBS)

cmds: $(CMD)

clean: $(CLEAN)
	rm -f $(ALL_OBJS)
	rm -f $(ALL)
	rm -f $(FINAL)

spotless:
	rm -rf $(BUILD_DIR)
