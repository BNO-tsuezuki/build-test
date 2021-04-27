#!/bin/sh

set -eu

aws deploy create-deployment \
--application-name $APPLICATION_NAME \
--deployment-group-name $SERVICE \
--region $REGION \
--s3-location bundleType=tgz,key=$PACK_NAME/$SERVICE.tar.gz,bucket=$BUILDPACKAGES_BUCKETNAME
