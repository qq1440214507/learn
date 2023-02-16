package framework

import (
	"log"
	"net/http"
)

type Core struct {
	router map[string]ControllerHandler
}

// NewCore 初始化核心框架
func NewCore() *Core {
	return &Core{router: map[string]ControllerHandler{}}
}
func (c *Core) Get(url string, handler ControllerHandler) {
	c.router[url] = handler
}

// ServerHTTP 实现handler接口
func (c *Core) ServeHTTP(response http.ResponseWriter, request *http.Request) {
	log.Println("core.ServeHTTP")
	ctx := NewContext(request, response)
	router := c.router["foo"]
	if router == nil {
		return
	}
	log.Println("core.Router")
	router(ctx)
}
