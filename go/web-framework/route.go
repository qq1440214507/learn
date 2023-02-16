package main

import "web-framework/framework"

func registerRouter(core *framework.Core) {
	core.Get("foo", FooControllerHandler)
}
