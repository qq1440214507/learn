package main

import (
	"web-framework/framework"
)

func SubjectAddController(c *framework.Context) error {
	c.Text("ok, SubjectAddController")
	return nil
}

func SubjectListController(c *framework.Context) error {
	c.Text("ok, SubjectListController")
	return nil
}

func SubjectDelController(c *framework.Context) error {
	id, _ := c.ParamString("id", "我没有id")
	c.Text("ok, SubjectDelController" + id)
	return nil
}

func SubjectUpdateController(c *framework.Context) error {
	c.Text("ok, SubjectUpdateController")
	return nil
}

func SubjectGetController(c *framework.Context) error {
	id, _ := c.ParamString("id", "我没有id")

	c.Text("ok, SubjectGetController" + id)
	return nil
}

func SubjectNameController(c *framework.Context) error {
	c.Text("ok, SubjectNameController")
	return nil
}
func UserLoginController(c *framework.Context) error {
	c.Text("ok, UserLoginController")
	return nil
}
