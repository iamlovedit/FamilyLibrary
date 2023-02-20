gatewaycontainer=library_gateway
packagecontainer=dynamo_package
familycontainer=family_library
identityContainer=library_identity

docker stop $gatewaycontainer
docker stop $packagecontainer
docker stop $familycontainer
docker stop $identityContainer

docker rm $gatewaycontainer
docker rm $packagecontainer
docker rm $familycontainer
docker rm $identityContainer

docker rmi ${DOCKER_REGISTRY}library_gateway
docker rmi ${DOCKER_REGISTRY}dynamo_package
docker rmi ${DOCKER_REGISTRY}family_library
docker rmi ${DOCKER_REGISTRY}library_identity

docker-compose -f docker-compose.prod.yml up -d