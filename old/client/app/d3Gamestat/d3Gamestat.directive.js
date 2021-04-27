'use strict';

angular.module('evoserverApp')
  .directive('d3Gamestat', ['d3Service', function (d3Service) {
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

            var margin = { top: 20, left: 50, bottom: 20, right: 10 };
            var width = d3.select(element[0]).node().offsetWidth;
            var height = parseInt(attrs.height) || 360;
            svg.attr('height', height + 'px');

            if (width <= 0)
            {
              return;
            }

            // tooltip
            var tooltip = d3.select('#tooltip');

            var xTimelineScale = d3.scale.linear()
                                         .domain([0, stat.gametime])
                                         .range([0, width - (margin.left + margin.right)]);
            var xTimelineAxis = d3.svg.axis()
                                      .scale(xTimelineScale)
                                      .orient('bottom')
                                      .ticks(20);
            svg.append('g')
               .classed('x timeline axis', true)
               .call(xTimelineAxis)
               .attr('transform', 'translate(' + margin.left + ', ' + (height - margin.bottom) + ')');

            // =====================================================================
            // タイムラインフィルタ
            var spawnedAndKilledTimeline = [];
            var ticketTimeline = [];
            var fobTimeline = [];
            var hqTimeline = [];
            var eventSwitcher = {
              'spawned': spawnedAndKilledTimeline,
              'killed': spawnedAndKilledTimeline,
              'ticket': ticketTimeline,
              'fob': fobTimeline,
              'hq_broken': hqTimeline,
              'hq_restored': hqTimeline
            };
            stat.timeline.forEach(function(event) {
              if (event.event_name) {
                var timeline = {};
                event.params.forEach(function(param) {
                  timeline[param.key] = param.value;
                });
                timeline.time = event.time;
                timeline.event_name = event.event_name;

                if (eventSwitcher[event.event_name]) {
                  eventSwitcher[event.event_name].push(timeline);
                }
              }
            });
            // =====================================================================

            // =====================================================================
            // スポーン状況
            var playersAlpha = stat.teams[0].players;
            var playersBravo = stat.teams[1].players;

            var maxTeamPlayerCount = Math.max(playersAlpha.length, playersBravo.length);
            // ALPHAは上に伸ばす
            var yCenter = ((height - (margin.top + margin.bottom)) / 2) + margin.top;
            var yTeamAlphaSpawnScale = d3.scale.linear()
                                               .domain([0, maxTeamPlayerCount])
                                               .range([yCenter, margin.top]);
            var yTeamBravoSpawnScale = d3.scale.linear()
                                               .domain([0, maxTeamPlayerCount])
                                               .range([yCenter, height - margin.bottom]);
            var spawnedAndKilledTimelineCounterMapper = {
              counter: 0,
              func: function(element/*, index, array*/) {
                return {
                  time: element.time,
                  name: element.name,
                  character: element.character,
                  event_name: element.event_name,
                  killer: element.event_name === 'killed' ? element.killer_name : '',
                  causer: element.event_name === 'killed' ? element.causer : '',
                  count: element.event_name === 'spawned' ? ++this.counter : --this.counter
                };
              },
              reset: function() {
                this.counter = 0;
              }
            };
            var teamAlphaTimeline = spawnedAndKilledTimeline.filter(function(element/*, index, array*/) {
              return stat.teams[0].players.find(function(inner_element) {
                return (inner_element.name === element.name) || (inner_element.name === element.killed_name);
              });
            });
            var teamBravoTimeline = spawnedAndKilledTimeline.filter(function(element/*, index, array*/) {
              return stat.teams[1].players.find(function(inner_element) {
                return (inner_element.name === element.name) || (inner_element.name === element.killed_name);
              });
            });
            teamAlphaTimeline = teamAlphaTimeline.map(spawnedAndKilledTimelineCounterMapper.func, spawnedAndKilledTimelineCounterMapper);
            teamAlphaTimeline.unshift({ time: 0, count: 0 });
            teamAlphaTimeline.push({ time: stat.gametime, count: teamAlphaTimeline[teamAlphaTimeline.length - 1].count });
            spawnedAndKilledTimelineCounterMapper.reset();
            teamBravoTimeline = teamBravoTimeline.map(spawnedAndKilledTimelineCounterMapper.func, spawnedAndKilledTimelineCounterMapper);
            teamBravoTimeline.unshift({ time: 0, count: 0 });
            teamBravoTimeline.push({ time: stat.gametime, count: teamAlphaTimeline[teamAlphaTimeline.length - 1].count });

            var teamAlphaArea = d3.svg.area()
                                      .x(function(d) {
                                        return xTimelineScale(d.time);
                                      })
                                      .y0(function() { return yCenter; })
                                      .y1(function(d) {
                                        return yTeamAlphaSpawnScale(d.count);
                                      })
                                      .interpolate('step-after');
            svg.append('path')
               .attr('d', teamAlphaArea(teamAlphaTimeline))
               .attr('fill', 'aqua')
               .attr('opacity', '0.1')
               .attr('transform', 'translate(' + margin.left + ', ' + '0)');
            var teamBravoArea = d3.svg.area()
                                      .x(function(d) {
                                        return xTimelineScale(d.time);
                                      })
                                      .y0(function() { return yCenter; })
                                      .y1(function(d) {
                                        return yTeamBravoSpawnScale(d.count);
                                      })
                                      .interpolate('step-after');
            svg.append('path')
               .attr('d', teamBravoArea(teamBravoTimeline))
               .attr('fill', 'deeppink')
               .attr('opacity', '0.1')
               .attr('transform', 'translate(' + margin.left + ', ' + '0)');

            //===========================================================================
            // マウスオーバーで情報表示（仮）
            var appendSpawnAndDeathInfo = function(dataset, yScale) {
              svg.selectAll('circle')
                 .data(dataset)
                 .enter()
                 .append('circle')
                 .attr('cx', function(d) {
                   return xTimelineScale(d.time);
                 })
                 .attr('cy', function(d) {
                   return yScale(d.count);
                 })
                 .attr('r', 5)
                 .attr('opacity', 0.3)
                 .attr('fill', function(d) {
                   return d.event_name === 'spawned' ? 'orangered' : 'dimgray';
                 })
                 .on('mouseover', function(d) {
                   return tooltip.style('visibility', 'visible').text(function() {
                     var message = '';
                     if (d.event_name === 'spawned') {
                       message = d.event_name + '\n' + d.name + '\n' + d.character;
                     } else if (d.event_name === 'killed') {
                       message = d.event_name + '\n' + d.name + '\n' + d.character + '\nby ' + d.killer + ' : ' + d.causer;
                     }
                     return message;
                   });
                 })
                 .on('mousemove', function() {
                   return tooltip.style('top', (event.pageY - 20) + 'px')
                   .style('left', (event.pageX + 10) + 'px');
                 })
                 .on('mouseout', function() {
                   return tooltip.style('visibility', 'hidden');
                 })
                 .attr('transform', 'translate(' + margin.left + ', ' + '0)')
                 ;
            };
            appendSpawnAndDeathInfo(teamAlphaTimeline, yTeamAlphaSpawnScale);
            appendSpawnAndDeathInfo(teamBravoTimeline, yTeamBravoSpawnScale);
            //===========================================================================

            var tickValues = d3.range(maxTeamPlayerCount + 1);
            tickValues.shift();
            var yTeamAlphaAxis = d3.svg.axis()
                                       .scale(yTeamAlphaSpawnScale)
                                       .orient('left')
                                       .tickValues(tickValues)
                                       .tickFormat(d3.format('d'));
            var yTeamBravoAxis = d3.svg.axis()
                                       .scale(yTeamBravoSpawnScale)
                                       .orient('left')
                                       .tickValues(tickValues)
                                       .tickFormat(d3.format('d'));
            svg.append('g')
               .classed('y team alpha axis', true)
               .call(yTeamAlphaAxis)
               .attr('transform', 'translate(' + (width - margin.right) + ', ' + '0)');
            svg.append('g')
               .classed('y team bravo axis', true)
               .call(yTeamBravoAxis)
               .attr('transform', 'translate(' + (width - margin.right) + ', ' + '0)');


            // =====================================================================

            // =====================================================================
            // チケット状況
            var yTicketScale = d3.scale.linear()
                                       .domain([stat.initial_ticket, 0])
                                       .range([0, height - (margin.top + margin.bottom)]);
            var yTicketAxis = d3.svg.axis()
                                    .scale(yTicketScale)
                                    .orient('left')
                                    .ticks(5);
            var teamAlphaTicket = ticketTimeline.filter(function(element/*, index, array*/) {
              return element.team === '0';
            });
            var teamBravoTicket = ticketTimeline.filter(function(element/*, index, array*/) {
              return element.team === '1';
            });
            var lineTicket = d3.svg.line()
                                   .x(function(d) {
                                     return xTimelineScale(d.time) + margin.left;
                                   })
                                   .y(function(d) {
                                     return yTicketScale(d.ticket) + margin.top;
                                   })
                                   .interpolate('linear');
            svg.append('g')
               .classed('y ticket axis', true)
               .call(yTicketAxis)
               .attr('transform', 'translate(' + margin.left + ', ' + margin.top + ')');
            svg.append('path')
               .attr('d', lineTicket(teamAlphaTicket))
               .classed('line', true)
               .attr('stroke', 'blue')
               .attr('stroke-width', 2)
               .attr('fill', 'none');
            svg.append('path')
               .attr('d', lineTicket(teamBravoTicket))
               .classed('line', true)
               .attr('stroke', 'red')
               .attr('stroke-width', 2)
               .attr('fill', 'none');
            // =====================================================================

          }; // scope.render
        });
      }
    };
  }]);
