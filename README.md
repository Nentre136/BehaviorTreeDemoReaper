# BehaviorTreeDemoReaper

# BehaviorTreeDemoReaper

​	该项目演示内容是对怪物AI行为树的基础设计，使用了BehaviorDesigner行为树插件进行设计。并尝试全逻辑热更新对外部插件导入的可能。

​	内容部分对Boss的AI行为逻辑做了演示，动画机使用的是U3D的原生动画机做的状态转换。玩家的基础视野、移动、闪避跳跃等基础控制，以及Boss行为对玩家的轻度反馈；一个简单的加载页面

​	热更新部分采用AssetBundle和HybridCLR互相结合的资源逻辑热更新。相比于xlua只能更新游戏的脚本逻辑和配置，不能更新C#端的框架，HybridCLR有着对全平台C#热更的优势，且能够对框架本身进行热更。有着不小的优势。

​	使用方式：场景选择 `MainScene` 后点击 `Play` 运行即可







· BehaviorDesigner

· HybridCLR

· AssetBundle