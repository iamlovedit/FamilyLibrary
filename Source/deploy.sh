COMPOSE_FILE=./docker-compose.prod.yml

docker-compose -f $COMPOSE_FILE pull
docker-compose -f $COMPOSE_FILE down
docker-compose -f $COMPOSE_FILE up -d
