# Mythonian

Mythonian 是一个 **平台跳跃ARPG** 项目  
采用 **2D侧视角**, 和 **像素风** 的贴图

## ---> [Wiki](https://github.com/MythoniaTeam/Mythonian-Wiki/wiki) <---

## Git Commit 信息 规范

```[TYPE](SCOPE): 概要 (#3, #4, close #1, fix #2)  ```

后面的`: `是 `英文冒号` + `空格`  

### TYPE 类型
- `feat`: 新的特性 / 文件
- `fix`: 修复错误
- `perf`: 优化程序 (提升性能等)
- `test`: 添加测试用的程序 / 文件
- `style`: 格式改动 (不影响程序效果的改动)
- `doc`s: 添加文档 / 注释

- `other`: 不容易归类, 不重要的小型commit

列表中靠上的类型优先级较高 (若同时包含多种更改, 选取靠上的)

### SCOPE类型
- 留空 / `Framework`: 框架相关
- `Content`: 游戏元素相关
- `UX`: 游戏的操作和用户体验相关
- `UI`: 用户界面相关

### Issue
相关的Issue应该用 英文括号 `()` 括起来  
Issue之间应该用 英文逗号 + 空格 `, ` 分隔  

可以输入 `close` `fix` `resolve` + `#编号`, 在相应commit合并到主分支时, 相应issue会自动关闭  

### Merge
合并分支的commit信息应该如下格式
```[merge-TYPE](SCOPE) FROM-BRANCH => TO-BRANCH: 概要(可留空) (#5)```

TYPE为该分支所有更改的类型概要
FROM-BRANCH 为合并的来源分支
TO-BRANCH 为合并的目标分支

其余项目和一般Commit类似
