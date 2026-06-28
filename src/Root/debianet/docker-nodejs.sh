#!/bin/bash
docker pull node:26-slim
docker run -it --rm --entrypoint sh node:26-slim