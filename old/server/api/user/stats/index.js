'use strict';

var express = require('express');
var controller = require('./stats.controller');
var auth = require('../../../auth/auth.service');

var router = express.Router();

router.get('/', auth.isAuthenticated(), controller.show);
router.post('/', auth.isAuthenticated(), controller.create);
router.get('/view', controller.view);
router.get('/view/:id', controller.view);
router.get('/mstable', controller.mstable);

module.exports = router;
