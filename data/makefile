DATA_DIR := data

DATA_DIRS := \
    $(DATA_DIR)/etc \
    $(DATA_DIR)/images  

data-node:

data: $(DATA_DIRS)
	@mkdir -p $(BUILD_DIR)
	@cp -r $(DATA_DIRS) $(BUILD_DIR)
    
data: data-$(HOST)