package middleware

import (
	"log"
	"time"
	"web-framework/framework"
)

// Cost  统计方法执行了多久时间
func Cost() framework.ControllerHandler {
	return func(c *framework.Context) error {
		start := time.Now()
		c.Next()
		end := time.Now()
		cost := end.Sub(start)
		log.Printf("api uri:%s coast:%v", c.GetRequest().RequestURI, cost)
		return nil
	}
}
