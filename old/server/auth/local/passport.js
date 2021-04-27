var passport = require('passport');
var LocalStrategy = require('passport-local').Strategy;
var logger = require('../../logger');

exports.setup = function (User, config) {

  passport.serializeUser(function (user, done) {
    //logger.system.debug('serializeUser [' + user.name + ']');
    done(null, {
      id: user.id,
      name: user.name,
      role: user.role
    });
  });

  passport.deserializeUser(function (user, done) {
    //logger.system.debug('deserializeUser [' + user.name + ']');
    done(null, user);
  });

  passport.use(new LocalStrategy({
      usernameField: 'email',
      passwordField: 'password' // this is the virtual field on the model
    },
    function(email, password, done) {
      User.findOne({
        email: email.toLowerCase()
      }, function(err, user) {
        if (err) return done(err);

        if (!user) {
          return done(null, false, { message: 'This email is not registered.' });
        }
        if (!user.authenticate(password)) {
          return done(null, false, { message: 'This password is not correct.' });
        }

        //logger.system.info('authenticated [' + user.name + ']');

        return done(null, user);
      });
    }
  ));

};
