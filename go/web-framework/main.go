package main

import (
	"net/http"
	"web-framework/framework"
	"web-framework/framework/middleware"
	_ "web-framework/framework/middleware"
)

func main() {
	core := framework.NewCore()
	core.Use(middleware.Cost())
	core.Use(middleware.Recovery())
	core.Use(Test1(), Test2())
	registerRouter(core)
	server := &http.Server{
		Handler: core,
		Addr:    "127.0.0.1:8080",
	}
	server.ListenAndServe()
}
