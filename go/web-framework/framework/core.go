package framework

import "net/http"

type Core struct {
}

// NewCore 初始化核心框架
func NewCore() *Core {
	return &Core{}
}

// ServerHTTP 实现handler接口
func (c *Core) ServeHTTP(response http.ResponseWriter, request *http.Request) {

}
