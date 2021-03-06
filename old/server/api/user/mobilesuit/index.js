'use strict';

var express = require('express');
var controller = require('./mobilesuit.controller');
var auth = require('../../../auth/auth.service');

var router = express.Router();

router.get('/', auth.isAuthenticated(), controller.index);
router.get('/:id', auth.isAuthenticated(), controller.show);
router.post('/', auth.isAuthenticated(), controller.create);
router.post('/exp', auth.isAuthenticated(), controller.addExp);
router.put('/:id', auth.isAuthenticated(), controller.update);
router.put('/:id/inventory', auth.isAuthenticated(), controller.updateInventory);
router.patch('/:id', auth.isAuthenticated(), controller.update);
router.delete('/:id', auth.isAuthenticated(), controller.destroy);

module.exports = router;
