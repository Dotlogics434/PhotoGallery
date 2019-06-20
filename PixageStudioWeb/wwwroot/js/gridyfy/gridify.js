/**
 * Created by khanhnh on 13/09/2014.
 */
'use strict';
var $qb = jQuery.noConflict();
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], factory);
    } else {
        factory(jQuery);
    }
}(function ($qb) {
    $qb.fn.extend({
        imagesLoaded: function(cb)
        {
            var images = $qb(this).find('img');
            var count = images.length;
            if (count == 0) cb();
            for(var i = 0, length = images.length; i< length; i++)
            {
                var image = new Image();
                image.onload = image.onerror = function(e){
                    count --;
                    if (count == 0) cb()
                }
                image.src = images[i].src;
            }
        },
        gridify: function(options) {
            var $qbthis = $qb(this),
                options = options || {},
                indexOfSmallest = function (a) {
                    var lowest = 0;
                    for (var i = 1, length = a.length; i < length; i++) {
                        if (a[i] < a[lowest]) lowest = i;
                    }
                    return lowest;
                },
                render = function()
                {
                    $qbthis.css('position', 'relative');
                    var items = $qbthis.find(options.srcNode),
                        transition = (options.transition || 'all 0.5s ease') + ', height 0, width 0',
                        width = $qbthis.innerWidth(),
                        item_margin = parseInt(options.margin || 0),
                        item_width = parseInt(options.max_width || options.width || 220),
                        column_count = Math.max(Math.floor(width/(item_width + item_margin)),1),
                        left = column_count == 1 ? item_margin/2 : (width % (item_width + item_margin)) / 2,
                        columns = [];

                    if (options.max_width) {
                        column_count = Math.ceil(width/(item_width + item_margin));
                        item_width = (width - column_count * item_margin - item_margin)/column_count;
                        left = item_margin/2;
                    }

                    for (var i = 0; i < column_count; i++) {
                        columns.push(0);
                    }

                    for(var i = 0, length = items.length; i< length; i++)
                    {
                        var $qbitem = $qb(items[i]), idx = indexOfSmallest(columns);
                        $qbitem.css({
                            width: item_width,
                            position: 'absolute',
                            margin: item_margin/2,
                            top: columns[idx] + item_margin/2,
                            left: (item_width + item_margin) * idx + left,
                            transition: transition
                        });
                        columns[idx] += $qbitem.innerHeight() + item_margin;
                    }
                };

            $qbthis.imagesLoaded(render);
            if (options.resizable) {
                var resize =  $qb(window).bind("resize", render);
                $qbthis.on('remove', resize.unbind);
            }
        }
    });
}));
