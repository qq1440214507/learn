package framework

import (
	"errors"
	"strings"
)

type Tree struct {
	root *node
}
type node struct {
	isLast  bool
	segment string
	handler ControllerHandler
	childs  []*node
}

// 判断是否 : 开通  通用匹配
func isWildSegment(segment string) bool {
	return strings.HasPrefix(segment, ":")
}

// 获取满足匹配规则的子节点
func (n *node) filterChildNodes(segment string) []*node {
	if len(n.childs) == 0 {
		return nil
	}
	if isWildSegment(segment) {
		return n.childs
	}
	nodes := make([]*node, 0, len(n.childs))
	for _, childNode := range n.childs {
		if isWildSegment(childNode.segment) {
			nodes = append(nodes, childNode)
		} else if childNode.segment == segment {
			nodes = append(nodes, childNode)
		}
	}
	return nodes
}

// 判断路由是否存在树中
func (n *node) matchNode(uri string) *node {
	segments := strings.SplitN(uri, "/", 2)
	segment := segments[0]

	if !isWildSegment(segment) {
		segment = strings.ToUpper(segment)
	}
	childNodes := n.filterChildNodes(segment)
	if childNodes == nil || len(childNodes) == 0 {
		return nil
	}

	if len(segments) == 1 {
		for _, tn := range childNodes {
			if tn.isLast {
				return tn
			}
		}
		return nil
	}

	for _, tn := range childNodes {
		matchNode := tn.matchNode(segments[1])
		if matchNode != nil {
			return matchNode
		}
	}

	return nil

}
func newNode() *node {
	return &node{
		isLast:  false,
		segment: "",
		childs:  []*node{},
	}
}
func (tree *Tree) AddRouter(uri string, handler ControllerHandler) error {
	n := tree.root
	// 路由冲突
	if n.matchNode(uri) != nil {
		return errors.New("routes exist:" + uri)
	}
	segments := strings.Split(uri, "/")
	for index, segment := range segments {
		if !isWildSegment(segment) {
			segment = strings.ToUpper(segment)
		}
		isLast := index == len(segments)-1
		var objNode *node
		childNodes := n.filterChildNodes(segment)
		if len(childNodes) > 0 {
			for _, childNode := range childNodes {
				if childNode.segment == segment {
					objNode = childNode
					break
				}
			}
		}
		if objNode == nil {
			childNode := newNode()
			childNode.segment = segment
			if isLast {
				childNode.isLast = true
				childNode.handler = handler
			}
			n.childs = append(n.childs, childNode)
			objNode = childNode
		}
		n = objNode
	}
	return nil
}

func (tree *Tree) FindHandler(uri string) ControllerHandler {
	matchNode := tree.root.matchNode(uri)
	if matchNode == nil {
		return nil
	}
	return matchNode.handler
}

func NewTree() *Tree {
	root := newNode()
	return &Tree{
		root: root,
	}
}
