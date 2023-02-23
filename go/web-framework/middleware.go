package main

import (
	"fmt"
	"web-framework/framework"
)

func Test1() framework.ControllerHandler {
	return func(c *framework.Context) error {
		fmt.Println("Test1 start")
		c.Next()
		fmt.Println("Test1 end")
		return nil
	}
}
func Test2() framework.ControllerHandler {
	return func(c *framework.Context) error {
		fmt.Println("Test2 start")
		c.Next()
		fmt.Println("Test2 end")
		return nil
	}
}
func Test3() framework.ControllerHandler {
	return func(c *framework.Context) error {
		fmt.Println("Test3 start")
		c.Next()
		fmt.Println("Test3 end")
		return nil
	}
}
