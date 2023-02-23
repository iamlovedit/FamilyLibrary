docker-compose pull
docker-compose down --volumes --remove-orphans mysql redis seq seqcli
docker-compose up -d