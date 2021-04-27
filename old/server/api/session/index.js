'use strict';

var express = require('express');
var controller = require('./session.controller');
var auth = require('../../auth/auth.service');

var router = express.Router();

router.get('/', controller.sessions);
router.get('/waitings', controller.waitings);

router.post('/', auth.isAuthenticated(), controller.sessionStart);
router.delete('/', auth.isAuthenticated(), controller.sessionEnd);
router.post('/keepalive', auth.isAuthenticated(), controller.sessionKeepalive);

router.post('/matchmaking', auth.isAuthenticated(), controller.matchmakingStart);
router.get('/matchmaking', auth.isAuthenticated(), controller.matchmakingEntry);
router.delete('/matchmaking', auth.isAuthenticated(), controller.matchmakingCancel);
router.post('/matchmaking/force', /*auth.isAuthenticated(),*/ controller.matchmakingForce);
router.post('/matchmaking/changemembermax', auth.isAuthenticated(), controller.matchmakingChangeMemberMax);

module.exports = router;
