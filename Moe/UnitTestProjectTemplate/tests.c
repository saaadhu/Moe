/*
 * Tests.c
 *
 * Created: 4/22/2011 1:12:40 PM
 *  Author: skselvaraj
 */ 

#include "moe.h"
#include "tests.h"
#include "asserts.h"

TEST_LIST_START
	TEST(test_addition)
	TEST(test_subtraction)
TEST_LIST_END

void test_addition()
{
	ASSERT_ARE_EQUAL(123, 12 + 3);
}

void test_subtraction()
{
	ASSERT_ARE_EQUAL(123, 126 - 3);
}