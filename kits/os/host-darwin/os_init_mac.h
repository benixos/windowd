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
#import <Foundation/NSObject.h>
#import <AppKit/NSMenu.h>

#ifndef OS_MAIN_H
#define OS_MAIN_H


@interface AppView : NSOpenGLView
{
	int colorBits, depthBits;
	BOOL runningFullScreen;
	NSDictionary *originalDisplayMode;
}

- (id) initWithFrame:(NSRect)frame colorBits:(int)numColorBits
		   depthBits:(int)numDepthBits fullscreen:(BOOL)runFullScreen;
- (void) reshape;
- (void) drawRect:(NSRect)rect;
- (BOOL) isFullScreen;
- (BOOL) setFullScreen:(BOOL)enableFS inFrame:(NSRect)frame;
- (void) dealloc;

@end

@class NSFileHandle;

@interface AppDelegate : NSObject //<ApplicationDelegate>
{
	NSFileHandle *readHandle;
	//UIWindow *window;
    //EAGLView *renderView;
}
+(void)populateMainMenu;
+(void)populateApplicationMenu:(NSMenu *)aMenu;
+(void)populateViewMenu:(NSMenu *)aMenu;
+(void)populateWindowMenu:(NSMenu *)aMenu;
+(void)populateHelpMenu:(NSMenu *)aMenu;

//@property (nonatomic, retain) IBOutlet EAGLView *glView;

@end


@interface AppResponder : NSResponder
{
	//IBOutlet NSWindow *glWindow;
	
	NSTimer *renderTimer;
	//Lesson01View *glView;
}

- (void) awakeFromNib;
- (void) keyDown:(NSEvent *)theEvent;
- (IBAction) setFullScreen:(id)sender;
- (void) dealloc;

@end


#endif //OS_MAIN_H
