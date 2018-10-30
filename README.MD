快速使用：将“非热更代码”目录下的东西放到你的内部代码中，用protoc_cs.exe替代原版的protoc.exe。That's all!

protobuf-3.6.1.zip是改动的protoc的源码。

删了反射Google.Protobuf.Reflection。及其副作用，没有了JSON，没有了Any，Option。部分ToString()。

尚未大规模测试，不对质量负责。

群友供献.