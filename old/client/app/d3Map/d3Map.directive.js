'use strict';

angular.module('evoserverApp')
  .directive('d3Map', ['d3Service', function (d3Service) {
    return {
      restrict: 'EA',
      scope: {
        stat: '=' // bi-directional data binding
      },
      link: function (scope, element, attrs) {
        d3Service.d3().then(function(d3) {
          var stat = scope.stat;

          var svg = d3.select(element[0])
                      .append('svg');

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

            var margin = { top: 20, left: 50, bottom: 20, right: 10 };
            //var width = d3.select(element[0]).node().offsetWidth;
            var width = 480;
            var height = 480;
            svg.attr('width', width + 'px')
               .attr('height', height + 'px')
               .style('margin', '4px 0 0 ' + (d3.select(element[0]).node().offsetWidth/2- width/2) + 'px');

            svg.append('rect')
               .attr('fill', 'rgba(0,0,0,0.2)')
               .attr('x', '0')
               .attr('y', '0')
               .attr('width', width)
               .attr('height', height);




            var myarr = Array();
            var k, j, len_k = stat.timeline.length, len_j;

            // -- timelineから(event_name==killed)を探してpositionを持っていたらmyarrに情報を格納 --
            for( k = 0; k < len_k; k++ )
            {
              if( stat.timeline[k].event_name == 'killed' )
              {
                len_j = stat.timeline[k].params.length;
                var obj = Object();
                for( j = 0; j < len_j; j++ )
                {
                  if( stat.timeline[k].params[j].key == 'position' )
                  {
                    obj['pos'] = stat.timeline[k].params[j].value.split(',');
                    myarr.push( obj );
                  }
                  else
                  {
                    obj[stat.timeline[k].params[j].key] = stat.timeline[k].params[j].value;
                  }
                }
              }
            }

            // -- 集計したpositionからscaleを作成(要確認) --
            var min = [d3.min(myarr, function(d) { return Number(d['pos'][0]); }), d3.min(myarr, function(d) { return Number(d['pos'][1]) })];
            var max = [d3.max(myarr, function(d) { return Number(d['pos'][0]); }), d3.max(myarr, function(d) { return Number(d['pos'][1]) })];
            min = d3.min(min, function(d) { return d; });
            max = d3.max(max, function(d) { return d; });
            var scale = d3.scale.linear()
                                .domain([ min, max ])
                                .range([-240,240]);

            // -- svgに点を配置 --
            var circles = svg.selectAll('circle')
                             .data(myarr)
                             .enter()
                             .append('circle')
                             .attr('cx', function(d,i) { return scale(d['pos'][0]) + ( width / 2 ); })
                             .attr('cy', function(d,i) { return scale(d['pos'][1]) + ( height / 2 ); })
                             .attr('r', 2)
                             .attr('fill', 'teal');


            // -- 更に情報を載せたい --
            var detail = ['','','',''];

            circles.on('mouseover', function(d) {
              detail[0] = 'name : ' + d['name'];
              detail[1] = 'killed by : ' + d['killer_name'];
              detail[2] = 'causer : ' + d['causer'];
              detail[3] = 'location : ' + d['pos'][0] + ', ' + d['pos'][1];
              txt.data(detail)
                 .text( function(d) { return d; } );
            } );


            // -- 情報表示領域作成 --
            svg.append('rect')
               .attr('x', 10 )
               .attr('y', 10)
               .attr('width', 256)
               .attr('height', 64)
               .attr('fill', 'rgba(0,0,0,0.5)');

            var txt = svg.selectAll('text')
                         .data(detail)
                         .enter()
                         .append('text')
                         .attr('x', 12)
                         .attr('y', function(d,i) { return i * 16 + 24; })
                         .attr('fill', 'white');

          }; // scope.render
        });
      }
    };
  }]);
