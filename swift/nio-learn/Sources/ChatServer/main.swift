//
// Created by wangyang on 2023/2/16.
//

import NIOCore
import NIOPosix
let chatHandler = ChatHandler()
let group = MultiThreadedEventLoopGroup(numberOfThreads: System.coreCount)
let bootstrap = ServerBootstrap(group: group)
        .serverChannelOption(ChannelOptions.backlog, value: 256)
        .serverChannelOption(ChannelOptions.socketOption(.so_reuseaddr), value: 1)
        .childChannelInitializer { channel in
            print("channel")
            return channel.pipeline.addHandler(ByteToMessageHandler(LineDelimiterCodec()))
                    .flatMap { v in
                        print("chatHandler")
                        return channel.pipeline.addHandler(chatHandler)
                    }
        }
        .childChannelOption(ChannelOptions.socketOption(.so_reuseaddr), value: 1)
        .childChannelOption(ChannelOptions.maxMessagesPerRead, value: 16)
        .childChannelOption(ChannelOptions.recvAllocator, value: AdaptiveRecvByteBufferAllocator())
defer {
    try! group.syncShutdownGracefully()
}
let channel = try bootstrap.bind(host: "127.0.0.1", port: 5588).wait()
guard let localAddress = channel.localAddress else {
    fatalError("Address was unable to bind. Please check that the socket was not closed or that the address family was understood.")
}
print("Server started and listening on \(localAddress)")
try channel.closeFuture.wait()
print("Server closed")