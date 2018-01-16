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
#define Point OSXPoint
#define Rect OSXRect
#define Cursor OSXCursor

#import <Foundation/Foundation.h>
#import <AppKit/AppKit.h>

#import <Cocoa/Cocoa.h>

#import <OpenGL/OpenGL.h>
#import <OpenGL/gl.h>
#import <OpenGL/glu.h>

#import <os_host.h>
#import "os_init_mac.h"

#include <errno.h>
#include <sys/select.h>

@interface PApp : NSApplication 
{
}
//- (void) init;
//	- (void) sendEvent:(NSEvent *)event;
	

@end

@implementation PApp


//- (void) init
//{
//		printf("NSApplication Papp");
//};
/*
- (void)sendEvent:(NSEvent *)event
{
    if ([event type] == NSKeyDown)
    {
        NSString *str;
        printf("keydown \n");
        str = [event characters];
        //if ([str characterAtIndex:0] == 0x1B) // escape
        //{
            //[(FunHouseAppDelegate*)[self delegate] zoomToFullScreenAction:self];
          //  return;
        //}
    }
    [super sendEvent:event];
}*/

@end


@interface AppController : NSObject {
}
- (IBAction)doSomething:(id)sender;
@end

@implementation AppController
- (IBAction)doSomething:(id)sender;
{
	printf("Button clicked!\n");
	NSWindow *infoWindow;
	
	infoWindow = NSGetCriticalAlertPanel( @"Initialization failed",
                                         @"Failed to initialize OpenGL",
                                         @"OK", nil, nil );
	[ NSApp runModalForWindow:infoWindow ];
	[ infoWindow close ];
}
@end

void boo(void)
{
		printf("boo!!\n\n");
};

NSAutoreleasePool *pool;
NSApplication *app;
AppDelegate *application;
AppResponder *controller;

int os_init(int argc, char **args){
	controller = nil;

	printf("os_init----------- \n");
	
    NSAutoreleasePool* pool = [[NSAutoreleasePool alloc] init];
	
    [NSApplication sharedApplication];

	[application init];
	[application populateMainMenu];
	
	[app setDelegate:application];	
    // @todo Create menus (especially Quit!)
	
    // Show window and run event loop
	//os_window_init(argc, args);

	return 0;
}

int os_Run(void)
{		
    [NSApp run];

/*	
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
    [application finishLaunching];
	
    //shouldKeepRunning = YES;
    //do
    //{
        [pool release];
        pool = [[NSAutoreleasePool alloc] init];
		
        NSEvent *event =
		[app
		 nextEventMatchingMask:NSAnyEventMask
		 untilDate:[NSDate distantFuture]
		 inMode:NSDefaultRunLoopMode
		 dequeue:YES];
		
        [app sendEvent:event];
        [app updateWindows];
    //} ;//while (shouldKeepRunning);
	
    [pool release];
*/
	return 0;
};

int os_shutdownApp(void)
{

	[application setDelegate:nil];
	[pool release];
	
	return 0;	
};

