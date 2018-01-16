#include <malloc.h>
#include <string.h>

#include <bitchain.h>
#include <bitblock.h>
#include <bithash.h>
#include <blockfile.h>

bitblock* createblock(bitblock* parent, char* name, char* parentkey, char* data){
	bitblock* newblock = (bitblock *)malloc(sizeof(bitblock));

	newblock->parent = parent;
	newblock->next = NULL;

	sprintf(newblock->blockName, name);
	sprintf(newblock->parentkey, parentkey);
	sprintf(newblock->data, data);

	return newblock;
};

bitblock* createroot(){
	//TODO: We really want fake, but legit looking data for unused fields
        bitblock* newblock = (bitblock *)malloc(sizeof(bitblock));

        newblock->parent = NULL;
        newblock->next = NULL;

        //newblock->blockName = NULL;
        //newblock->parentkey = NULL;
        //newblock->data = NULL;

        return newblock;	
};

bitblock* createleaf(bitblock* parent){
        //TODO: We really want fake, but legit looking data for unused fields
	bitblock* newblock = (bitblock *)malloc(sizeof(bitblock));
	
	newblock->parent = parent;
	newblock->next = NULL;
	//newblock->blockName = NULL;
//	memcpy(newblock->parentkey,parent->parentkey,BITBLOCK_BLOCK_KEY);
	//newblock->data = NULL; 
	return newblock;                                                        
};

int addblock(bitblock* parent, bitblock* child){
	return -1;	
};

int setparent(bitblock* block, bitblock* parent){
	return -1;
};

int setchild(bitblock* block, bitblock* child){
	return -1;
};

int setname(bitblock* block, char* name){
	return -1;
};

int setparentkey(bitblock* block, char* key){
	return -1;
};

int setdata(bitblock* block, char* data){
	return -1;
};





