//
// Created by wangyang on 2023/2/16.
//

import NIOPosix
import NIOCore
import Dispatch

public final class ChatHandler: ChannelInboundHandler {
    public typealias InboundIn = ByteBuffer
    public typealias OutboundOut = ByteBuffer

    private let channelSyncQueue = DispatchQueue(label: "channelsQueue")
    private var channels: [ObjectIdentifier: Channel] = [:]

    public func channelActive(context: ChannelHandlerContext) {
        let remoteAddress = context.remoteAddress!
        let channel = context.channel
        channelSyncQueue.async { [self] in
            writeToAll(channels: channels, allocator: channel.allocator, message: "(ChatServer) - New client connected with address: \(remoteAddress)\n")
            channels[ObjectIdentifier(channel)] = channel
        }
        var buffer = channel.allocator.buffer(capacity: 64)
        buffer.writeString("(ChatServer) - Welcome to: \(context.localAddress!)\n")
        context.writeAndFlush(wrapOutboundOut(buffer), promise: nil)
    }

    public func channelInactive(context: ChannelHandlerContext) {
        let channel = context.channel
        channelSyncQueue.async { [self] in
            if channels.removeValue(forKey: ObjectIdentifier(channel)) != nil {
                writeToAll(channels: channels, allocator: channel.allocator, message: "(ChatServer) - Client disconnected\n")
            }
        }
    }

    public func channelRead(context: ChannelHandlerContext, data: NIOAny) {
        let id = ObjectIdentifier(context.channel)
        var read = unwrapInboundIn(data)
        var buffer = context.channel.allocator.buffer(capacity: read.readableBytes + 64)
        buffer.writeString("(\(context.remoteAddress!)) - ")
        buffer.writeBuffer(&read)
        channelSyncQueue.async { [self, buffer] in
            writeToAll(channels: channels.filter {  $0.key != id }, buffer: buffer)
        }
    }

    public func errorCaught(context: ChannelHandlerContext, error: Error) {
        print("error:", error)
        context.close(promise: nil)
    }

    private func writeToAll(channels: [ObjectIdentifier: Channel], allocator: ByteBufferAllocator, message: String) {
        let buffer = allocator.buffer(string: message)
        writeToAll(channels: channels, buffer: buffer)
    }

    private func writeToAll(channels: [ObjectIdentifier: Channel], buffer: ByteBuffer) {
        channels.forEach {
            $0.value.writeAndFlush(buffer, promise: nil)
        }
    }
}

extension ChatHandler: @unchecked Sendable {}