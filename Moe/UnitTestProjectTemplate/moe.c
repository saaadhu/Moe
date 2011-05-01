/*
 * Tests.c
 *
 * Created: 4/22/2011 1:12:40 PM
 *  Author: skselvaraj
 */ 

#include "moe.h"

extern FunctionNameAndAddress namesAndAddresses[];
extern int numNamesAndAddresses;

void do_nothing();

void init_test_list()
{
	do_nothing();
}

void end_test_list()
{
	do_nothing();
}

int main(void)
{
	init_test_list();
	for (int i = 0; i<numNamesAndAddresses; i++)
	{
		do_nothing();		
	}
	end_test_list();

	for (int i = 0; i<numNamesAndAddresses; i++)
	{
		do_nothing();
		namesAndAddresses[i].test_function();				
		do_nothing();
	}

	do_nothing();
}    


