#ifndef BLOCKFILE_H
#define BLOCKFILE_H
#include <bitchain.h>
#include <blockfile.h>

#ifdef __cplusplus
extern "C" {
#endif

     blockfile* writeblockfile(blockfile* block, char* path);

     blockfile* loadblockfile(char *path);
     blockfile* loadblocks(char* blocklist, char* blockroot);

#ifdef __cplusplus
} /* extern "C" */
#endif

#endif

