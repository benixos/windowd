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

//extern AppResponder *controller;

@implementation AppDelegate

+(void)populateMainMenu
{
	NSMenu *mainMenu = [[NSMenu alloc] initWithTitle:@"MainMenu"];
	NSMenuItem *menuItem;
	NSMenu *submenu;
	
	menuItem = [mainMenu addItemWithTitle:@"Apple" action:NULL keyEquivalent:@""];
	submenu = [[NSMenu alloc] initWithTitle:@"Apple"];
	[NSApp performSelector:@selector(setAppleMenu:) withObject:submenu];
	[self populateApplicationMenu:submenu];
	[mainMenu setSubmenu:submenu forItem:menuItem];
	
	menuItem = [mainMenu addItemWithTitle:@"View" action:NULL keyEquivalent:@""];
	submenu = [[NSMenu alloc] initWithTitle:NSLocalizedString(@"View", "@The View menu")];
	[self populateViewMenu:submenu];
	[mainMenu setSubmenu:submenu forItem:menuItem];
	
	menuItem = [mainMenu addItemWithTitle:@"Window" action:NULL keyEquivalent:@""];
	submenu = [[NSMenu alloc] initWithTitle:NSLocalizedString(@"Window", @"The Window menu")];
	[self populateWindowMenu:submenu];
	[mainMenu setSubmenu:submenu forItem:menuItem];
	[NSApp setWindowsMenu:submenu];
	
	menuItem = [mainMenu addItemWithTitle:@"Help" action:NULL keyEquivalent:@""];
	submenu = [[NSMenu alloc] initWithTitle:NSLocalizedString(@"Help", @"The Help menu")];
	[self populateHelpMenu:submenu];
	[mainMenu setSubmenu:submenu forItem:menuItem];
	
	[NSApp setMainMenu:mainMenu];
}

+(void)populateApplicationMenu:(NSMenu *)aMenu
{
	NSString *applicationName = [[NSProcessInfo processInfo] processName];
	NSMenuItem *menuItem;
	
	menuItem = [aMenu addItemWithTitle:[NSString stringWithFormat:@"%@ %@", NSLocalizedString(@"About", nil), applicationName]
								action:@selector(orderFrontStandardAboutPanel:)
						 keyEquivalent:@""];
	[menuItem setTarget:NSApp];
	
	[aMenu addItem:[NSMenuItem separatorItem]];
	
	menuItem = [aMenu addItemWithTitle:NSLocalizedString(@"Preferences...", nil)
								action:NULL
						 keyEquivalent:@","];
	
	[aMenu addItem:[NSMenuItem separatorItem]];
	
	menuItem = [aMenu addItemWithTitle:NSLocalizedString(@"Services", nil)
								action:NULL
						 keyEquivalent:@""];
	NSMenu * servicesMenu = [[NSMenu alloc] initWithTitle:@"Services"];
	[aMenu setSubmenu:servicesMenu forItem:menuItem];
	[NSApp setServicesMenu:servicesMenu];
	
	[aMenu addItem:[NSMenuItem separatorItem]];
	
	menuItem = [aMenu addItemWithTitle:[NSString stringWithFormat:@"%@ %@", NSLocalizedString(@"Hide", nil), applicationName]
								action:@selector(hide:)
						 keyEquivalent:@"h"];
	[menuItem setTarget:NSApp];
	
	menuItem = [aMenu addItemWithTitle:NSLocalizedString(@"Hide Others", nil)
								action:@selector(hideOtherApplications:)
						 keyEquivalent:@"h"];
	[menuItem setKeyEquivalentModifierMask:NSCommandKeyMask | NSAlternateKeyMask];
	[menuItem setTarget:NSApp];
	
	menuItem = [aMenu addItemWithTitle:NSLocalizedString(@"Show All", nil)
								action:@selector(unhideAllApplications:)
						 keyEquivalent:@""];
	[menuItem setTarget:NSApp];
	
	[aMenu addItem:[NSMenuItem separatorItem]];
	
	menuItem = [aMenu addItemWithTitle:[NSString stringWithFormat:@"%@ %@", NSLocalizedString(@"Quit", nil), applicationName]
								action:@selector(terminate:)
						 keyEquivalent:@"q"];
	[menuItem setTarget:NSApp];
}

+(void)populateViewMenu:(NSMenu *)aMenu
{
	NSMenuItem *menuItem;
	menuItem = [aMenu addItemWithTitle:NSLocalizedString(@"Full Screen", nil)
								action:@selector(fullscreen:) keyEquivalent:@"F"];
	[menuItem setTarget:NSApp];
	
	menuItem = [aMenu addItemWithTitle:NSLocalizedString(@"Cmd-F exits full screen", nil)
								action:NULL keyEquivalent:@""];
}

+(void)populateWindowMenu:(NSMenu *)aMenu
{
}

+(void)populateHelpMenu:(NSMenu *)aMenu
{
}

- (void)applicationWillFinishLaunching:(NSNotification *)notification
{
	printf("cocoa applicationWillFinishLaunching\n");
}

- (void)applicationDidFinishLaunching:(NSNotification *)notification
{
	printf("cocoa applicationDidFinishLaunching\n");
	[AppDelegate populateMainMenu];
	[NSThread detachNewThreadSelector:@selector(AppMain)
							 toTarget:self withObject:nil];
	[NSApplication detachDrawingThread:@selector(AppMain)
							  toTarget:self withObject:nil];
	[readHandle waitForDataInBackgroundAndNotify];
}

- (id)init
{
	
	printf("cocoa application init\n");
	
	//if(self = [super init]){
	readHandle = [[NSFileHandle alloc] initWithFileDescriptor:3 closeOnDealloc:YES];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(AppMain:)
												 name:NSFileHandleDataAvailableNotification
											   object:readHandle];
	[[[NSWorkspace sharedWorkspace] notificationCenter] addObserver:self
														   selector:@selector(receiveWake:)
															   name:NSWorkspaceDidWakeNotification
															 object:NULL];
	//}
	[AppDelegate populateMainMenu];
	
	return self;
}

- (void)dealloc
{
	[[NSNotificationCenter defaultCenter] removeObserver:self];
	[readHandle release];
	return [super dealloc];
}
#pragma mark Notifications


@end


@interface AppResponder (InternalMethods)
- (void) setupRenderTimer;
- (void) updateGLView:(NSTimer *)timer;
- (void) createFailed;
@end

@implementation AppResponder

/*
 * Setup timer to update the OpenGL view.
 */
- (void) setupRenderTimer
{
	NSTimeInterval timeInterval = 0.005;
	
	renderTimer = [ [ NSTimer scheduledTimerWithTimeInterval:timeInterval
													  target:self
													selector:@selector( updateGLView: )
													userInfo:nil repeats:YES ] retain ];
	[ [ NSRunLoop currentRunLoop ] addTimer:renderTimer
									forMode:NSEventTrackingRunLoopMode ];
	[ [ NSRunLoop currentRunLoop ] addTimer:renderTimer
									forMode:NSModalPanelRunLoopMode ];
}


/*
 * Called by the rendering timer.
 */
- (void) updateGLView:(NSTimer *)timer
{
	//if( glView != nil )
	//	[ glView drawRect:[ glView frame ] ];
}  


/*
 * Handle key presses
 */
- (void) keyDown:(NSEvent *)theEvent
{
	unichar unicodeKey;
	
	unicodeKey = [ [ theEvent characters ] characterAtIndex:0 ];
	switch( unicodeKey )
	{
			printf("keyDown\n");
			// Handle key presses here
	}
}


/*
 * Set full screen.
 */
- (IBAction)setFullScreen:(id)sender
{
	/*
	[ glWindow setContentView:nil ];
	if( [ glView isFullScreen ] )
	{
		if( ![ glView setFullScreen:FALSE inFrame:[ glWindow frame ] ] )
			[ self createFailed ];
		else
			[ glWindow setContentView:glView ];
	}
	else
	{
		if( ![ glView setFullScreen:TRUE
							inFrame:NSMakeRect( 0, 0, 800, 600 ) ] )
			[ self createFailed ];
	}*/
}


/*
 * Called if we fail to create a valid OpenGL view
 */
- (void) createFailed
{
	NSWindow *infoWindow;
	
	infoWindow = NSGetCriticalAlertPanel( @"Initialization failed",
                                         @"Failed to initialize OpenGL",
                                         @"OK", nil, nil );
	[ NSApp runModalForWindow:infoWindow ];
	[ infoWindow close ];
	[ NSApp terminate:self ];
}


/* 
 * Cleanup
 */
- (void) dealloc
{
	//[ glWindow release ]; 
	//[ glView release ];
	if( renderTimer != nil && [ renderTimer isValid ] )
		[ renderTimer invalidate ];
}

@end





