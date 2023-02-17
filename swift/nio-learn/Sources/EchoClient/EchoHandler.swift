//
// Created by wangyang on 2023/2/16.
//

import NIOCore
import NIOPosix
public final class EchoHandler: ChannelInboundHandler {
    let line:String
    public init(line: String) {
        self.line = line
    }

    public typealias InboundIn = ByteBuffer
    public typealias OutboundOut = ByteBuffer

    private var sendBytes = 0
    private var receiveBuffer: ByteBuffer = ByteBuffer()

    public func channelActive(context: ChannelHandlerContext) {
        print("Client connected to \(context.remoteAddress!)")
        let buffer = context.channel.allocator.buffer(string:line)
        sendBytes = buffer.readableBytes
        context.writeAndFlush(wrapOutboundOut(buffer), promise: nil)
    }

    public func channelRead(context: ChannelHandlerContext, data: NIOAny) {
        var unwrappedInboundData = unwrapInboundIn(data)
        sendBytes -= unwrappedInboundData.readableBytes
        receiveBuffer.writeBuffer(&unwrappedInboundData)
        if sendBytes == 0 {
            let string = String(buffer: receiveBuffer)
            print("Received: '\(string)' back from the server, closing channel.")
            context.close(promise: nil)
        }
    }

    public func errorCaught(context: ChannelHandlerContext, error: Error) {
        print("error:",error)
        context.close(promise: nil)
    }
}
