'use strict';

var _ = require('lodash');
var async = require('async');
var User = require('../user.model');
var UserMobilesuit = require('../mobilesuit/mobilesuit.model');
var Master = require('../../../config/master');
var ms = require('../mobilesuit/mobilesuit.controller');
var logger = require('../../../logger');


var respondOK = function(res, userId, nodeId, next) {
  logger.system.info('devtree_ctrl: success user:' + userId + ' node:' + nodeId);
  if (typeof next === 'function') {
    return next();
  }
  return res.status(201).send('OK');
};

var validationError = function(res, statusCode, err, next) {
  logger.system.info('devtree_ctrl: ' + err);
  if (typeof next === 'function') {
    return next();
  }
  return res.status(statusCode).send(err);
};

var isRootNode = function(treeNode) {
  return (_.isEmpty(treeNode.parent));
};

var isDevelopedNode = function(user, nodeId) {
  var Idx = _.findIndex(user.developedDevTree, function (nd) { return (nd === nodeId) });
  if (Idx >= 0) {
    return true;
  }
  return false;
};

var isPurchasedNode = function(user, nodeId) {
  var Idx = _.findIndex(user.purchasedDevTree, function (nd) { return (nd === nodeId) });
  if (Idx >= 0) {
    return true;
  }
  return false;
};

// ツリーノードの開発条件チェック
function developNodeRequest (res, user, nodeId, next) {
  Master.devtree.findById(nodeId, function findTreeNodeRes (err, treeNode) {
    if (err) return handleError(res, err, next);
    if (!treeNode) return validationError(res, 404, 'cant develop, notfound node', next);

    if (!isRootNode(treeNode)) {
      var length = treeNode.parent.length;
      // 親が複数ある場合は、全て開発されていないと開発不可
      for (var i=0; i<length; i++) {
        if (!isDevelopedNode(user, treeNode.parent[i])) return validationError(res, 400, 'cant develop, not develop the parent node (' + treeNode.parent[i] + ')', next);
      }
    }
    if (isDevelopedNode(user, treeNode.Name)) return validationError(res, 400, 'cant develop, already developed', next);

    if (isRootNode(treeNode)) {
      // ルートノードは機体のみ許容
      if (treeNode.type !== 'MobileSuit') return validationError(res, 400, 'cant develop, invalid rootnode', next);
      developTreeNodeExec(res, user, treeNode, null/*parentMS*/, true/*isRootNode*/, next);
    } else {
      UserMobilesuit.findOne({userId: user._id, msId: treeNode.parentMobileSuit},
        function findParentMSRes (err, mobilesuit) {
          if (err) return validationError(res, 422, err, next);
          if (!mobilesuit) return validationError(res, 404, 'cant develop, notfound Parent mobilesuit', next);
          if (mobilesuit.exp < treeNode.exp) return validationError(res, 400, 'cant develop, not enough exp', next);

          developTreeNodeExec(res, user, treeNode, mobilesuit, false/*isRootNode*/, next);
      });
    }
  });
}

// ツリーノードの開発状態を保存
function developTreeNodeExec (res, user, treeNode, parentMS, isRootNode, next) {

  user.developedDevTree.push(treeNode.Name);
  user.save(function userSaveRes (err) {
    if (err) return validationError(res, 422, err, next);

    if (isRootNode) {
      developTreeNodeComplete(res, user, treeNode, next);
    } else {
      var _exp = parentMS.exp;
      parentMS.exp -= treeNode.exp;
      parentMS.save(function mobileSuitSaveRes(err) {
        if (err) return validationError(res, 422, err, next);
        logger.system.info('devtree_ctrl::devlopTreeNode exp upd ' + _exp + '=>' + parentMS.exp);
        developTreeNodeComplete(res, user, treeNode, next);
      });
    }
  });
}

// 開発完了
function developTreeNodeComplete(res, user, treeNode, next) {

  if (typeof next === 'function') {
    // 同時解禁ノードの開発時
    respondOK(res, user._id, treeNode.Name, next);
  } else {
    respondOK(res, user._id, treeNode.Name);
  }
}

// ツリーノード購入条件チェック
function purchaseNodeRequest (res, user, nodeId, next) {
  Master.devtree.findById(nodeId, function findTreeNodeRes (err, treeNode) {
    if (err) return handleError(res, err, next);
    if (!treeNode) return validationError(res, 404, 'notfound node', next);

    if (!isDevelopedNode(user, treeNode.Name)) return validationError(res, 400, 'cant purchase, not develop', next);
    if (isPurchasedNode(user, treeNode.Name)) return validationError(res, 400, 'cant purchase, already purchased', next);
    if (user.gamePoint < treeNode.gp) return validationError(res, 400, 'cant purchase, not enough gp', next);

    if (isRootNode(treeNode)) {
      if (treeNode.type !== 'MobileSuit') return validationError(res, 400, 'cant purchase, invalid rootnode', next);
      purchaseTreeNodeExec(res, user, treeNode, null/*parentMS*/, true/*isRootNode*/, next);
    } else {
      UserMobilesuit.findOne({userId: user._id, msId: treeNode.parentMobileSuit},
        function findParentMSRes (err, mobilesuit) {
          if (err) return validationError(res, 422, err, next);
          if (!mobilesuit) return validationError(res, 404, 'cant purchase, notfound Parent mobilesuit', next);

          purchaseTreeNodeExec(res, user, treeNode, mobilesuit, false/*isRootNode*/, next);
      });
    }
  });
}

// ツリーノードの購入を実行
function purchaseTreeNodeExec (res, user, treeNode, parentMS, isRootNode, next) {
  var _gamePoint = user.gamePoint;
  user.gamePoint -= treeNode.gp;
  user.purchasedDevTree.push(treeNode.Name);

  user.save(function userSaveRes (err) {
    if (err) return validationError(res, 422, err, next);
    logger.system.info('devtree_ctrl::purchaseTreeNode gp upd ' + _gamePoint + '=>' + user.gamePoint);

    if ( isRootNode ) {
      // 機体の入手のみ可能
      var inventory = [];
      var unlockedWp = [];
      ms.addMobileSuit(user._id, treeNode.id, inventory, unlockedWp, function mobilesuitAddRes(err, newMobilesuit) {
        if (err) return validationError(res, 422, err, next);
        if (!newMobilesuit) return validationError(res, 404, 'cant purchase, mobilesuit create failed', next);

        purchaseTreeNodeComplete(res, user, treeNode, next);
      });
    } else {
      if (treeNode.type === 'Weapon') {
        var eqData = Master.equipments.object[treeNode.id];
        if (eqData) {
          logger.system.info('devtree_ctrl::default equip '+ parentMS.msId + ' wp ' + eqData.Name);
          // このスロットの初めての武器ならそのまま装備しておく
          var slot = Master.getWeaponSlotOrderNum(eqData.weaponSlotOrder);
          if (slot >= 0 && _.isEmpty(parentMS.inventory[slot])) {
            var wpArr = ['','',''];
            parentMS.inventory = _.map(wpArr, function (w,index) {
              if(index === slot) return eqData.Name;
              else if (_.isEmpty(parentMS.inventory[index])) return '';
              return parentMS.inventory[index];
            });
          }
          parentMS.unlockedWeapons.push(treeNode.Name);
        }
      }
      else if (treeNode.type === 'Color') {
        parentMS.unlockedColors.push(treeNode.Name);
      }
      else if (treeNode.type === 'Engine') {
        parentMS.unlockedBuffs.push(treeNode.Name);
      }

      // 購入した装備を保存
      parentMS.save(function mobileSuitSaveRes(err) {
        if (err) return validationError(res, 422, err, next);
        if (treeNode.type === 'MobileSuit') {
          var inventory = [];
          var unlockedWp = [];
          ms.addMobileSuit(user._id, treeNode.id, inventory, unlockedWp, function mobilesuitAddRes(err, newMobilesuit) {
            if (err) return validationError(res, 422, err, next);
            if (!newMobilesuit) return validationError(res, 404, 'purchase failed, mobilesuit create failed', next);
            purchaseTreeNodeComplete(res, user, treeNode, next);
          });
        } else {
          purchaseTreeNodeComplete(res, user, treeNode, next);
        }
      });
    }
  });
}

// 購入完了。同時解禁ノードが指定されている場合は一緒に開発して購入
function purchaseTreeNodeComplete(res, user, treeNode, next) {

  if (!(_.isEmpty(treeNode.bundleNodes))) {
    async.eachSeries(treeNode.bundleNodes, function (bdlNode, done) {
      logger.system.info('devtree_ctrl::DevelopBundleNode user:' + user._id + ' ' + bdlNode);
      developNodeRequest(res, user, bdlNode, done);
    },
    function bundleNodesDevelopComplete(err) {
      // 開発完了
      async.eachSeries(treeNode.bundleNodes, function (bdlNode, done) {
        logger.system.info('devtree_ctrl::PurchaseBundleNode user:' + user._id + ' ' + bdlNode);
        purchaseNodeRequest(res, user, bdlNode, done);
      },
      function bundleNodesPurchaseComplete(err) {
        // 購入完了
        return respondOK(res, user._id, treeNode.Name);
      });
    });
  } else {
    if (typeof next === 'function') {
      // 同時解禁ノードのアンロック中
      return respondOK(res, user._id, treeNode.Name, next);
    } else {
      // 購入完了
      return respondOK(res, user._id, treeNode.Name);
    }
  }
}

// mobilesuits
exports.mobilesuits = function(req, res) {
  var userId = req.user._id;
  logger.system.info('devtree_ctrl::mobilesuits user:' + userId);

  User.findById(userId, function findUserRes (err, user) {
    if (err) return validationError(res, 422, err);
    if (!user) return validationError(res, 404, 'notfound user');

    var ShopMobileSuits = _.filter(Master.devtree['array'], function(node){
        return node.type == "MobileSuit";
    });

    res.json(200, {'mobileSuitNodes':ShopMobileSuits});
  });
};

// developDevTree
exports.developDevTreeNode = function(req, res) {
  var userId = req.user._id;
  var nodeId = req.body.nodeId;
  logger.system.info('devtree_ctrl::developTreeNode user:' + userId + ' node:' + nodeId);

  User.findById(userId, function findUserRes (err, user) {
    if (err) return validationError(res, 422, err);
    if (!user) return validationError(res, 404, 'notfound user');

    developNodeRequest(res, user, nodeId);
  });
};

// purchaseDevTree
exports.purchaseDevTreeNode = function(req, res) {
  var userId = req.user._id;
  var nodeId = req.body.nodeId;
  logger.system.info('devtree_ctrl::purchaseTreeNode user:' + userId + " node:" + nodeId);

  User.findById(userId, function findUserRes (err, user) {
    if (err) return validationError(res, 422, err);
    if (!user) return validationError(res, 404, 'notfound user');

    purchaseNodeRequest(res, user, nodeId);
  });
};

function dealMobileSuitValidate(nodeId, user, cb)
{
  Master.devtree.findById(nodeId, function(err, treeNode) {
    if (err || !treeNode) {
      return cb('devtree_ctrl:: dealMobileSuit ERR notfound devtree masterdata', null);
    }
    if (isDevelopedNode(user, treeNode.Name) || isPurchasedNode(user, treeNode.Name)) {
      return cb('devtree_ctrl:: dealMobileSuit ERR cant deal, already unlocked', null);
    }

    Master.mobilesuits.findById(treeNode.id, function(err, ms) {
      if (err || !ms) {
        return cb('devtree_ctrl::dealMobileSuit ERR notfound mobilesuit masterdata', null);
      }
      return cb(null, treeNode);
    });
  });
}

function dealWeaponValidation(nodeId, cb)
{
  Master.devtree.findById(nodeId, function(err, treeNodeWp) {
    if (err || !treeNodeWp) {
      return cb('devtree_ctrl:: dealMobileSuit ERR notfound devtree masterdata', null);
    }

    Master.equipments.findById(treeNodeWp.id, function(err, equip) {
      if (err || !equip) {
        return cb('devtree_ctrl:: dealMobileSuit ERR notfound equip masterdata', null);
      }
      return cb(null, treeNodeWp);
    });
  });
}

// 指定した機体をユーザーに配布
exports.dealMobileSuit = function(user, nodeId, callback) {
  // 機体アンロック
  logger.system.info('devtree_ctrl:: dealMobileSuit: ' + nodeId);
  dealMobileSuitValidate(nodeId, user, function(err, treeNode) {
    if (err || !treeNode) {
      return callback(err);
    }
    user.developedDevTree.push(treeNode.Name);
    user.purchasedDevTree.push(treeNode.Name);

    // この機体の初期武器をアンロック(有るなら)
    var inventory = ['','',''];
    var unlockedWp = [];
    var unlockedColor = [];
    async.eachSeries(treeNode.bundleNodes, function(bn, next) {
      Master.devtree.findById(bn, function(err, treeNode) {
        if (err || !treeNode) {
          logger.system.info('devtree_ctrl:: dealMobileSuit ERR notfound devtree masterdata');
          next();
        } else if (treeNode.type == 'Weapon') { 
          logger.system.info('devtree_ctrl:: WP: ' + bn + ' Unlock');
          dealWeaponValidation(bn, function(err, treeNodeWp){
            if (err) {
              logger.system.info(err);
              return next();
            }
            if (!treeNodeWp) return next();

            user.developedDevTree.push(treeNodeWp.Name);
            user.purchasedDevTree.push(treeNodeWp.Name);
            // 初期装備を記憶
            unlockedWp.push(treeNodeWp.Name);
            var eqData = Master.equipments.object[treeNodeWp.id];
            var slot = Master.getWeaponSlotOrderNum(eqData.weaponSlotOrder);
            if (slot >= 0 && _.isEmpty(inventory[slot])) {
              inventory[slot] = eqData.Name;
            }
            next();
          });
        } else if (treeNode.type == 'Color') {
          user.developedDevTree.push(treeNode.Name);
          user.purchasedDevTree.push(treeNode.Name);
          // 初期カラーを記憶
          unlockedColor.push(treeNode.Name);
          next();
        } else {
          next();
        }
      });
    },
    function bdlNodeUlkComplete(err) {
      // ユーザーのアンロック情報DB保存
      user.save(function(err) {
        if (err) return callback(err);
        // 機体をDBに追加
        ms.addMobileSuit(user._id, treeNode.id, inventory, unlockedWp, function(err, newMobilesuit) {
          if (err) return callback(err);
          if (!newMobilesuit) return callback('cant purchase, mobilesuit create failed');
          for (var i = 0; i < unlockedColor.length; ++i ) {
            newMobilesuit.unlockedColors.push(unlockedColor[i]);
          }
          newMobilesuit.save(function mobilesuitSaveRes (err) {
            if (err) {
              logger.system.info('devtree_ctrl:: save failed mobilesuit');
              callback(err);
            } else {
              callback();
            }
          });
        });
      });
    });
  });
};

// DEBUG: devtree ALL unlock
exports.unlockDevTreeNodeAll = function(req, res) {
  var userId = req.user._id;
  var msArr = _.filter(Master.devtree.array, function (o) { return (o.type === 'MobileSuit')});

  User.findById(userId, function findUserRes (err, user) {
    if (err) return validationError(res, 422, err);
    if (!user) return validationError(res, 404, 'notfound user');

    // 最初に機体を追加し、全て完了したらアタッチメントを付ける
    async.each(msArr, function (msNode, next) {
        logger.system.info('devtree_ctrl:: node' + msNode.Name + ' type ' + msNode.type + 'id' + msNode.id);
        var purchaseIdx = _.findIndex(user.purchasedDevTree,
                                  function (nd) { return (nd === msNode.Name) });
        if (purchaseIdx >= 0) {
          logger.system.info('devtree_ctrl:: already unlocked');
          return next();
        }

        user.developedDevTree.push(msNode.Name);
        user.purchasedDevTree.push(msNode.Name);

        var inventory = [];
        var unlockedWp = [];
        ms.addMobileSuit(user._id, msNode.id, inventory, unlockedWp,
         function mobilesuitAddRes(err, newMobilesuit) {
           if (err) logger.system.info('devtree_ctrl:: mobilesuit add failed');
           if (!newMobilesuit) logger.system.info('devtree_ctrl:: invalid mobilesuit add');
           next();
         }
        );
      },
      function msCreateCompleted (err) {
        var atattchmentArr = _.filter(Master.devtree.array, function (o) { return (o.type !== 'MobileSuit'); });

        async.each(atattchmentArr, function (atcNode, next) {
          logger.system.info('devtree_ctrl:: attach ' + atcNode.Name + ' ' + atcNode.type);
          var purchaseIdx = _.findIndex(user.purchasedDevTree,
                                    function (nd) { return (nd === atcNode.Name) });
          if (purchaseIdx >= 0) {
            logger.system.info('devtree_ctrl:: already unlocked');
            return next();
          }

          UserMobilesuit.findOne({userId: userId, msId: atcNode.parentMobileSuit},
            function findMSRes (err, mobilesuit) {
              if (err) {
                logger.system.info('devtree_ctrl:: mobilesuit find failed');
                return next();
              }
              if (!mobilesuit) {
                logger.system.info('devtree_ctrl:: mobilesuit notfound');
                return next();
              }

              if (atcNode.type === 'Weapon') {
                mobilesuit.unlockedWeapons.push(atcNode.Name);
              } else if (atcNode.type === 'Color') {
                mobilesuit.unlockedColors.push(atcNode.Name);
              } else if (atcNode.type === 'Engine') {
                mobilesuit.unlockedBuffs.push(atcNode.Name);
              } else {
                return next();
              }

              user.developedDevTree.push(atcNode.Name);
              user.purchasedDevTree.push(atcNode.Name);
              mobilesuit.save(function mobilesuitSaveRes (err) {
                if (err) logger.system.info('devtree_ctrl:: save failed mobilesuit');
                return next();
              });
            }
          );
        },
        function attachCompleted (err) {
          user.save(function userSaveRes (err) {
            if (err) return validationError(res, 422, err);
            res.send(201);
          });
          logger.system.info('devtree_ctrl:: AllUnlockCompleted');
        });
      }
    );
  });
};

function handleError(res, err, next) {
  logger.system.info('devtree_ctrl: handleError ', err);
  if (typeof next === 'function') {
    return next();
  }
  return res.send(500, err);
}
