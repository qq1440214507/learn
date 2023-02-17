// swift-tools-version: 5.7
// The swift-tools-version declares the minimum version of Swift required to build this package.

import PackageDescription

let package = Package(
        name: "nioStudy",
        platforms: [
            .macOS(.v12)
        ],
        dependencies: [
            // Dependencies declare other packages that this package depends on.
            // .package(url: /* package url */, from: "1.0.0"),
            .package(url: "https://github.com/apple/swift-nio.git", from: "2.0.0")
        ],
        targets: [
            .executableTarget(name: "EchoServer", dependencies: [.product(name: "NIOCore", package: "swift-nio"),
                                                                 .product(name: "NIOPosix", package: "swift-nio"),
                                                                 .product(name: "NIOHTTP1", package: "swift-nio")]),
            .executableTarget(name: "EchoClient", dependencies: [.product(name: "NIOCore", package: "swift-nio"),
                                                                 .product(name: "NIOPosix", package: "swift-nio"),
                                                                 .product(name: "NIOHTTP1", package: "swift-nio")]),
            .executableTarget(name: "ChatServer", dependencies: [.product(name: "NIOCore", package: "swift-nio"),
                                                                 .product(name: "NIOPosix", package: "swift-nio"),
                                                                 .product(name: "NIOHTTP1", package: "swift-nio")]),
            .executableTarget(name: "ChatClient", dependencies: [.product(name: "NIOCore", package: "swift-nio"),
                                                                 .product(name: "NIOPosix", package: "swift-nio"),
                                                                 .product(name: "NIOHTTP1", package: "swift-nio")])

        ]
)
