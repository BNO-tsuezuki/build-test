'use strict';

var express = require('express');
var controller = require('./matchresult.controller');
var auth = require('../../auth/auth.service');

var router = express.Router();

router.post('/', auth.isAuthenticated(), controller.dealOutReward);

module.exports = router;
