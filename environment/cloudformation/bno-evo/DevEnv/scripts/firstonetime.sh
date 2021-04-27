#!/bin/sh

set -eu

SQL_CMD="mysql -u evouser -p -v -h $DB_WRITE_ENDPOINT"

# $SQL_CMD << EOF
# CREATE DATABASE IF NOT EXISTS evoaccountdb;
# CREATE DATABASE IF NOT EXISTS evogamedb;
# EOF
