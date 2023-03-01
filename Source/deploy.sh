COMPOSE_FILE=./docker-compose.prod.yml
export DOCKER_REGISTRY=registry.cn-chengdu.aliyuncs.com/galaservices/
docker-compose -f $COMPOSE_FILE pull
docker-compose -f $COMPOSE_FILE down --rmi local
docker-compose -f $COMPOSE_FILE up -d
