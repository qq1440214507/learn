package framework

type IGroup interface {
	Get(string, ...ControllerHandler)
	Post(string, ...ControllerHandler)
	Put(string, ...ControllerHandler)
	Delete(string, ...ControllerHandler)
	Group(string) IGroup
	Use(middlewares ...ControllerHandler)
}
type Group struct {
	core        *Core
	prefix      string
	parent      *Group
	middlewares []ControllerHandler
}

func (g *Group) Use(middlewares ...ControllerHandler) {
	g.middlewares = append(g.middlewares, middlewares...)
}

func NewGroup(core *Core, prefix string) *Group {
	return &Group{
		core:   core,
		prefix: prefix,
		parent: nil,
	}
}
func (g *Group) getMiddlewares() []ControllerHandler {
	if g.parent == nil {
		return g.middlewares
	}
	return append(g.parent.getMiddlewares(), g.middlewares...)
}
func (g *Group) Get(uri string, handlers ...ControllerHandler) {
	allHandlers := append(g.middlewares, handlers...)
	g.core.Get(g.prefix+uri, allHandlers...)
}

func (g *Group) Post(uri string, handlers ...ControllerHandler) {
	allHandlers := append(g.middlewares, handlers...)
	g.core.Post(g.prefix+uri, allHandlers...)

}

func (g *Group) Put(uri string, handlers ...ControllerHandler) {
	allHandlers := append(g.middlewares, handlers...)
	g.core.Put(g.prefix+uri, allHandlers...)

}

func (g *Group) Delete(uri string, handlers ...ControllerHandler) {
	allHandlers := append(g.middlewares, handlers...)
	g.core.Delete(g.prefix+uri, allHandlers...)

}

// Group 实现Group方法
func (g *Group) Group(uri string) IGroup {
	cGroup := NewGroup(g.core, uri)
	cGroup.parent = g
	return cGroup
}
