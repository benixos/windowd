#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <malloc.h>
#include <unistd.h>

#include <bitchain.h>
#include <bitblock.h>
#include <bithash.h>
#include <blockfile.h>

blockfile* createblockfile(char* path){
	char* filehash = getfilehash(path);

	blockfile* fileblock = (blockfile*) malloc(sizeof(blockfile));
	memcpy(fileblock->filename,&filehash, BITBLOCK_FILENAME_HASH_LENGTH);
	fileblock->type = BLOCK;

	FILE *fp = fopen(path, "r");

	if(fp != NULL ) {
		int res;
		char buf[BITBLOCK_DATA_BLOCK];
		bitblock* lastblock;

		fileblock->root = createroot();
		
		lastblock = &fileblock->root;
	
		while( (res = fread(buf, 1, sizeof buf, fp)) > 0){
			lastblock->next = createblock(lastblock, "", "", buf);
			lastblock = &lastblock->next;
		}

		bitblock* leaf = createleaf(lastblock);

		fclose(fp);
	}

	return fileblock;
};


/*
 * Write a full blockchain to disk for testing purposes.
 */
blockfile* writeblockfile(blockfile* block, char* path){
	FILE* fd = fopen(path, "wb");

	if(fd != NULL){
		fwrite(&block, sizeof(blockfile), 1, fd);
								}
	return close(fd);
};

blockfile* exportfiledata(blockfile* block, char* path){
	FILE* fd = fopen(path, "wb");
	int res;

	if(fd != NULL){
		bitblock* writeBlock;

		writeBlock = &block->root->next;

//		while(writeBlock != NULL ) {                   

                     //fwrite(&block->root, sizeof(bitblock), 1, fd);
                     
		     writeBlock = writeBlock->next;
//                }
	
		return close(fd);
	}
	return -1;
};

blockfile* loadblockfile(char *path){
	return -1;
};

blockfile* loadblocks(char* blocklist, char* blockroot){
	return -1;
};
