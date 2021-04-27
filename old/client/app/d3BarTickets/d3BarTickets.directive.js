'use strict';

angular.module('evoserverApp')
  .directive('d3BarTickets', ['d3Service', function (d3Service) {
    return {
      restrict: 'EA',
      scope: {
        stat: '=' // bi-directional data binding
      },
      link: function (scope, element, attrs) {
        d3Service.d3().then(function(d3) {
          var margin = parseInt(attrs.margin) || 10;
          var barHeight = parseInt(attrs.barHeight) || 15;
          var barPadding = parseInt(attrs.barPadding) || 4;
          var teams = scope.stat.teams || [
            {
              remain_ticket: 3000,
              decreased_ticket_by_killed: 3000,
              decreased_ticket_by_conquered: 4000
            },
            {
              remain_ticket: 6000,
              decreased_ticket_by_killed: 3000,
              decreased_ticket_by_conquered: 1000
            },
          ];

          // our d3 code will go here
          var svg = d3.select(element[0])
                      .append('svg')
                      .style('width', '100%');

          // browser onresize event
          window.onresize = function() {
            scope.$apply();
          };

          // watch for resize event
          scope.$watch(function() {
            return angular.element(window)[0].innerWidth;
          }, function() {
            scope.render(teams);
          });

          scope.render = function(teams) {
            // our custom d3 code
            svg.selectAll('*').remove();
            if (!teams) {
              return;
            }

            // tooltip
            var tooltip = d3.select('#tooltip');

            var width = d3.select(element[0]).node().offsetWidth - margin;
            var height = (barHeight + barPadding) * 2;
            svg.attr('height', height);

            var totalTicket = teams[0].remain_ticket + teams[0].decreased_ticket_by_killed + teams[0].decreased_ticket_by_conquered;
            var xScale = d3.scale.linear()
                                 .domain([0, totalTicket])
                                 .range([0, width]);

            var list = [
              [ { x: 0, y: teams[0].remain_ticket }, { x: 1, y: teams[1].remain_ticket } ],
              [ { x: 0, y: teams[0].decreased_ticket_by_killed }, { x: 1, y: teams[1].decreased_ticket_by_killed } ],
              [ { x: 0, y: teams[0].decreased_ticket_by_conquered }, { x: 1, y: teams[1].decreased_ticket_by_conquered } ]
            ];

            var colors =
              // remain, killed, conquered
              [ 'rgb(26, 67, 204)', 'rgb(255, 103, 60)', 'rgb(174, 255, 86)' ];

            var stack = d3.layout.stack();
            var dataset = stack(list);
            svg.selectAll('g')
               .data(dataset).enter()
               .append('g')
               .attr('fill', function(d, i) {
                 return colors[i];
               })
               .attr('opacity', function(d, i) {
                 return i === 0 ? 1.0 : 0.3;
               })
               .selectAll('rect') // 棒グラフひとつを対象にする
               .data(function(d) {
                 return d; // listの一つ分のデータ
               })
               .enter()
               .append('rect')
               .attr('x', function(d) {
                 return Math.round(margin / 2) + xScale(d.y0);
               })
               .attr('y', function(d, i) {
                 return i * (barHeight + barPadding);
               })
               .attr('height', barHeight)
               .attr('width', function(d) {
                 return xScale(d.y);
               })
               .on('mouseover', function(d) {
                 return tooltip.style('visibility', 'visible').text(d.y);
               })
               .on('mousemove', function() {
                 return tooltip.style('top', (event.pageY - 20) + 'px')
                               .style('left', (event.pageX + 10) + 'px');
               })
               .on('mouseout', function() {
                 return tooltip.style('visibility', 'hidden');
               })
               ;
          }; // scope.render
        });
      }
    };
  }]);
