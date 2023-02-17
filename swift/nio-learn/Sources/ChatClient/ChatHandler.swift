//
// Created by wangyang on 2023/2/16.
//

import NIOCore
import NIOPosix
final class ChatHandler:ChannelInboundHandler{
    typealias InboundIn = ByteBuffer
    typealias OutboundOut = ByteBuffer

    private func printByte(_ byte:UInt8){
        fputc(Int32(byte),stdout)
    }

    func channelRead(context: ChannelHandlerContext, data: NIOAny) {
        var buffer = unwrapInboundIn(data)
        while let byte:UInt8 = buffer.readInteger(){
            printByte(byte)
        }
    }

    func errorCaught(context: ChannelHandlerContext, error: Error) {
        print("error:",error)
        context.close(promise: nil)
    }

}
