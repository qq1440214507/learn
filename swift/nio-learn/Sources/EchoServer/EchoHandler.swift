//
// Created by wangyang on 2023/1/23.
//

import NIOCore
public final class EchoHandler:ChannelInboundHandler{
    public typealias InboundIn = ByteBuffer
    public typealias OutboundOut = ByteBuffer

    public func channelRead(context: ChannelHandlerContext, data: NIOAny) {
        print("读取消息\(data)")
        context.write(data, promise: nil)
    }

    public func channelReadComplete(context: ChannelHandlerContext) {
        print("消息读取完毕")
        context.flush()
    }

    public func errorCaught(context: ChannelHandlerContext, error: Error) {
        print("error:",error)
        context.close(promise: nil)
    }
}