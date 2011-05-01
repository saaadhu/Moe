/*
 * Tests.c
 *
 * Created: 4/22/2011 1:12:40 PM
 *  Author: skselvaraj
 */ 
#ifndef DRIVER_H
#define DRIVER_H

typedef void (*test_func)();

typedef struct tagFunctionNameAndAddress
{
	char *name;
	test_func test_function;	
} FunctionNameAndAddress;

#define SYM(x) #x
#define TEST_LIST_START FunctionNameAndAddress namesAndAddresses[] = {
#define TEST(x) { SYM(x), x },
#define TEST_LIST_END }; int numNamesAndAddresses = sizeof(namesAndAddresses)/sizeof(namesAndAddresses[0]);

#endif
