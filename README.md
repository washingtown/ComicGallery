# 漫画画廊
个人练手项目，用来将个人收藏的漫画/本子变成网页，方便阅读（就像E站，嗯你懂的）
## 运行环境
- Asp.Net Core 3.1
## 使用说明
### 漫画存放
设置一个专用目录来存放漫画，结构如下：
```
RootDir
   ├── 分类1
   │   ├── 漫画1
   │   ├── 目录1
   │   │     ├── 漫画2
   │   │     ├── 漫画3
   │   │     └── ...
   │   ├── 漫画4
   │   ├── 漫画5
   │   └── ...
   ├── 分类2
   │   ├── 漫画6
   │   ├── 漫画7
   │   └── ...
   └──...
```
项目会按照如下规则扫描目录下的文件：  
- 根目录下的每个直接子目录会被识别为一个“分类”(`Gallery`)
- 分类目录内的每个含图片的目录都会被识别为一篇漫画/本子(`Comic`)
- 漫画/本子目录下的每张图片就是一页
- 请按顺序命名漫画里的每一页
### 配置文件
修改`appsetting.json`文件的`ApplicationSettings`字段：
- `RootDir`: 漫画存放目录
- `DefaultPassword`: 账号默认密码
### 运行
```
cd ComicGallery
dotnet run
```
访问地址 `http://localhost:5000/`
登录系统，默认账户Admin，密码123。  
进入“扫描”页面。点击“开始扫描”，等待扫描完毕。  
欣赏吧！  
### 权限管理
项目有2个角色，Admin和Reader，Admin可以管理账户和扫描漫画，Reader只能看。
## Docker部署
可以将项目打包成Docker镜像，方便部署到NAS上。
### 生成镜像
`docker build -t comicgallery -f ./ComicGallery/Dockerfile .`  
### 导出镜像
`docker save -o comicgallery.tar comicgallery`
### 部署容器
将导出的`comicgallery.tar`导入到服务器中，然后运行  
`docker run -d -p <port>:80 -v <comic_dir>:/data/galleries --name comicgallery comicgallery:latest`  
其中`port`为对外使用的端口，`comic_dir`为漫画存放目录