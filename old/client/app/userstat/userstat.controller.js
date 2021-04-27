'use strict';

angular.module('evoserverApp')
  .factory('UserstatCtrl', function() {
  })
  .controller('UserstatCtrl', function ($scope, $http) {
    $scope.message = 'Hello';

    $http.get('api/user/stats/mstable/').then( function(res) {
      //console.log(res.data);
      $scope.mstable = res.data;
    }, function(res) {
      console.log(res);
    });

    $http.get('api/user/stats/view/').then(function(res) {

      var arr = Array();
      res.data.forEach( function(elm) {
        if (elm.name.length > 0)
          arr.push(elm);
      });
      $scope.userstats = arr;

      console.log($scope.userstats);

    }, function(res) {
      console.log(res);
    });


// -- function --
    $scope.getSum = function(array, element) {
      var sum = 0;
      array.forEach(function(elm) {
        sum += elm[element];
      });
      return sum;
    };

    $scope.getMax = function(array, element) {
      var max = 0;
      array.forEach(function(elm) {
        if (elm[element] > max) {
          max = elm[element];
        }
      });
      return max;
    };

    $scope.sortBy = function(array, element, r) {
      array.sort(function(a, b) {
        if (!r) {
          if (a[element] > b[element]) return -1;
          if (a[element] < b[element]) return 1;
        } else {
          if (a[element] < b[element]) return -1;
          if (a[element] > b[element]) return 1;
        }
        return 0;
      });
      return array;
    };
    $scope.sortByFinalblowsRate = function(array, r) {
      array.sort(function(a, b) {
        var av = a.death > 0 ? (a.finalblows / a.death) : a.finalblows;
        var bv = b.death > 0 ? (b.finalblows / b.death) : b.finalblows;

        if (!r) {
          if (av > bv) return -1;
          if (av < bv) return 1;
        } else {
          if (av < bv) return -1;
          if (av > bv) return 1;
        }
        return 0;
      });
      return array;
    };
    $scope.sortByEliminationsRate = function(array, r) {
      array.sort(function(a, b) {
        var av = a.death > 0 ? (a.eliminations / a.death) : a.eliminations;
        var bv = b.death > 0 ? (b.eliminations / b.death) : b.eliminations;

        if (!r) {
          if (av > bv) return -1;
          if (av < bv) return 1;
        } else {
          if (av < bv) return -1;
          if (av > bv) return 1;
        }
        return 0;
      });
      return array;
    };
    $scope.sortByDamageRate = function(array, r) {
      array.sort(function(a, b) {
        var av = a.damage_taken > 0 ? (a.damage_given / a.damage_taken) : a.damage_given;
        var bv = b.damage_taken > 0 ? (b.damage_given / b.damage_taken) : b.damage_given;

        if (!r) {
          if (av > bv) return -1;
          if (av < bv) return 1;
        } else {
          if (av < bv) return -1;
          if (av > bv) return 1;
        }
        return 0;
      });
      return array;
    };

    $scope.getTimeString = function(num) {
      var h = 0, m = 0;
      var str = '';
      h = parseInt(num / 3600);
      if (h > 0) {
        str += (h + 'h');
        num -= (h * 3600);
      }
      m = parseInt(num / 60);
      if (m > 0 || str.length > 0) {
        str += (m + 'm');
        num -= (m * 60);
      }
      str += (num + 's');

      return str;
    };

    $scope.sort_flag = Object();
    $scope.prev_title = '';
    $scope.onClickTitle = function(title) {
      if (title in $scope.sort_flag) {

          $scope.sort_flag[title] = !$scope.sort_flag[title];

      } else {
        $scope.sort_flag[title] = true;
      }


      if ($scope.userstats) {
        switch (title) {
          case 'fb':
            $scope.userstats = $scope.sortByFinalblowsRate($scope.userstats, $scope.sort_flag[title]);
            break;

          case 'el':
            $scope.userstats = $scope.sortByEliminationsRate($scope.userstats, $scope.sort_flag[title]);
            break;

          case 'dr':
            $scope.userstats = $scope.sortByDamageRate($scope.userstats, $scope.sort_flag[title]);
            break;

          default:
            $scope.userstats = $scope.sortBy($scope.userstats, title, $scope.sort_flag[title]);
            break;
        }
      }


      var elm = document.getElementById('t_' + title);
      if (elm) {
        elm.innerHTML = $scope.sort_flag[title] ? '▲' : '▼';
      }

      if (title != $scope.prev_title) {
        elm = document.getElementById('t_' + $scope.prev_title);
        if (elm) {
          elm.innerHTML = '';
        }
      }

      $scope.prev_title = title;
    };

    $scope.onClickInfo = function() {
      var element = document.getElementById('info');
      if (element) {
        if (element.style.display == 'none') {
          element.style.display = 'inline';
        } else {
          element.style.display = 'none';
        }
      }
    };

// -- function --


    $scope.stat = null;

//-- shokichi --
    var obj = Object();
    obj['_id'] = "57358e0df87d1f1b0bd982b0";
    obj['damage_given'] = 100;
    obj['damage_taken'] = 50;
    obj['death'] = 1;
    obj['finalblows'] = 999;
    obj['eliminations'] = 999;
    obj['killstreak'] = 1;
    obj['lose'] = 3;
    obj['name'] = "namae";
    obj['recovery'] = 0;
    obj['spotted'] = 0;
    obj['time'] = 30000;
    obj['win'] = 12;
    obj['conquered_seconds'] = 0;
    obj['conquered_times'] = 0;
    obj['maintain_seconds'] = 0;
//-- shokichi --

    $scope.stat = obj;

    $scope.onClickMs = function (mss, $event) {
      var b = false;
      var elements = document.getElementsByName('row_ds_' + mss._id);
      if (elements) {
        var eid = document.getElementById('col_usr_' + mss.userId);
        if (eid) {

          var i, len;
          for (i = 0, len = elements.length; i < len; ++i) {
            if (b = (elements[i].style.display == 'none')) {
              elements[i].style.display = 'table-row';
              eid.rowSpan++;
            } else {
              elements[i].style.display = 'none';
              eid.rowSpan--;
            }
          }

          for (var j = 0, len_j = elements[len-1].cells.length; j < len_j; ++j) {
            elements[len-1].cells[j].rowSpan = 2;
          }
        }
      }

      elements = document.getElementsByName('col_' + mss._id);
      if (elements) {
        for (var i = 0, len = elements.length; i < len; ++i) {
          if (b) {
            elements[i].rowSpan = 1;
          } else {
            elements[i].rowSpan = 2;
          }
        }
      }

      var eid = document.getElementById('col_ms_' + mss._id);
      if (eid) {
        if (b) {
          eid.rowSpan = mss.damage_source.length + 2;
        } else {
          eid.rowSpan = 1;
        }
      }
    }

    $scope.onClick = function (stat, $event) {
      var b = false;
      var eid = document.getElementById('col_usr_' + stat._id);
      if (eid) {
        var elements = document.getElementsByName('row_' + stat._id);
        if (elements) {

          for (var i = 0, len_i = elements.length; i < len_i; ++i) {
            if (b = (elements[i].style.display == 'none')) {
              elements[i].style.display = 'table-row';
            } else {
              elements[i].style.display = 'none';
            }
          }
        }

        if (b) {
          eid.rowSpan = elements.length + 1;
        } else {
          eid.rowSpan = 1;

          for (var i = 0, len_i = stat.mobilesuit_stat.length; i < len_i; ++i) {
            elements = document.getElementsByName('row_ds_' + stat.mobilesuit_stat[i]._id);
            if (elements) {
              for (var j = 0, len_j = elements.length; j < len_j; ++j) {
                  elements[j].style.display = 'none';
              }
            }

            elements = document.getElementsByName('col_' + stat.mobilesuit_stat[i]._id);
            if (elements) {
              for (var j = 0, len_j = elements.length; j < len_j; ++j) {
                elements[j].rowSpan = 2;
              }
            }
            var eid2 = document.getElementById('col_ms_' + stat.mobilesuit_stat[i]._id);
            if (eid2) {
                eid2.rowSpan = 1;
            }

          }

        }
      }
    }

    $scope.onMouseOver = function (stat, $event) {
      $scope.stat = stat;
      var element = document.getElementById('popup');
      if( element ) {
        element.style.visibility = 'visible';

        var jc = stat.win + stat.lose;
        var dmg_done = $scope.getSum(stat.mobilesuit_stat, 'damage_given');
        var dmg_taken = $scope.getSum(stat.mobilesuit_stat, 'damage_taken');
        var dmg_guard = $scope.getSum(stat.mobilesuit_stat, 'damage_guard');

        var str = '<br><div style="width:auto;margin:-8px 12px 8px 12px;"><table class="simple">';
        str += '<tr><th style="width:20%; padding:4px;"></th><th style="width:40%; padding:4px;">総合</th><th style="width:40%; padding:4px;">試合平均</th></tr>';
        str += '<tr><th style="padding:4px;">ファイナルブロウ</th><td style="text-align:center;">' + (stat.finalblows) + '</td><td style="text-align:center;">' + ((stat.finalblows)/jc).toFixed(1) +  '</td></tr>';
        str += '<tr><th style="padding:4px;">キル</th><td style="text-align:center;">' + (stat.eliminations) + '</td><td style="text-align:center;">' + ((stat.eliminations)/jc).toFixed(1) +  '</td></tr>';
        str += '<tr><th style="padding:4px;">与ダメ</th><td style="text-align:center;">' + (dmg_done) + '</td><td style="text-align:center;">' + ((dmg_done)/jc).toFixed(1) +  '</td></tr>';
        str += '<tr><th style="padding:4px;">被ダメ</th><td style="text-align:center;">' + (dmg_taken) + '</td><td style="text-align:center;">' + ((dmg_taken)/jc).toFixed(1) +  '</td></tr>';
        str += '<tr><th style="padding:4px;">防ダメ</th><td style="text-align:center;">' + (dmg_guard) + '</td><td style="text-align:center;">' + ((dmg_guard)/jc).toFixed(1) +  '</td></tr>';
        //str += '<tr><th style="padding:4px;">獲得Exp</th><td style="text-align:center;">' + ((stat.win*100)+(stat.damage_given*0.1)+(stat.kill*20)).toFixed(1) + '</td><td style="text-align:center;">' + (((stat.win*100)+(stat.damage_given*0.1)+(stat.kill*20))/(stat.win+stat.lose)).toFixed(1) + '</td></tr>';
        //str += '<tr><th style="padding:4px;">獲得GP</th><td style="text-align:center;">' + ((stat.win*5000)+(stat.damage_given*0.4)+(stat.kill*600)).toFixed(1) + '</td><td style="text-align:center;">' + (((stat.win*5000)+(stat.damage_given*0.4)+(stat.kill*600))/(stat.win+stat.lose)).toFixed(1) + '</td></tr>';
        str += '<tr><th style="padding:4px;" colspan="3">使用機体 <font size=1>(top3)</font></th></tr>';
        var mss = $scope.sortBy(stat.mobilesuit_stat, 'time', false);
        var c = 0;
        mss.some( function(elm) {
          str += '<tr>';
          str += '<td colspan="3" style="padding: 2px 2px;">';
          str += '<svg width="100%" height="20px" style="vertical-align:bottom;">';
          str += '<g>';
          str += '<rect fill="#0196ff" x="0" y="0" width="' + parseInt((elm.time / $scope.getMax(stat.mobilesuit_stat, "time")) * 100) + '%" height="100%"></rect>';
          str += '<text text-anchor="start" y="16">' + $scope.mstable[elm.msId] + '</text>';
          str += '<text text-anchor="end" x="100%" y="16">';
          str += $scope.getTimeString(elm.time);
          str += '</text>';
          str += '</g>';
          str += '</svg>';
          str += '</td>';
          str += '</tr>';

          c++;
          if (c >= 3)
            return true;
        });
        str += '</table></div>';

        element.innerHTML = '&nbsp;' + (stat.name.length>0 ? stat.name : stat._id) + '\'s Stats Overview.<br>' + document.getElementById('graph_'+ stat._id).innerHTML + str;

        // ※本当はココでグラフの再描画を掛けたかった
      }
    }
    $scope.onMouseOut = function ($event) {
      document.getElementById('popup').style.visibility = 'hidden';
    }
    $scope.onMouseMove = function ($event) {
      var element = document.getElementById('popup');
      if( element ) {
        element.style.marginLeft = (event.clientX-180) + 'px';
        element.style.marginTop = (event.clientY-30) + 'px';
        //console.log(getPosition(element));
      }
    }

  });

angular.module('evoserverApp')
  .directive('d3UserstatTest', ['d3Service', function (d3Service) {
    return {
      restrict: 'EA',
      scope: {
        stat: '=' // bi-directional data binding
      },
      link: function (scope, element, attrs) {
        d3Service.d3().then(function(d3) {
          var stat = scope.stat;

          var svg = d3.select(element[0])
                      .append('svg')
                      .style('width', '100%');

          // browser onresize event
          window.onresize = function() {
            scope.$apply();
          };

          // watch for resize event
          scope.$watch(function() {
            return d3.select(element[0]).node().offsetWidth;
          }, function() {
            scope.render();
          });


          scope.render = function() {
            if (!stat) {
              return;
            }

            // our custom d3 code
            svg.selectAll('*').remove();

            //var width = d3.select(element[0]).node().offsetWidth;
            var width = 512;
            var height = 150;

            svg.append('rect')
               .attr('fill', 'rgba(0,0,255,0.0)')
               .attr('x', '0')
               .attr('y', '0')
               .attr('width', width)
               .attr('height', height);



            var r = 72;
            var arc = d3.svg.arc()
                        .outerRadius(r)
                        .innerRadius(65);

            var pie = d3.layout.pie()
                        .sort(null)
                        .value(function(d) { return d; });


            var data = [[stat.win, stat.lose], [stat.finalblows, stat.death], [stat.damage_given, stat.damage_taken]];
            var str = ['勝敗', 'ファイナルブロウ', 'ダメージ'];
            var rate = [stat.win / (stat.win + stat.lose), stat.finalblows / (stat.finalblows + stat.death), stat.damage_given / (stat.damage_given + stat.damage_taken)];


            var idx = 0;
            for(idx = 0; idx < 3; idx++) {

              var x = (((width-((r*2)*3))/(3+1))*(idx+1)) + (r*(idx+1)) + (r*idx);
              var g = svg.append('g')
                         .attr('transform', 'translate(' + x + ',' + ( height / 2 + 2 ) + ')');

              g.selectAll(".arc")
               .data(pie(data[idx]))
               .enter()
               .append('g')
               .attr('class', 'arc')
               .append('path')
               .attr('d', arc)
               .style('fill', function(d, i) {
                switch(i) {
                  case 0:   return '#0196ff';
                  default:  return 'rgba(0,0,0,0)';
                }
              });

              g.append('text')
               .text( str[idx] )
               .attr('text-anchor', 'middle')
               .attr('y', -2);

              g.append('text')
               .text( (rate[idx] * 100).toFixed(1) + '%' )
               .attr('text-anchor', 'middle')
               .attr('y', 16);
            }

          }; // scope.render
        });
      }
    };
  }]);
