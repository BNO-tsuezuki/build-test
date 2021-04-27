/**
 * Main application routes
 */

'use strict';

var errors = require('./components/errors');

module.exports = function(app) {

  // Insert routes below
  app.use('/api/sessions', require('./api/session'));
  app.use('/api/gamestats', require('./api/gamestat'));
  app.use('/api/performancestats', require('./api/performancestat'));
  app.use('/api/masterserver', require('./api/masterserver'));
  app.use('/api/matchresult', require('./api/matchresult'));
  app.use('/api/user/mobilesuits', require('./api/user/mobilesuit'));
  app.use('/api/user/devtree', require('./api/user/devtree'));
  app.use('/api/user/stats', require('./api/user/stats'));
  app.use('/api/master', require('./api/master'));
  app.use('/api/shop/purchase', require('./api/purchase'));
  app.use('/api/clienttravel', require('./api/clienttravel'));
  app.use('/api/things', require('./api/thing'));
  app.use('/api/users', require('./api/user'));

  app.use('/auth', require('./auth'));

  // All undefined asset or api routes should return a 404
  app.route('/:url(api|auth|components|app|bower_components|assets)/*')
   .get(errors[404]);

  // All other routes should redirect to the index.html
  app.route('/*')
    .get(function(req, res) {
      res.sendfile(app.get('appPath') + '/index.html');
    });
};
