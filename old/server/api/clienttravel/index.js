'use strict';

var express = require('express');
var controller = require('./clienttravel.controller');

var router = express.Router();

router.post('/', controller.clientTravel);

module.exports = router;