'use strict';

angular.module('evoserverApp')
  .directive('d3Bars', ['d3Service', function (d3Service, $window) {
    return {
      restrict: 'EA',
      scope: {},
      link: function (scope, element, attrs) {
        d3Service.d3().then(function(d3) {
          var margin = parseInt(attrs.margin) || 20,
              barHeight = parseInt(attrs.barHeight) || 20,
              barPadding = parseInt(attrs.barPadding) || 5;

          // our d3 code will go here
          var svg = d3.select(element[0])
                      .append('svg')
                      .style('width', '100%');

          // browser onresize event
          window.onresize = function() {
            scope.$apply();
          };
          
          // hard-code data
          scope.data = [
            { name: 'Greg', score: 98 },
            { name: 'Ari', score: 96 },
            { name: 'Q', score: 75 },
            { name: 'Loser', score: 48 }
          ];
          
          // watch for resize event
          scope.$watch(function() {
            return angular.element($window).innerWidth;
          }, function() {
            scope.render(scope.data);
          });
          
          scope.render = function(data) {
            // our custom d3 code
            svg.selectAll('*').remove();
            if (!data) {
              return;
            }
            
            var width = d3.select(element[0]).node().offsetWidth - margin;
            var height = scope.data.length * (barHeight + barPadding);
            var color = d3.scale.category20();
            var xScale = d3.scale.linear()
                                 .domain([0, d3.max(data, function(d) {
                                   return d.score;
                                 })])
                                 .range([0, width]);
            svg.attr('height', height);
            
            svg.selectAll('rect')
               .data(data).enter()
               .append('rect')
               .attr('height', barHeight)
               .attr('width', 140)
               .attr('x', Math.round(margin / 2))
               .attr('y', function(d, i) {
                 return i * (barHeight + barPadding);
               })
               .attr('fill', function(d) { return color(d.score); })
               .transition()
                 .duration(1000)
                 .attr('width', function(d) {
                   return xScale(d.score);
                 });
          }; // scope.render
        });
      }
    };
  }]);
