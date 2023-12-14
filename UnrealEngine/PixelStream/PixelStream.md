# PixelStream

> UE 4.27

[官方文档](https://docs.unrealengine.com/4.27/zh-CN/SharingAndReleasing/PixelStreaming/)
像素流送：在云端服务器上运行虚幻引擎应用程序，通过WebRTC将渲染的帧和音频流送到浏览器和移动设备。

通过像素流送可将打包的虚幻引擎应用程序在桌面PC或云端服务器上运行，也可包含少量虚幻引擎中自带的网络服务。使用者通过任意现代网络浏览器进行连接（电脑版或移动版），并从虚幻引擎应用程序流送渲染的帧和音频。不需要使用者安装或下载其他内容。操作类似于从YouTube或Netflix下载一个视频，区别是使用者可使用键盘、鼠标、触控输入，甚至在播放器网页中创建的自定义HTML5 UI来与应用程序进行交互。

参考文章：
<https://zhuanlan.zhihu.com/p/566593290>
<https://zhuanlan.zhihu.com/p/76406905>
<https://zhuanlan.zhihu.com/p/383825174>

坑：

1. 多实例局域网自动分配像素流，N卡上限开3个，可黑科技突破
2. 运行setup.ps1安装所有依赖项，Win10可能会报错无法运行脚本，管理员权限打开PowerShell，输入 Set-ExecutionPolicy RemoteSigned，选择A/Y开启

提炼：

1. 写.bat批处理来执行各个信令和exe
2. RenderServerManager是干嘛用的？
