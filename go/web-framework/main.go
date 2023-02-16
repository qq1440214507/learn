package main

import (
	"net/http"
	"web-framework/framework"
)

func main() {
	core := framework.NewCore()
	registerRouter(core)
	server := &http.Server{
		Handler: core,
		Addr:    "127.0.0.1:8080",
	}
	server.ListenAndServe()
}
