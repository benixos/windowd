TEST_DIR := test/client
TEST_INSTALL_DIR := 
TEST_SRC := \
    $(TEST_DIR)/index.html \
    $(TEST_DIR)/index.js

testapp: agilib-js jquery xmpp-node
	@mkdir -p $(BUILD_DIR)
	@mkdir -p $(BUILD_DIR)/applications
	@mkdir -p $(BUILD_DIR)/applications/test   
	@cp $(TEST_DIR)/index.html $(BUILD_DIR)/
	@cp $(TEST_DIR)/index.js $(BUILD_DIR)/
