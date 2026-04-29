#!/bin/bash

REBUILD=""
SERVICE=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --rebuild)
      REBUILD="--build --force-recreate"
      shift
      ;;
    *)
      SERVICE="$1"
      shift
      ;;
  esac
done

if [ -z "$SERVICE" ]; then
  if [ -n "$REBUILD" ]; then
    echo "Stopping all services..."
    docker compose -f docker-compose.kafka.yml -f docker-compose.yml down
    echo "Starting all services with rebuild..."
  else
    echo "Starting all services..."
  fi
  docker compose -f docker-compose.kafka.yml -f docker-compose.yml up -d --remove-orphans $REBUILD
else
  echo "Rebuilding service: $SERVICE"
  docker compose -f docker-compose.kafka.yml -f docker-compose.yml up -d --remove-orphans --build --force-recreate "$SERVICE"
fi