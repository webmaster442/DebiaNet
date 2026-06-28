#!/bin/bash
docker pull plantuml/plantuml-server:jetty
docker run -d -p 8080:8080 plantuml/plantuml-server:jetty
echo "PlantUML server is running on http://localhost:8080"
echo "Press a key to stop the server..."
read -n 1 -s
docker stop $(docker ps -q --filter ancestor=plantuml/plantuml-server:jetty)