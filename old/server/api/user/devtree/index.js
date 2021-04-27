'use strict';

var express = require('express');
var controller = require('./devtree.controller');
var auth = require('../../../auth/auth.service');

var router = express.Router();

router.get('/mobilesuits', auth.isAuthenticated(), controller.mobilesuits);
router.post('/develop', auth.isAuthenticated(), controller.developDevTreeNode);
router.post('/purchase', auth.isAuthenticated(), controller.purchaseDevTreeNode);
router.post('/unlock', auth.isAuthenticated(), controller.purchaseDevTreeNode);
router.post('/unlockall', auth.isAuthenticated(), controller.unlockDevTreeNodeAll);

module.exports = router;
