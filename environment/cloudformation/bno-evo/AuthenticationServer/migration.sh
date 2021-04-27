#!/bin/sh

set -eu

DB_NAME=evoauthdb

SQL_PATH=~/tmp/$PACK_NAME/$DB_NAME.sql
TMP_PATH=~/tmp/$PACK_NAME/tmp.sql


                     
cat $SQL_PATH | sed -e '1s/^\xef\xbb\xbf//' | sed -e '1iBEGIN;' -e '$aCOMMIT;' > $TMP_PATH
                      
mysql -u evouser -p -v -h $DB_WRITE_ENDPOINT $DB_NAME < $TMP_PATH
