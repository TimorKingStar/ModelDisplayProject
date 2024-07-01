//
//  UntiyToIOS.m
//  Unity-iPhone
//
//  Created by ayoue on 2024/6/15.
//

#import <Foundation/Foundation.h>


extern "C" {
    
    extern void CadenceInitWith(const char *modelName,const char *progress) {
        NSString *s = [NSString stringWithUTF8String:modelName];
        NSString *ss = [NSString stringWithUTF8String:progress];
        NSLog(@"接受从Unity传递过来的字符串 %@---%@", s,ss);
    }
    
}
