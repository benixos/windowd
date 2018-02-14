#include <bitchain.h>

#ifndef BITHASH_H
#define BITHASH_H

#ifdef __cplusplus
extern "C" {
#endif

	char* getfilehash(char* path);
	char* getblockhash(char* block);

#ifdef __cplusplus
} /* extern "C" */
#endif

#endif

