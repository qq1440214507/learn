package main

import (
	"context"
	"fmt"
	"log"
	"time"
	"web-framework/framework"
)

func FooControllerHandler(ctx *framework.Context) error {
	print("进入")
	finish := make(chan struct{}, 1)
	panicChan := make(chan interface{}, 1)
	durationCtx, cancel := context.WithTimeout(ctx.BaseContext(), time.Second)
	defer cancel()

	go func() {
		// 异常处理
		defer func() {
			if p := recover(); p != nil {
				panicChan <- p
			}
		}()
		time.Sleep(10 * time.Second)
		ctx.Json(200, "ok")
		finish <- struct{}{}
	}()

	select {
	case p := <-panicChan:
		// 防止其他Goroutine写入乱序
		ctx.WriterMux().Lock()
		defer ctx.WriterMux().Unlock()
		log.Println(p)
		ctx.Json(500, "panic")
	case <-finish:
		fmt.Println("finish")
	case <-durationCtx.Done():
		// 防止其他Goroutine写入乱序
		ctx.WriterMux().Lock()
		defer ctx.WriterMux().Unlock()
		ctx.Json(500, "time out")
		ctx.SetHasTimeout()
	}
	return nil

}
