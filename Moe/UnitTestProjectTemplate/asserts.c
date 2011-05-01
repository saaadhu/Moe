/*
 * Tests.c
 *
 * Created: 4/22/2011 1:12:40 PM
 *  Author: skselvaraj
 */ 

#include "asserts.h"

#define ASSERT_ARE_EQUAL(expected, actual) assert_are_equal(__FUNCTION__, expected, actual)

void do_nothing(){}

void assert_are_equal(const char *function_name, int expected, int actual)
{
	do_nothing();
}