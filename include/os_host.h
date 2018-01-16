/*
 Peyote 3D 
 
 Copyright (c) 2010 Douglas J Lockamy
 
 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 */
#ifndef OS_HOST_H
#define OS_HOST_H

#ifdef __cplusplus
extern "C" {
#endif

	int os_init(int argc, char **args);
	int os_Run(void);
	int os_shutdownApp(void);
	
	typedef int os_threadID;
	
	os_threadID os_spawn_thread(char *func, const char* name,int priority, void* data);
	int os_start_thread(os_threadID);
	int os_pause_thread(os_threadID);
	int os_wait_for_thread(os_threadID);
	int os_exit_thread(void);
#ifdef __cplusplus
	}
#endif
	
#endif //OS_HOST_H


