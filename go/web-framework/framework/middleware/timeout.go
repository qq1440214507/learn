package middleware

import (
	"context"
	"fmt"
	"log"
	"time"
	"web-framework/framework"
)

func Timeout(d time.Duration) framework.ControllerHandler {
	return func(c *framework.Context) error {
		// 正常执行完毕
		finishChan := make(chan struct{}, 1)
		// 异常chan
		panicChan := make(chan interface{}, 1)
		durationCtx, cancel := context.WithTimeout(c.BaseContext(), d)
		defer cancel()
		go func() {
			defer func() {
				if p := recover(); p != nil {
					panicChan <- p
				}
			}()
			// 执行业务逻辑
			c.Next()
			finishChan <- struct{}{}
		}()
		// 等待执行结果
		select {
		// 发生panic
		case panicInfo := <-panicChan:
			log.Panicln(panicInfo)
			c.Json(500, "time out panic")
			// 执行成功
		case <-finishChan:
			fmt.Println("finished")
			//超时
		case <-durationCtx.Done():
			c.SetHasTimeout()
			c.Json(500, "time out ")

		}
		return nil
	}
}
