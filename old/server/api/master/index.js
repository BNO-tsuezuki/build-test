'use strict';

var express = require('express');
var controller = require('./master.controller');

var router = express.Router();

router.get('/:target', controller.index);
router.get('/:target/:id', controller.show);

module.exports = router;