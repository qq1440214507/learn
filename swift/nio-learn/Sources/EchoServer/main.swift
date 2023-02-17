//
// Created by wangyang on 2023/1/5.
//

import NIOCore
import NIOPosix

private func runEchoServer() throws {
    let group = MultiThreadedEventLoopGroup(numberOfThreads: System.coreCount)
    let bootstrap = ServerBootstrap(group: group)
            .serverChannelOption(ChannelOptions.backlog, value: 256)
            .serverChannelOption(ChannelOptions.socketOption(.so_reuseaddr), value: 1)
            .childChannelInitializer { channel in
                channel.pipeline.addHandler(BackPressureHandler())
                        .flatMap { v in
                            channel.pipeline.addHandler(EchoHandler())
                        }
            }
            .childChannelOption(ChannelOptions.socketOption(.so_reuseaddr), value: 1)
            .childChannelOption(ChannelOptions.maxMessagesPerRead, value: 16)
            .childChannelOption(ChannelOptions.recvAllocator, value: AdaptiveRecvByteBufferAllocator())
    defer {
        try! group.syncShutdownGracefully()
    }
    let channel = try! bootstrap.bind(host: "127.0.0.1", port: 9955).wait()
    print("Server started and listening on \(channel.localAddress!)")
    try  channel.closeFuture.wait()
    print("Server stopped and listening on \(channel.localAddress!)")
}

try runEchoServer()