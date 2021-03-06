'use strict';

var express = require('express');
var controller = require('./purchase.controller');
var auth = require('../../auth/auth.service');

var router = express.Router();

router.post('/', auth.isAuthenticated(), controller.purchase);

module.exports = router;