#!/bin/sh

set -eu


AtExit() {
  [[ -n ${TMP_DBCONF_FILE-} ]] && rm -f "$TMP_DBCONF_FILE"
}
trap AtExit EXIT
trap 'rc=$?; trap - EXIT; AtExit; exit $?' INT PIPE TERM

TMP_DBCONF_FILE=$(mktemp "/tmp/${0##*/}.tmp.XXXXXX")

read -sp "Enter database password:" DB_PASS
echo 
echo "[Client]" > $TMP_DBCONF_FILE
echo "user = evouser" >>  $TMP_DBCONF_FILE
echo "password = $DB_PASS" >> $TMP_DBCONF_FILE


DOWNLOADS_PATH=~/tmp/$PACK_NAME/downloads
mkdir -p $DOWNLOADS_PATH

aws s3 cp s3://$BUILDPACKAGES_BUCKETNAME/$PACK_NAME/$DB_NAME.sql $DOWNLOADS_PATH/$DB_NAME.sql

# cat $DOWNLOADS_PATH/tmp.sql | sed -e '1s/^\xef\xbb\xbf//' | sed -e '1iBEGIN;' -e '$aCOMMIT;' > $DOWNLOADS_PATH/$DB_NAME.sql
# rm $DOWNLOADS_PATH/tmp.sql

mysql --defaults-extra-file=$TMP_DBCONF_FILE -v -h $DB_WRITE_ENDPOINT < $DOWNLOADS_PATH/$DB_NAME.sql
