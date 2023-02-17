//
// Created by wangyang on 2023/2/16.
//

import NIOCore

public final class LineDelimiterCodec:ByteToMessageDecoder{
    public typealias InboundOut = ByteBuffer
    public typealias Outbound = ByteBuffer
    let newLine = "%".utf8.first!

    public func decode(context: ChannelHandlerContext, buffer: inout ByteBuffer) throws -> DecodingState {
        print("hello")
        let readable = buffer.withUnsafeReadableBytes { $0.firstIndex(of: newLine)  }
        if let r = readable{
            print("hello2")
            context.fireChannelRead(wrapInboundOut(buffer.readSlice(length: r+1)!))
            return .continue
        }
        return .needMoreData
    }
}