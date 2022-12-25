# ndp-starter-kit
Starter Template for NextJS + .NET7 + PostgresSQL (also with docker).
<br>

## Overview

This is a starter template for an application using:
1. NextJS frontend - with typescript, [react-query](https://tanstack.com/query/), [sass](https://sass-lang.com/)
2. .NET 7 webapi - with [efcore](https://docs.microsoft.com/en-us/ef/core/)
3. PostgreSQL

The app has a basic CRD demo/example linking all the parts together which you can easily spin up with docker compose.

<br>

## How to Run

### Running everything with Docker

Ensure you have Docker with Docker Compose installed.

1. Create the network "my_network": ```docker network create my_network```
2. Docker compose up everything: ```docker compose up -d```
3. See website on ```http://localhost:3000```
4. If you want to specify dev environment: ```docker compose -f docker-compose.dev.yml```

### Only Database in Docker

You can run only the DB container in dev if you want to run the other code outside containers with:
```docker compose up db -d```

The frontend/api is standard and you can start them with
```npm install``` ```npm run dev```  and  ```dotnet run```
(assuming of course you have node/npm and the dotnet cli installed)


### Database outside

If you want to run the db outside of docker, you will need to have postgres installed locally.
- Ensure the user/password combination inside the .env file is created.
- Ensure that the database "Notes" is created.


<br>

## Next Steps

If you are using this template, ensure that:
- You remove secrets from source control.
  - db credentials in .env
  - db connection string with credentials in ```dotnet-webapi/appsettings.json```
- Adjust the URLS in"
  - .env file inside ```nextjs-frontend```
  - docker-compose and dockerfiles
  - CORS setting in ```dotnet-webapi/Program.cs```
- Add SSL/HTTPS.
- Remember to [migrate](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) any database model changes.
- Rename the project!

 <br>

 ## Note on Secrets/Environment files/variables and docker

 Secrets are *not* completely safe as env variables in docker. Anyone with enough privileges to access the container can inspect and see them.
 It is recommended to use [Docker Secrets](https://docs.docker.com/engine/swarm/secrets/) or some other form of security like Azure key vault to keep
 sensitive information.

 .env files are more useful for defaults like API URLS.

 #### Side Note:
 You will notice that the API_URL for the frontend is added in 2 places. In the ```docker-compose``` files and in the ```.env``` within ```nextjs-frontend```.
 The reason being if you start the frontend and webapi without docker, you cannot access the webapi from the frontend with "http",
 so it uses the "https" URL from the .env *within* ```nextjs-frontend```in that scenario.


<br>

## Sample nginx config file:

Remember to replace the API url for the frontend and add your site to CORS in the webapi.

```
server {
        listen 80;
        listen [::]80;

        server_name my_server_name;

        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;

        location / {
                proxy_pass http://localhost:3000/;
        }

        location /_next/static/ {
                proxy_pass http://localhost:3000/_next/static/;
        }

        location /api/ {
                proxy_pass http://localhost:5000/;
        }
}
```
