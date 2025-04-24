#!/bin/bash

print_help() {
  echo "Usage: ./run.sh key=value ...

Required keys:
  db_user        SQL Server username (e.g., sa)
  db_password    SQL Server password
  db_name        Database name (e.g., FilesDb)
  app_port       Host port to expose the app (e.g., 5201)

Example:
  ./run.sh db_user=sa db_password=myPass db_name=FilesDb app_port=5201
"
}

# Set default
DB_PORT=1433

# Parse key=value args
for ARG in "$@"; do
  case $ARG in
    db_user=*) DB_USER="${ARG#*=}" ;;
    db_password=*) DB_PASSWORD="${ARG#*=}" ;;
    db_name=*) DB_NAME="${ARG#*=}" ;;
    app_port=*) APP_PORT="${ARG#*=}" ;;
    -h|--help) print_help; exit 0 ;;
    *) echo "❌ Unknown argument: $ARG"; print_help; exit 1 ;;
  esac
done

# Validate required
if [[ -z "$DB_USER" || -z "$DB_PASSWORD" || -z "$DB_NAME" || -z "$APP_PORT" ]]; then
  echo "❌ Missing required arguments."
  print_help
  exit 1
fi

export DB_USER DB_PASSWORD DB_NAME DB_PORT APP_PORT
docker-compose up --build
