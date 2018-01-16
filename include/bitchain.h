#ifndef BITCHAIN_H
#define BITCHAIN_H

#ifdef __cplusplus
extern "C" {
#endif

#define BITBLOCK_FILENAME_HASH_LENGTH 65
#define BITBLOCK_DATA_NAME_BLOCK_HASH_LENGTH 65
#define BITBLOCK_BLOCK_NAME_LENGTH 130 //BITBLOCK_FILE_HASH_LENGTH + BITBLOCK_DATA_BLOCK_HASH_LENGTH;
#define BITBLOCK_DATA_BLOCK 1024
#define BITBLOCK_BLOCK_KEY 65

typedef enum streamtype {
	BLOCK,
	STREAM
} streamtype;

typedef struct bitblock {
	struct bitblock *parent;
	struct bitblock *next;

	char blockName[BITBLOCK_BLOCK_NAME_LENGTH];
	char parentkey[BITBLOCK_BLOCK_KEY];
	char data[BITBLOCK_DATA_BLOCK];
} bitblock;


typedef struct blockfile {
	char filename[BITBLOCK_FILENAME_HASH_LENGTH];
	int type;
	bitblock *root; 
} blockfile;



#ifdef __cplusplus
} /* extern "C" */
#endif

#endif
