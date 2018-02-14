#include <bitchain.h>

#ifndef BITCHAIN_H
#define BITCHAIN_H

#ifdef __cplusplus
extern "C" {
#endif

bitblock createblock(bitblock* parent, char* name, char* key, char* data);

bitblock* createroot();
bitblock* createleaf(bitblock* parent);

int addblock(bitblock* parent, bitblock* child);

int setparent(bitblock* block, bitblock* parent);
int setchild(bitblock* block, bitblock* child);

int setname(bitblock* block, char* name);
int setparentkey(bitblock* block, char* key);

int setdata(bitblock* block, char* data);


#ifdef __cplusplus
} /* extern "C" */
#endif

#endif
