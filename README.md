# micro-store

A simple microservice solution to learn more about Docker, AWS ECR and AWS ECS.


----------------------------------------------------------------------------------
## Build the docker images

```
docker build --rm -t microstore/account.api:latest .
docker build --rm -t microstore/inventory.api:latest .
docker build --rm -t microstore/store.api:latest .
```


----------------------------------------------------------------------------------
## Check the image was created

```
docker image ls
```

----------------------------------------------------------------------------------
## Run the container

> NOTE: `-p` is mapping the port. `-e` is setting environment variables (overriding the default ASP.NET Core settings).

```
docker run --name account.api --rm -p 7000:7000 -p 7001:7001 -e ASPNETCORE_HTTP_PORT=https://+:7001 -e ASPNETCORE_URLS=http://+:7000 microstore/account.api

docker run --name inventory.api --rm -p 7002:7002 -p 7003:7003 -e ASPNETCORE_HTTP_PORT=https://+:7003 -e ASPNETCORE_URLS=http://+:7002 microstore/inventory.api

docker run --name store.api --rm -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 microstore/store.api

```

----------------------------------------------------------------------------------
## Check in the browser

http://localhost:5000/store

----------------------------------------------------------------------------------
## Stop the container

```
docker ps
docker container stop <CONTAINER ID>
```

----------------------------------------------------------------------------------