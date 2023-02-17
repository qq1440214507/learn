//
// Created by wangyang on 2023/2/16.
//
import NIOCore
import NIOPosix
let group = MultiThreadedEventLoopGroup(numberOfThreads: 1)
let bootstrap = ClientBootstrap(group: group)
        // Enable SO_REUSEADDR.
        .channelOption(ChannelOptions.socketOption(.so_reuseaddr), value: 1)
        .channelInitializer { channel in
            channel.pipeline.addHandler(ChatHandler())
        }
defer {
    try! group.syncShutdownGracefully()
}
let channel = try bootstrap.connect(host: "127.0.0.1", port: 5588).wait()
print("ChatClient connected to ChatServer: \(channel.remoteAddress!), happy chatting\n. Press ^D to exit.")
while let line = readLine(strippingNewline: true){
    print(line)
    let buffer = channel.allocator.buffer(string: line)
    try! channel.writeAndFlush(buffer).wait()
}
try! channel.close().wait()

print("ChatClient closed")