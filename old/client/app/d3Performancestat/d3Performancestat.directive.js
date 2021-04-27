'use strict';

angular.module('evoserverApp')
  .directive('d3Performancestat', ['d3Service', function (d3Service) {
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
            var height = parseInt(attrs.height) || 240;
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


			// get 
			function getFmt(val)
			{
				var r = ("  " + parseFloat(val).toFixed(1)).slice(-5);
				return r;
			}

			// -- --
			console.log(stat);

			svg.on('mousemove', function() {
				var pos = d3.mouse(this);

				if( child )
				{
					var mx = pos[0]-100;

					if( mx < margin.left )
						mx = margin.left;
					if( mx + 200 > width - margin.right )
						mx = width - margin.right - 200;

					child.attr('visibility', 'visible')
						 .attr('x', mx)
						 .attr('y', pos[1]-80);

//					child.select('text')
//						 .text( ( ( ( height - margin.bottom ) - pos[1] ) / ( height - margin.bottom - margin.top ) * maxFps ).toFixed(1) + 'fps' );
					var idx = parseInt( ( pos[0] - margin.left ) / ( bw * 5 ) );

					if( idx >= 0 && idx < arrFpsLen )
					{
						txt_fps.text(' : ' + getFmt(arrFps[idx][0]) + ', ' + getFmt(arrFps[idx][1]) + ', ' + getFmt(arrFps[idx][2]) );
						txt_frame.text(' : ' + getFmt(arrDelta[idx][0][0]) + ', ' + getFmt(arrDelta[idx][0][1]) + ', ' + getFmt(arrDelta[idx][0][2]));
						txt_game.text(' : ' + getFmt(arrDelta[idx][1][0]) + ', ' + getFmt(arrDelta[idx][1][1]) + ', ' + getFmt(arrDelta[idx][1][2]));
						txt_render.text(' : ' + getFmt(arrDelta[idx][2][0]) + ', ' + getFmt(arrDelta[idx][2][1]) + ', ' + getFmt(arrDelta[idx][2][2]));
						txt_gpu.text(' : ' + getFmt(arrDelta[idx][3][0]) + ', ' + getFmt(arrDelta[idx][3][1]) + ', ' + getFmt(arrDelta[idx][3][2]));
					}
				}
				if( rect )
				{
					if( pos[0] >= margin.left && pos[0] <= width - margin.right )
					{
						//rect.attr('x', parseInt( pos[0] / ( bw * 5 ) ) * ( bw * 5 ) );
						var px = ( parseInt( ( pos[0] - margin.left ) / ( bw * 5 ) ) * ( bw * 5 ) ) + margin.left;
						rect.attr('visibility', 'visible')
							.attr('x', px);
					}
					else
					{
						rect.attr('visibility', 'hidden');
					}
				}
			});

			// -- --
			var arrFps = Array();

			var maxFps = d3.max(stat.timeline, function(d) {
				// params->fpsの位置は変わらない前提
				var arr = d.params[0]['value'].split(' ');
				arrFps.push(arr);
				return Number(arr[1]);
			});

			var arrDelta = Array();

			var maxDelta = d3.max(stat.timeline, function(d) {
				var arr = Array();
				var obj = Array();
				obj.push(d.params[1]['value'].split(' '));
				obj.push(d.params[2]['value'].split(' '));
				obj.push(d.params[3]['value'].split(' '));
				obj.push(d.params[4]['value'].split(' '));
				Array.prototype.push.apply(arr, obj[0]);
				Array.prototype.push.apply(arr, obj[1]);
				Array.prototype.push.apply(arr, obj[2]);
				Array.prototype.push.apply(arr, obj[3]);
				arrDelta.push(obj);
				return d3.max(arr, function(d) { return Number(d); });
			});

			var arrFpsLen = arrFps.length;

			var fpsScale = d3.scale.linear()
								   .domain([0, maxFps])
								   .range([0, height-margin.bottom-margin.top]);

			var deltaScale = d3.scale.linear()
									 .domain([0, maxDelta])
									 .range([0, height-margin.bottom-margin.top]);
			var circles;
			var dd = '';

			var drawArr = [1,0,2];
			var drawColor = [
				[ '#444', '#666', '#888' ],				// frame
				[ '#f87f00', '#ff7f20', '#ffc060' ],	// game
				[ '#2040ff', '#2060ff', '#40a8ff' ],	// render
				[ '#00ff00', '#40ff40', '#7fff7f' ]		// gpu
			];
			var bw = ( ( width - margin.left - margin.right ) / arrFpsLen ) / 5;

			for( var ii = 0; ii < arrFpsLen; ii++ )
			{
				for( var jj = 0; jj < 4; jj++ )
				{
					for( var kk = 0; kk < 3; kk++ )
					{
						var val = arrDelta[ii][jj][drawArr[kk]];

						svg.append('rect')
						   .attr('x', ( ( ii * (bw*5) + jj * bw ) + margin.left ))
						   .attr('y', ( ( height - margin.bottom ) - deltaScale(Number(val)) ))
						   .attr('width', bw)
						   .attr('height', deltaScale(Number(val)))
						   .attr('fill', function() {
								return drawColor[jj][2-kk];
						   });
					}
				}
			}

			// -- 最大 --
			var g_max = svg.append('g');

			circles = g_max.selectAll('circle')
						   .data(arrFps)
						   .enter()
						   .append('circle')
						   .attr('cx', function(d,i) {
						      return ( i * ( ( width - margin.left - margin.right ) / arrFpsLen ) + margin.left );
						   })
						   .attr('cy', function(d,i) {
						      return ( ( height - margin.bottom ) - fpsScale( Number(d[1]) ) );
						   })
						   .attr('r', 2)
						   .attr('fill', 'fuchsia');

			dd = 'M ' + circles[0][0].attributes.cx.value + ' ' + circles[0][0].attributes.cy.value + ' ';
			for( var ii = 1; ii < arrFpsLen; ii++ )
			{
				dd = dd + 'L ' + circles[0][ii].attributes.cx.value + ' ' + circles[0][ii].attributes.cy.value + ' ';
			}
			g_max.append('path')
				 .attr('fill', 'none')
				 .attr('stroke', 'fuchsia')
				 .attr('d', dd);

			// -- 平均 --
			var g_avg = svg.append('g');

			circles = g_avg.selectAll('circle')
						   .data(arrFps)
						   .enter()
						   .append('circle')
						   .attr('cx', function(d,i) {
						      return i * ( ( width - margin.left - margin.right ) / arrFpsLen ) + margin.left;
						   })
						   .attr('cy', function(d,i) {
						      return ( height - margin.bottom ) - fpsScale( Number(d[0]) );
						   })
						   .attr('r', 2)
						   .attr('fill', 'blue');

			dd = 'M ' + circles[0][0].attributes.cx.value + ' ' + circles[0][0].attributes.cy.value + ' ';
			for( var ii = 1; ii < arrFpsLen; ii++ )
			{
				dd = dd + 'L ' + circles[0][ii].attributes.cx.value + ' ' + circles[0][ii].attributes.cy.value + ' ';
			}
			g_avg.append('path')
				 .attr('fill', 'none')
				 .attr('stroke', 'blue')
				 .attr('d', dd);

			// -- 最小 --
			var g_min = svg.append('g');

			circles = g_min.selectAll('circle')
						   .data(arrFps)
						   .enter()
						   .append('circle')
						   .attr('cx', function(d,i) {
						      return i * ( ( width - margin.left - margin.right ) / arrFpsLen ) + margin.left;
						   })
						   .attr('cy', function(d,i) {
						      return ( height - margin.bottom ) - fpsScale( Number(d[2]) );
						   })
						   .attr('r', 2)
						   .attr('fill', '#6e6e6e');

			dd = 'M ' + circles[0][0].attributes.cx.value + ' ' + circles[0][0].attributes.cy.value + ' ';
			for( var ii = 1; ii < arrFpsLen; ii++ )
			{
				dd = dd + 'L ' + circles[0][ii].attributes.cx.value + ' ' + circles[0][ii].attributes.cy.value + ' ';
			}
			g_min.append('path')
				 .attr('fill', 'none')
				 .attr('stroke', '#6e6e6e')
				 .attr('d', dd);



			// -- --
			var rect = svg.append('rect')
						  .attr('y', margin.top)
						  .attr('width', bw * 4)
						  .attr('height', height - margin.bottom - margin.top)
						  .attr('fill', 'rgba(0,0,0,0)')
						  .attr('stroke', '#f00')
						  .attr('visibility', 'hidden');


			// -- --
			var child = svg.append('svg')
						   .attr('x', 0)
						   .attr('y', 0)
						   .attr('width', 200)
						   .attr('height', 74)
						   .attr('visibility', 'hidden');

			child.append('rect')
				 .attr('x', '0%')
				 .attr('y', '0%')
				 .attr('rx', 4)
				 .attr('ry', 4)
				 .attr('width', '100%')
				 .attr('height', '100%')
				 .attr('fill', 'rgba(0,0,0,0.5)');

			child.append('text')
				.attr('x', 4)
				.attr('y', 14)
				.attr('fill', 'white')
				.text('fps');
			var txt_fps = child.append('text')
				.attr('x', 196)
				.attr('y', 14)
				.attr('fill', 'white')
				.attr('xml:space', 'preserve')
				.attr('text-anchor', 'end')
				.style('font-family', '"Osaka−等幅","ＭＳ ゴシック"');
			child.append('text')
				.attr('x', 4)
				.attr('y', 28)
				.attr('fill', 'white')
				.text('frame');
			var txt_frame = child.append('text')
				.attr('x', 196)
				.attr('y', 28)
				.attr('fill', 'white')
				.attr('xml:space', 'preserve')
				.attr('text-anchor', 'end')
				.style('font-family', '"Osaka−等幅","ＭＳ ゴシック"');
			child.append('text')
				.attr('x', 4)
				.attr('y', 42)
				.attr('fill', 'white')
				.text('game');
			var txt_game = child.append('text')
				.attr('x', 196)
				.attr('y', 42)
				.attr('fill', 'white')
				.attr('xml:space', 'preserve')
				.attr('text-anchor', 'end')
				.style('font-family', '"Osaka−等幅","ＭＳ ゴシック"');
			child.append('text')
				.attr('x', 4)
				.attr('y', 56)
				.attr('fill', 'white')
				.text('render');
			var txt_render = child.append('text')
				.attr('x', 196)
				.attr('y', 56)
				.attr('fill', 'white')
				.attr('xml:space', 'preserve')
				.attr('text-anchor', 'end')
				.style('font-family', '"Osaka−等幅","ＭＳ ゴシック"');
			child.append('text')
				.attr('x', 4)
				.attr('y', 70)
				.attr('fill', 'white')
				.text('gpu');
			var txt_gpu = child.append('text')
				.attr('x', 196)
				.attr('y', 70)
				.attr('fill', 'white')
				.attr('xml:space', 'preserve')
				.attr('text-anchor', 'end')
				.style('font-family', '"Osaka−等幅","ＭＳ ゴシック"');


//            // =====================================================================
//            // タイムラインフィルタ
//            var spawnedAndKilledTimeline = [];
//            var ticketTimeline = [];
//            var fobTimeline = [];
//            var hqTimeline = [];
//            var eventSwitcher = {
//              'spawned': spawnedAndKilledTimeline,
//              'killed': spawnedAndKilledTimeline,
//              'ticket': ticketTimeline,
//              'fob': fobTimeline,
//              'hq_broken': hqTimeline,
//              'hq_restored': hqTimeline
//            };
//            stat.timeline.forEach(function(event) {
//              if (event.event_name) {
//                var timeline = {};
//                event.params.forEach(function(param) {
//                  timeline[param.key] = param.value;
//                });
//                timeline.time = event.time;
//                timeline.event_name = event.event_name;
//
//                if (eventSwitcher[event.event_name]) {
//                  eventSwitcher[event.event_name].push(timeline);
//                }
//              }
//            });
//            // =====================================================================
//
//            // =====================================================================
//            // スポーン状況
//            var playersAlpha = stat.teams[0].players;
//            var playersBravo = stat.teams[1].players;
//
//            var maxTeamPlayerCount = Math.max(playersAlpha.length, playersBravo.length);
//            // ALPHAは上に伸ばす
//            var yCenter = ((height - (margin.top + margin.bottom)) / 2) + margin.top;
//            var yTeamAlphaSpawnScale = d3.scale.linear()
//                                               .domain([0, maxTeamPlayerCount])
//                                               .range([yCenter, margin.top]);
//            var yTeamBravoSpawnScale = d3.scale.linear()
//                                               .domain([0, maxTeamPlayerCount])
//                                               .range([yCenter, height - margin.bottom]);
//            var spawnedAndKilledTimelineCounterMapper = {
//              counter: 0,
//              func: function(element/*, index, array*/) {
//                return {
//                  time: element.time,
//                  name: element.name,
//                  character: element.character,
//                  event_name: element.event_name,
//                  killer: element.event_name === 'killed' ? element.killer_name : '',
//                  causer: element.event_name === 'killed' ? element.causer : '',
//                  count: element.event_name === 'spawned' ? ++this.counter : --this.counter
//                };
//              },
//              reset: function() {
//                this.counter = 0;
//              }
//            };
//            var teamAlphaTimeline = spawnedAndKilledTimeline.filter(function(element/*, index, array*/) {
//              return stat.teams[0].players.find(function(inner_element) {
//                return (inner_element.name === element.name) || (inner_element.name === element.killed_name);
//              });
//            });
//            var teamBravoTimeline = spawnedAndKilledTimeline.filter(function(element/*, index, array*/) {
//              return stat.teams[1].players.find(function(inner_element) {
//                return (inner_element.name === element.name) || (inner_element.name === element.killed_name);
//              });
//            });
//            teamAlphaTimeline = teamAlphaTimeline.map(spawnedAndKilledTimelineCounterMapper.func, spawnedAndKilledTimelineCounterMapper);
//            teamAlphaTimeline.unshift({ time: 0, count: 0 });
//            teamAlphaTimeline.push({ time: stat.gametime, count: teamAlphaTimeline[teamAlphaTimeline.length - 1].count });
//            spawnedAndKilledTimelineCounterMapper.reset();
//            teamBravoTimeline = teamBravoTimeline.map(spawnedAndKilledTimelineCounterMapper.func, spawnedAndKilledTimelineCounterMapper);
//            teamBravoTimeline.unshift({ time: 0, count: 0 });
//            teamBravoTimeline.push({ time: stat.gametime, count: teamAlphaTimeline[teamAlphaTimeline.length - 1].count });
//
//            var teamAlphaArea = d3.svg.area()
//                                      .x(function(d) {
//                                        return xTimelineScale(d.time);
//                                      })
//                                      .y0(function() { return yCenter; })
//                                      .y1(function(d) {
//                                        return yTeamAlphaSpawnScale(d.count);
//                                      })
//                                      .interpolate('step-after');
//            svg.append('path')
//               .attr('d', teamAlphaArea(teamAlphaTimeline))
//               .attr('fill', 'aqua')
//               .attr('opacity', '0.1')
//               .attr('transform', 'translate(' + margin.left + ', ' + '0)');
//            var teamBravoArea = d3.svg.area()
//                                      .x(function(d) {
//                                        return xTimelineScale(d.time);
//                                      })
//                                      .y0(function() { return yCenter; })
//                                      .y1(function(d) {
//                                        return yTeamBravoSpawnScale(d.count);
//                                      })
//                                      .interpolate('step-after');
//            svg.append('path')
//               .attr('d', teamBravoArea(teamBravoTimeline))
//               .attr('fill', 'deeppink')
//               .attr('opacity', '0.1')
//               .attr('transform', 'translate(' + margin.left + ', ' + '0)');
//
//            //===========================================================================
//            // マウスオーバーで情報表示（仮）
//            var appendSpawnAndDeathInfo = function(dataset, yScale) {
//              svg.selectAll('circle')
//                 .data(dataset)
//                 .enter()
//                 .append('circle')
//                 .attr('cx', function(d) {
//                   return xTimelineScale(d.time);
//                 })
//                 .attr('cy', function(d) {
//                   return yScale(d.count);
//                 })
//                 .attr('r', 5)
//                 .attr('opacity', 0.3)
//                 .attr('fill', function(d) {
//                   return d.event_name === 'spawned' ? 'orangered' : 'dimgray';
//                 })
//                 .on('mouseover', function(d) {
//                   return tooltip.style('visibility', 'visible').text(function() {
//                     var message = '';
//                     if (d.event_name === 'spawned') {
//                       message = d.event_name + '\n' + d.name + '\n' + d.character;
//                     } else if (d.event_name === 'killed') {
//                       message = d.event_name + '\n' + d.name + '\n' + d.character + '\nby ' + d.killer + ' : ' + d.causer;
//                     }
//                     return message;
//                   });
//                 })
//                 .on('mousemove', function() {
//                   return tooltip.style('top', (event.pageY - 20) + 'px')
//                   .style('left', (event.pageX + 10) + 'px');
//                 })
//                 .on('mouseout', function() {
//                   return tooltip.style('visibility', 'hidden');
//                 })
//                 .attr('transform', 'translate(' + margin.left + ', ' + '0)')
//                 ;
//            };
//            appendSpawnAndDeathInfo(teamAlphaTimeline, yTeamAlphaSpawnScale);
//            appendSpawnAndDeathInfo(teamBravoTimeline, yTeamBravoSpawnScale);
//            //===========================================================================
//
//            var tickValues = d3.range(maxTeamPlayerCount + 1);
//            tickValues.shift();
//            var yTeamAlphaAxis = d3.svg.axis()
//                                       .scale(yTeamAlphaSpawnScale)
//                                       .orient('left')
//                                       .tickValues(tickValues)
//                                       .tickFormat(d3.format('d'));
//            var yTeamBravoAxis = d3.svg.axis()
//                                       .scale(yTeamBravoSpawnScale)
//                                       .orient('left')
//                                       .tickValues(tickValues)
//                                       .tickFormat(d3.format('d'));
//            svg.append('g')
//               .classed('y team alpha axis', true)
//               .call(yTeamAlphaAxis)
//               .attr('transform', 'translate(' + (width - margin.right) + ', ' + '0)');
//            svg.append('g')
//               .classed('y team bravo axis', true)
//               .call(yTeamBravoAxis)
//               .attr('transform', 'translate(' + (width - margin.right) + ', ' + '0)');
//
//
//            // =====================================================================
//
//            // =====================================================================
//            // チケット状況
//            var yTicketScale = d3.scale.linear()
//                                       .domain([stat.initial_ticket, 0])
//                                       .range([0, height - (margin.top + margin.bottom)]);
//            var yTicketAxis = d3.svg.axis()
//                                    .scale(yTicketScale)
//                                    .orient('left')
//                                    .ticks(5);
//            var teamAlphaTicket = ticketTimeline.filter(function(element/*, index, array*/) {
//              return element.team === '0';
//            });
//            var teamBravoTicket = ticketTimeline.filter(function(element/*, index, array*/) {
//              return element.team === '1';
//            });
//            var lineTicket = d3.svg.line()
//                                   .x(function(d) {
//                                     return xTimelineScale(d.time) + margin.left;
//                                   })
//                                   .y(function(d) {
//                                     return yTicketScale(d.ticket) + margin.top;
//                                   })
//                                   .interpolate('linear');
//            svg.append('g')
//               .classed('y ticket axis', true)
//               .call(yTicketAxis)
//               .attr('transform', 'translate(' + margin.left + ', ' + margin.top + ')');
//            svg.append('path')
//               .attr('d', lineTicket(teamAlphaTicket))
//               .classed('line', true)
//               .attr('stroke', 'blue')
//               .attr('stroke-width', 2)
//               .attr('fill', 'none');
//            svg.append('path')
//               .attr('d', lineTicket(teamBravoTicket))
//               .classed('line', true)
//               .attr('stroke', 'red')
//               .attr('stroke-width', 2)
//               .attr('fill', 'none');
//            // =====================================================================

          }; // scope.render
        });
      }
    };
  }]);
