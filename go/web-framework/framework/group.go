package framework

type IGroup interface {
	Get(string, ControllerHandler)
	Post(string, ControllerHandler)
	Put(string, ControllerHandler)
	Delete(string, ControllerHandler)
	Group(string) IGroup
}
type Group struct {
	core   *Core
	prefix string
	parent *Group
}

func NewGroup(core *Core, prefix string) *Group {
	return &Group{
		core:   core,
		prefix: prefix,
		parent: nil,
	}
}

func (g *Group) Get(uri string, handler ControllerHandler) {
	g.core.Get(g.prefix+uri, handler)
}

func (g *Group) Post(uri string, handler ControllerHandler) {
	g.core.Post(g.prefix+uri, handler)

}

func (g *Group) Put(uri string, handler ControllerHandler) {
	g.core.Put(g.prefix+uri, handler)

}

func (g *Group) Delete(uri string, handler ControllerHandler) {
	g.core.Delete(g.prefix+uri, handler)

}

// Group 实现Group方法
func (g *Group) Group(uri string) IGroup {
	cgroup := NewGroup(g.core, uri)
	cgroup.parent = g
	return cgroup
}
