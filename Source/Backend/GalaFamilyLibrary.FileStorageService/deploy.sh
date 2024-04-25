container=file_storage
image=registry.cn-chengdu.aliyuncs.com/galaservices/file_storage:latest

docker stop container
docker rm container
docker rmi image
docker pull image

docker run -d --name container --restart=always -p 8001:80 -v /usr/local/docker/familiyLibrary/files:/app/files