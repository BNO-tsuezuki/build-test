'use strict';

var express = require('express');
var controller = require('./masterserver.controller');
var auth = require('../../auth/auth.service');

var router = express.Router();

router.get('/', auth.isAuthenticated(), controller.index);
router.post('/', auth.isAuthenticated(), controller.registerServer);
router.post('/keepalive', auth.isAuthenticated(), controller.keepalive);
router.delete('/', auth.isAuthenticated(), controller.unregisterServer);


module.exports = router;
