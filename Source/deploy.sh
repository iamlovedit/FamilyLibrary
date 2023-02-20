# shellcheck disable=SC2054
array=(library_gateway,dynamo_package,family_library,library_identity)
for ((i = 0; i < ${#array[@]}; i++)); do
  docker stop ${array[i]}
  docker rm ${array[i]}
  docker rmi ${DOCKER_REGISTRY}/${array[i]}
done

docker-compose -f docker-compose.prod.yml up -d
