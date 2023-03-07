package main

import (
	"context"
	"log"
	"net/http"
	"os"
	"os/signal"
	"syscall"
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
	go func() {
		server.ListenAndServe()
	}()
	quit := make(chan os.Signal)
	signal.Notify(quit, syscall.SIGINT, syscall.SIGTERM, syscall.SIGQUIT)
	<-quit
	// 优雅关闭
	if err := server.Shutdown(context.Background()); err != nil {
		log.Fatal("Server Shutdown:", err)
	}
}
