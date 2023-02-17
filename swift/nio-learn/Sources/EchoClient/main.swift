//
// Created by wangyang on 2023/2/16.
//

import NIOCore
import NIOPosix
print("Please enter line to send to the server")
let line = readLine(strippingNewline: true)!
let group = MultiThreadedEventLoopGroup(numberOfThreads: 1)
let bootstrap = ClientBootstrap(group: group)
        .channelOption(ChannelOptions.socketOption(.so_reuseaddr), value: 1)
        .channelInitializer { channel in
            channel.pipeline.addHandler(EchoHandler(line: line))
        }
defer {
    try! group.syncShutdownGracefully()
}

let channel = try bootstrap.connect(host: "127.0.0.1", port: 9955).wait()
try channel.closeFuture.wait()
print("channel closed successfully")